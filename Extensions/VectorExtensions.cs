using System;
using Godot;

public static class VectorExtensions
{

  public static int Xint(this Vector2 v)
  {
    return (int)v.X;
  }

  public static int Yint(this Vector2 v)
  {
    return (int)v.Y;
  }

  public static Vector2 GetFourWayDirection(int x)
  {
    switch (x)
    {
      case 0:
        return Vector2.Right;
      case 1:
        return Vector2.Up;
      case 2:
        return Vector2.Left;
      case 3:
        return Vector2.Down;
      default:
        throw new Exception($"Expected value between 0 and 3, got {x}");
    }
  }

  public static Vector2I RountToInt(this Vector2 v)
  {
    return new Vector2I(Mathf.RoundToInt(v.X), Mathf.RoundToInt(v.Y));
  }

  public static Vector3I RoundToInt(this Vector3 v){
	return new Vector3I(
		Mathf.RoundToInt(v.X),
		Mathf.RoundToInt(v.Y),
		Mathf.RoundToInt(v.Z)
	);
  }

  public static Vector4I RoundToInt(this Vector4 v){
	return new Vector4I(
		Mathf.RoundToInt(v.X),
		Mathf.RoundToInt(v.Y),
		Mathf.RoundToInt(v.Z),
		Mathf.RoundToInt(v.W)
	);
  }
}