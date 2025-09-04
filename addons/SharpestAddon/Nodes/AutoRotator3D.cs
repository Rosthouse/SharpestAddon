using Godot;

namespace rosthouse.sharpest.addon;

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
    Rotate(delta);
  }

  public override void _PhysicsProcess(double delta)
  {
    if (!physicsProcess)
    {
      return;
    }

    Rotate(delta);
  }

  private void Rotate(double delta)
  {
    var parent = GetParent<Node3D>();
    parent.Quaternion *= Quaternion.FromEuler(amount * (float)delta);
  }
}
