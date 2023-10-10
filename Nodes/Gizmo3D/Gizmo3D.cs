using Godot;
using System;
using System.Reflection;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;

namespace rosthouse.sharpest.addon
{
  public partial class Gizmo3D : Node3D
  {
    public static uint MASK = (uint)1 << 31;
    public static Gizmo3D Create(Node owner = null)
    {
      var g = GD.Load<PackedScene>("res://addons/SharpestAddon/Nodes/Gizmo3D/gizmo_3d.tscn").Instantiate<Gizmo3D>();
      if (owner != null)
      {
        g.Owner = owner;
      }
      return g;
    }

    public enum GizmoActionType
    {
      NONE,
      MOVE, ROTATE,
    }

    [Signal] public delegate void MovedEventHandler(Vector3 movment);
    [Signal] public delegate void RotatedEventHandler(Quaternion rotation);
    [Export] public float Scaling { get; private set; } = 1f;
    [Export] public float TranslateSpeed { get; set; } = 0.01f;
    [Export] public NodePath Remote { get => GetNode<RemoteTransform3D>("%RemoteTransform").RemotePath; set => GetNode<RemoteTransform3D>("%RemoteTransform").RemotePath = value; }
    private GizmoActionType mode = GizmoActionType.NONE;
    private Vector3 currentNormal;
    private MeshInstance3D currentMesh;

    [Export]
    public GizmoActionType Mode
    {
      get => this.mode; private set
      {
        this.mode = value;
        this.SetHandleVisibility(value);
      }
    }
    private Vector2 rotationMouseMask;
    // private DrawLayer cvl;
    private Node3D translateHandles;
    private Node3D rotateHandles;
    // private Vector3 _Rotation;

    // public Vector3 Translation { get; private set; } = Vector3.Zero;
    public float RotationSpeed { get; private set; } = 1f;

    public override void _Ready()
    {
      base._Ready();

      this.translateHandles = GetNode<Node3D>("Translate");
      this.rotateHandles = GetNode<Node3D>("Rotate");

      GetNode<Area3D>("%XAxis").InputEvent += (_, @event, _, _, _) => this.HandleTranslateClick(@event, GetNode<MeshInstance3D>("%XAxis/CollisionShape3D/MeshInstance3D"));
      GetNode<Area3D>("%YAxis").InputEvent += (_, @event, _, _, _) => this.HandleTranslateClick(@event, GetNode<MeshInstance3D>("%YAxis/CollisionShape3D/MeshInstance3D"));
      GetNode<Area3D>("%ZAxis").InputEvent += (_, @event, _, _, _) => this.HandleTranslateClick(@event, GetNode<MeshInstance3D>("%ZAxis/CollisionShape3D/MeshInstance3D"));

      GetNode<Area3D>("%XPlane").InputEvent += (_, @event, _, _, _) => this.HandleRotateClick(@event, GetNode<MeshInstance3D>("%XPlane/CollisionShape3D/MeshInstance3D"), Vector3.Right);
      GetNode<Area3D>("%YPlane").InputEvent += (_, @event, _, _, _) => this.HandleRotateClick(@event, GetNode<MeshInstance3D>("%YPlane/CollisionShape3D/MeshInstance3D"), Vector3.Up);
      GetNode<Area3D>("%ZPlane").InputEvent += (_, @event, _, _, _) => this.HandleRotateClick(@event, GetNode<MeshInstance3D>("%ZPlane/CollisionShape3D/MeshInstance3D"), Vector3.Back);

      GetNode<Button>("%MoveBtn").Pressed += () => SetHandleVisibility(GizmoActionType.MOVE);
      GetNode<Button>("%RotateBtn").Pressed += () => SetHandleVisibility(GizmoActionType.ROTATE);
      this.SetHandleVisibility(this.mode);
    }


    public override void _Input(InputEvent @event)
    {
      if (Input.IsActionJustReleased("ui_left_click"))
      {
        GD.Print("released");
        this.mode = GizmoActionType.NONE;
        this.currentMesh = null;
      }
    }


    Transform3D originalTransform = new();
    Vector2 dragStartPosition = new(0, 0);
    public override void _Process(double delta)
    {
      if (!this.Visible)
      {
        return;
      }

      var size = GetViewport().GetCamera3D().Size / Scaling;
      this.Scale = new(size, size, size);

      // this.dragStartPosition = GetViewport().GetMousePosition();

      switch (mode)
      {
        case GizmoActionType.MOVE:
          this.HandleTranslation(this.currentMesh);
          GetViewport().SetInputAsHandled();
          break;
        case GizmoActionType.ROTATE:
          this.HandleRotation(this.currentMesh, this.currentNormal);
          GetViewport().SetInputAsHandled();
          break;
        case GizmoActionType.NONE:
          break;
      }
      this.originalTransform = this.GlobalTransform;
      this.dragStartPosition = GetViewport().GetMousePosition();
    }



    private void SetHandleVisibility(GizmoActionType value)
    {

      if (translateHandles == null || rotateHandles == null)
      {
        return;
      }
      this.translateHandles.Visible = false;
      this.rotateHandles.Visible = false;

      switch (value)
      {
        case GizmoActionType.MOVE:
          this.translateHandles.Visible = true;
          break;
        case GizmoActionType.ROTATE:
          this.rotateHandles.Visible = true;
          break;
        case GizmoActionType.NONE:
          // do nothing
          break;
      }
    }

    private void HandleTranslateClick(InputEvent @event, MeshInstance3D mesh)
    {

      if (@event is not InputEventMouseButton)
      {
        return;
      }

      if (Input.IsActionJustPressed("ui_left_click") && !@event.IsEcho())
      {
        GD.Print("clicked translate");
        var ev = @event as InputEventMouseButton;
        originalTransform = this.GlobalTransform;
        this.dragStartPosition = ev.Position;
        this.mode = GizmoActionType.MOVE;
        this.currentMesh = mesh;
      }
    }

    private void HandleRotateClick(InputEvent @event, MeshInstance3D mesh, Vector3 normal)
    {
      if (@event is not InputEventMouseButton)
      {
        return;
      }
      if (@event.IsActionPressed("ui_left_click") && !@event.IsEcho())
      {
        var ev = @event as InputEventMouseButton;
        this.dragStartPosition = ev.Position;
        this.mode = GizmoActionType.ROTATE;
        this.currentNormal = normal;
        this.currentMesh = mesh;
      }

    }

    public void HandleTranslation(MeshInstance3D mesh)
    {
      var cam = GetViewport().GetCamera3D();
      var mp = GetViewport().GetMousePosition();
      var step = (cam.UnprojectPosition(mesh.GlobalPosition) - cam.UnprojectPosition(this.GlobalPosition)).Normalized();
      this.GlobalTransform = originalTransform;

      var dis = mp - dragStartPosition;
      var output = step * dis;

      var dir = (mesh.GlobalPosition - this.GlobalPosition).Normalized();

      var diff = dir * (output.X + output.Y) / 15f;

      GD.Print($"Diff{diff}");

      this.TranslateObjectLocal(diff);
    }

    public void HandleRotation(MeshInstance3D mesh, Vector3 normal)
    {
      // material_override.albedo_color.a8 = 200
      var mp = GetViewport().GetMousePosition();
      var parentCenter = GetViewport().GetCamera3D().UnprojectPosition(this.GlobalPosition);
      var start = parentCenter.AngleToPoint(dragStartPosition);
      var angle = parentCenter.AngleToPoint(mp);
      var dir = (GetViewport().GetCamera3D().GlobalPosition - this.GlobalPosition).Normalized();
      this.GlobalTransform = originalTransform;

      if (normal.Dot(dir) > 0)
      {
        this.RotateObjectLocal(normal, start - angle);
      }
      else
      {
        this.RotateObjectLocal(normal, angle - start);
      }
    }

  }

}
