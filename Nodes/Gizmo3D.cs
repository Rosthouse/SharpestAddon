using Godot;
using System;
using System.Reflection;

public partial class Gizmo3D : Node3D
{
  public enum GizmoActionType
  {
    MOVE, ROTATE
  }

  [Export] public Node3D Reference { get; set; } = null;
  [Export] public float TranslateSpeed { get; set; } = 0.1f;
  public GizmoActionType Mode { get; private set; } = GizmoActionType.MOVE;

  private Vector3 transformDirection = Vector3.Inf;
  private DrawLayer cvl;

  public override void _Ready()
  {
    base._Ready();

    GetNode<Area3D>("%XAxis").InputEvent += (Node camera, InputEvent @event, Vector3 position, Vector3 normal, long shapeIdx) => this.MoveGizmo(@event, Vector3.Right);
    GetNode<Area3D>("%YAxis").InputEvent += (Node camera, InputEvent @event, Vector3 position, Vector3 normal, long shapeIdx) => this.MoveGizmo(@event, Vector3.Up);
    GetNode<Area3D>("%ZAxis").InputEvent += (Node camera, InputEvent @event, Vector3 position, Vector3 normal, long shapeIdx) => this.MoveGizmo(@event, Vector3.Back);

    GetNode<Button>("%MoveBtn").Pressed += () => this.Mode = GizmoActionType.MOVE;
    GetNode<Button>("%RotateBtn").Pressed += () => this.Mode = GizmoActionType.ROTATE;
    this.cvl = GetNode<DrawLayer>("%Brush");
  }

  public override void _Input(InputEvent @event)
  {

    base._Input(@event);
    GD.Print("Handling Gizmo input");
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
      var movement = Vector3.Zero;
      var mouseDelta = iemm.Relative * TranslateSpeed;

      movement += GetViewport().GetCamera3D().GlobalTransform.Basis.Y * -mouseDelta.Y;
      movement += GetViewport().GetCamera3D().GlobalTransform.Basis.X * mouseDelta.X;

      var invDirection = Vector3.One * this.transformDirection;
      movement *= invDirection;
      this.Reference.GlobalPosition += movement;
      this.GlobalPosition += movement;
    }
  }


  private void MoveGizmo(InputEvent @event, Vector3 direction)
  {

    if (Input.IsActionJustPressed("ui_left_click"))
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
    switch (this.Mode)
    {
      case GizmoActionType.MOVE:
        this.cvl.Arrow(this.Position, Vector3.Right, Colors.Red);
        this.cvl.Arrow(this.Position, Vector3.Up, Colors.Green);
        this.cvl.Arrow(this.Position, Vector3.Back, Colors.Blue);
        break;
      case GizmoActionType.ROTATE:
        this.cvl.Arc(this.Position, Vector3.Right, Colors.Red);
        this.cvl.Arc(this.Position, Vector3.Up, Colors.Green);
        this.cvl.Arc(this.Position, Vector3.Back, Colors.Blue);
        break;
    }
  }
}
