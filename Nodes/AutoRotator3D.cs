using Godot;
using System;

/// <summary>
/// Rotates its parent by the amount given as parameters.
/// </summary>
[GlobalClass]
public partial class AutoRotator3D : Node3D
{
  [Export] public Vector3 amount;
  [Export] private bool physicsProcess;

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta)
  {
    if (physicsProcess)
    {
      return;
    }
    this.Rotate(delta);
  }

  public override void _PhysicsProcess(double delta)
  {
    if (!physicsProcess)
    {
      return;
    }

    this.Rotate(delta);
  }

  private void Rotate(double delta)
  {
    var parent = this.GetParent<Node3D>();
    parent.Quaternion *= Quaternion.FromEuler(this.amount * (float)delta);
  }
}
