

using Godot;

public static class Node3DExtensions
{

  public static void RotateAround(this Node3D n, Vector3 point, Vector3 axis, float angle)
  {
    var rot = angle + n.Rotation.Y;
    var tStart = point;
    n.GlobalTranslate(-tStart);
    n.Transform = n.Transform.Rotated(axis, -rot);
    n.GlobalTranslate(tStart);
  }
}
