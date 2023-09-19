

using System.Globalization;
using Godot;

public static class Node3DExtensions
{

  public static void RotateAround(this Node3D n, Vector3 pivot, Vector3 axis, float angle)
  {
    var q = new Quaternion(axis, angle);
    n.GlobalPosition = q * (n.GlobalPosition - pivot) + pivot;
    n.GlobalRotation = q * n.GlobalRotation;
  }

  public static void LookAtOnAxis(this Node3D n, Vector3 target, Vector3 axis)
  {
    Vector3 targetPostition = new Vector3(
      axis.X > 0 ? n.GlobalPosition.X : target.X,
      axis.Y > 0 ? n.GlobalPosition.Y : target.Y,
      axis.Z > 0 ? n.GlobalPosition.Z : target.Z
    );
    n.LookAt(targetPostition);
  }
}
