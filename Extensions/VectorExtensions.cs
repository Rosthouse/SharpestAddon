using System;
using Godot;

public static class VectorExtensions
{

	public static int Xint(this Vector2 v)
	{
		return (int)v.x;
	}

	public static int Yint(this Vector2 v)
	{
		return (int)v.y;
	}

	public static Vector2 GetFourWayDirection(int x){
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
}