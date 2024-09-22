using Godot;
using System.Linq;
using System;
using System.Reflection;
using System.Reflection.Metadata;

namespace rosthouse.sharpest.addon
{
  public partial class Gizmo3D : Node3D
  {
    public static uint MASK = 32;
    public static Gizmo3D Create(Node owner = null)
    {
      var g = GD.Load<PackedScene>("res://addons/SharpestAddon/Nodes/Gizmo3D/gizmo_3d.tscn").Instantiate<Gizmo3D>();
      if (owner != null)
      {
        g.Owner = owner;
      }
      return g;
    }

    public enum ActionType
    {
      NONE,
      MOVE, ROTATE,
    }

    [Signal] public delegate void MovedEventHandler(Vector3 movment);
    [Signal] public delegate void RotatedEventHandler(Vector3 axis, float angle);
    [Export] public float Scaling { get; private set; } = 1f;
    [Export] public float TranslateSpeed { get; set; } = 0.01f;
    [Export] public NodePath Remote { get => GetNode<RemoteTransform3D>("%RemoteTransform").RemotePath; set => GetNode<RemoteTransform3D>("%RemoteTransform").RemotePath = value; }
    private Vector3 currentNormal;
    Vector2 dragStartPosition = new(0, 0);
    private Handle currentHandle;


    private Node3D translate;
    private Node3D rotate;
    private Control controls;
    private Node3D visuals;


    public override void _Ready()
    {
      base._Ready();

      this.visuals = GetNode<Node3D>("Visuals");

      this.translate = GetNode<Node3D>("Translate");
      this.rotate = GetNode<Node3D>("Rotate");

      this.controls = GetNode<Control>("%Controls");
      this.VisibilityChanged += () => this.Rotation = Vector3.Zero;
    }


    public override void _Input(InputEvent @event)
    {
      var st = (SceneTree)Engine.GetMainLoop();
      var vp = st.CurrentScene.GetViewport();

      if (@event is InputEventMouseButton ev)
      {

        if (Input.IsActionJustReleased("ui_left_click"))
        {
          GD.Print("released");
          if (this.currentHandle != null)
          {
            vp.SetInputAsHandled();
            this.currentHandle = null;
          }
        }

        if (Input.IsActionJustPressed("ui_left_click"))
        {
          var dss = PhysicsServer3D.SpaceGetDirectState(vp.World3D.Space);
          var res = dss.CastRayFromCamera(collisionMask: MASK, collideWithAreas: true);
          if (res.HasValue && res.Value.GetCollisionObject3D() is Handle h)
          {
            GD.Print($"Hit {res.Value.GetCollisionObject3D().Name}");
            vp.SetInputAsHandled();
            this.currentHandle = h;
            var mesh = res.Value.GetCollisionObject3D().GetNode<MeshInstance3D>("Mesh");
            switch (this.currentHandle.Mode)
            {
              case ActionType.MOVE:
                this.HandleTranslateClick(ev, mesh);
                break;
              case ActionType.ROTATE:
                this.HandleRotateClick(ev, mesh, res.Value.normal);
                break;
              case ActionType.NONE:
                break;
            }
          }
        }
      }

    }
    public override void _Process(double delta)
    {
      var size = (this.GlobalPosition - GetViewport().GetCamera3D().GlobalPosition).Length() * Scaling;
      Vector3 scale = new(size, size, size);
      this.translate.Scale = scale;
      this.rotate.Scale = scale;

      if (this.currentHandle == null)
      {
        return;
      }

      switch (this.currentHandle.Mode)
      {
        case ActionType.MOVE:
          this.HandleTranslation(this.currentHandle);
          GetViewport().SetInputAsHandled();
          break;
        case ActionType.ROTATE:
          this.HandleRotation(this.currentHandle, this.currentNormal);
          GetViewport().SetInputAsHandled();
          break;
        case ActionType.NONE:
          break;
      }
      this.dragStartPosition = GetViewport().GetMousePosition();
    }

    private void HandleTranslateClick(InputEventMouseButton @event, MeshInstance3D mesh)
    {
      if (Input.IsActionJustPressed("ui_left_click") && !@event.IsEcho())
      {
        GD.Print("clicked translate");
        this.dragStartPosition = @event.Position;
      }
    }

    private void HandleRotateClick(InputEventMouseButton @event, MeshInstance3D mesh, Vector3 normal)
    {
      if (@event.IsActionPressed("ui_left_click") && !@event.IsEcho())
      {
        this.dragStartPosition = @event.Position;
        this.currentNormal = normal;
      }
    }

    public void HandleTranslation(Handle h)
    {
      var cam = GetViewport().GetCamera3D();
      var mp = GetViewport().GetMousePosition();
      var step = (cam.UnprojectPosition(h.GlobalPosition) - cam.UnprojectPosition(this.GlobalPosition)).Normalized();

      var dis = mp - dragStartPosition;
      var output = step * dis;

      var dir = h.Position.Normalized();

      var diff = dir * (output.X + output.Y) / 15f;

      GD.Print($"Diff{diff}");

      this.Translate(diff);
      this.EmitSignal(nameof(Moved), diff);
    }

    public void HandleRotation(Handle h, Vector3 normal)
    {
      // material_override.albedo_color.a8 = 200
      var mp = GetViewport().GetMousePosition();
      var parentCenter = GetViewport().GetCamera3D().UnprojectPosition(this.GlobalPosition);
      var start = parentCenter.AngleToPoint(dragStartPosition);
      var angle = parentCenter.AngleToPoint(mp);
      var dir = (GetViewport().GetCamera3D().GlobalPosition - this.GlobalPosition).Normalized();

      var rotAngle = normal.Dot(dir) > 0 ? start - angle : angle - start;

      this.RotateObjectLocal(normal, rotAngle);
      this.EmitSignal(nameof(Rotated), normal, rotAngle);
    }

  }

}
