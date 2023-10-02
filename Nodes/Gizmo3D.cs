using Godot;
using System;
using System.Reflection;

namespace rosthouse.sharpest.addon
{
  public partial class Gizmo3D : Node3D
  {
    public static uint MASK = (uint)1 << 31;
    public static Gizmo3D Create(Node owner = null)
    {
      var g = GD.Load<PackedScene>("res://addons/SharpestAddon/Nodes/gizmo_3d.tscn").Instantiate<Gizmo3D>();
      if (owner != null)
      {
        g.Owner = owner;
      }
      return g;
    }
    public enum GizmoActionType
    {
      MOVE, ROTATE
    }

    [Signal] public delegate void MovedEventHandler(Vector3 movment);
    [Signal] public delegate void RotatedEventHandler(Vector3 rotation);
    [Export] public float TranslateSpeed { get; set; } = 0.01f;
    private GizmoActionType mode = GizmoActionType.MOVE;
    [Export]
    public GizmoActionType Mode
    {
      get => this.mode; private set
      {
        this.mode = value;
        this.SetHandleVisibility(value);
      }
    }


    private Vector3 transformDirection = Vector3.Inf;
    private Vector3 rotateDirection = Vector3.Inf;
    private DrawLayer cvl;
    private Node3D translateHandles;
    private Node3D rotateHandles;

    public Vector3 Translation { get; private set; } = Vector3.Zero;
    public Vector3 RotationDelta { get; private set; } = Vector3.Zero;
    public Vector2 RotateSpeed { get; private set; }

    public override void _Ready()
    {
      base._Ready();

      this.translateHandles = GetNode<Node3D>("Translate");
      this.rotateHandles = GetNode<Node3D>("Rotate");

      GetNode<Area3D>("%XAxis").InputEvent += (Node camera, InputEvent @event, Vector3 position, Vector3 normal, long shapeIdx) => this.TranslateGizmo(@event, Vector3.Right);
      GetNode<Area3D>("%YAxis").InputEvent += (Node camera, InputEvent @event, Vector3 position, Vector3 normal, long shapeIdx) => this.TranslateGizmo(@event, Vector3.Up);
      GetNode<Area3D>("%ZAxis").InputEvent += (Node camera, InputEvent @event, Vector3 position, Vector3 normal, long shapeIdx) => this.TranslateGizmo(@event, Vector3.Back);

      GetNode<Area3D>("%XPlane").InputEvent += (Node camera, InputEvent @event, Vector3 position, Vector3 normal, long shapeIdx) => this.RotateGizmo(@event, Vector3.Right);
      GetNode<Area3D>("%YPlane").InputEvent += (Node camera, InputEvent @event, Vector3 position, Vector3 normal, long shapeIdx) => this.RotateGizmo(@event, Vector3.Up);
      GetNode<Area3D>("%ZPlane").InputEvent += (Node camera, InputEvent @event, Vector3 position, Vector3 normal, long shapeIdx) => this.RotateGizmo(@event, Vector3.Back);

      GetNode<Button>("%MoveBtn").Pressed += () => this.Mode = GizmoActionType.MOVE;
      GetNode<Button>("%RotateBtn").Pressed += () => this.Mode = GizmoActionType.ROTATE;
      this.cvl = GetNode<DrawLayer>("%Brush");

      this.VisibilityChanged += () => this.cvl.QueueRedraw();
    }

    public override void _Input(InputEvent @event)
    {

      base._Input(@event);
      if (@event.IsActionReleased("ui_left_click"))
      {
        this.transformDirection = Vector3.Inf;
        this.rotateDirection = Vector3.Inf;
      }

      if (this.transformDirection == Vector3.Inf)
      {
        return;
      }

      if (@event is InputEventMouseMotion iemm)
      {
        this.Translation = Vector3.Zero;
        var mouseDelta = iemm.Relative * TranslateSpeed;

        Translation += GetViewport().GetCamera3D().GlobalTransform.Basis.Y * -mouseDelta.Y;
        Translation += GetViewport().GetCamera3D().GlobalTransform.Basis.X * mouseDelta.X;

        var invDirection = Vector3.One * this.transformDirection;
        Translation *= invDirection;
        this.GlobalPosition += Translation;
        this.EmitSignal(nameof(Moved), Translation);
      }
    }


    public override void _Process(double delta)
    {
      if (!this.Visible)
      {
        return;
      }

      switch (this.Mode)
      {
        case GizmoActionType.MOVE:
          this.cvl.Arrow(this.GlobalPosition, Vector3.Right, Colors.Red, 5);
          this.cvl.Arrow(this.GlobalPosition, Vector3.Up, Colors.Green, 5);
          this.cvl.Arrow(this.GlobalPosition, Vector3.Back, Colors.Blue, 5);
          break;
        case GizmoActionType.ROTATE:
          // this.cvl.Disc(this.GlobalPosition, 50, Colors.Red);
          // this.cvl.Disc(this.GlobalPosition, 50, Colors.Green);
          // this.cvl.Disc(this.GlobalPosition, 50, Colors.Blue);
          break;
      }
    }

    private void SetHandleVisibility(GizmoActionType value)
    {
      switch (value)
      {
        case GizmoActionType.MOVE:
          this.translateHandles.Visible = true;
          this.rotateHandles.Visible = false;
          break;
        case GizmoActionType.ROTATE:
          this.translateHandles.Visible = false;
          this.rotateHandles.Visible = true;
          break;
      }
      this.cvl.QueueRedraw();
    }

    private void TranslateGizmo(InputEvent @event, Vector3 direction)
    {
      GD.Print($"Handling input for axis {direction}");

      if (@event.IsActionPressed("ui_left_click") && !@event.IsEcho())
      {
        GD.Print($"Clicking axis {direction}");
        this.transformDirection = direction;
      }
      if (@event.IsActionReleased("ui_left_click"))
      {
        GD.Print($"Released {direction}");
        this.transformDirection = Vector3.Inf;
      }
    }


    private void RotateGizmo(InputEvent @event, Vector3 normal)
    {
      GD.Print($"Handling input for plane {normal}");

      if (@event.IsActionPressed("ui_left_click") && !@event.IsEcho())
      {
        GD.Print($"Clicking plane {normal}");
        this.rotateDirection = normal;
      }
      if (@event.IsActionReleased("ui_left_click"))
      {
        GD.Print($"Released {normal}");
        this.rotateDirectionx = Vector3.Inf;
      }
    }

  }

}
