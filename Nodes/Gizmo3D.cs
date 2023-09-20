using Godot;
using System;
using System.Reflection;

namespace rosthouse.sharpest.addons
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
    [Export] public float TranslateSpeed { get; set; } = 0.01f;
    public GizmoActionType Mode { get; private set; } = GizmoActionType.MOVE;

    private Vector3 transformDirection = Vector3.Inf;
    private DrawLayer cvl;
    public Vector3 Movement { get; private set; } = Vector3.Zero;

    public override void _Ready()
    {
      base._Ready();

      GetNode<Area3D>("%XAxis").InputEvent += (Node camera, InputEvent @event, Vector3 position, Vector3 normal, long shapeIdx) => this.MoveGizmo(@event, Vector3.Right);
      GetNode<Area3D>("%YAxis").InputEvent += (Node camera, InputEvent @event, Vector3 position, Vector3 normal, long shapeIdx) => this.MoveGizmo(@event, Vector3.Up);
      GetNode<Area3D>("%ZAxis").InputEvent += (Node camera, InputEvent @event, Vector3 position, Vector3 normal, long shapeIdx) => this.MoveGizmo(@event, Vector3.Back);

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
      }

      if (this.transformDirection == Vector3.Inf)
      {
        return;
      }

      if (@event is InputEventMouseMotion iemm)
      {
        this.Movement = Vector3.Zero;
        var mouseDelta = iemm.Relative * TranslateSpeed;

        Movement += GetViewport().GetCamera3D().GlobalTransform.Basis.Y * -mouseDelta.Y;
        Movement += GetViewport().GetCamera3D().GlobalTransform.Basis.X * mouseDelta.X;

        var invDirection = Vector3.One * this.transformDirection;
        Movement *= invDirection;
        this.GlobalPosition += Movement;
        this.EmitSignal(nameof(Moved), Movement);
      }
    }


    private void MoveGizmo(InputEvent @event, Vector3 direction)
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

    public override void _Process(double delta)
    {
      if(!this.Visible){
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
          this.cvl.Arc(this.GlobalPosition, Vector3.Right, Colors.Red);
          this.cvl.Arc(this.GlobalPosition, Vector3.Up, Colors.Green);
          this.cvl.Arc(this.GlobalPosition, Vector3.Back, Colors.Blue);
          break;
      }
    }
  }

}
