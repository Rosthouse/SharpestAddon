

using Godot;


namespace rosthouse.sharpest.addon
{
	public static class RectExtensions
	{
		public static Vector2 RandomPointInside(this Rect2 rect)
		{
			return new Vector2(
				(float)GD.RandRange(rect.Position.X, rect.End.X),
				(float)GD.RandRange(rect.Position.Y, rect.End.Y)
			);
		}

		public static System.Numerics.Vector4 ToSystemVector4(this Rect2 rect)
		{
			return new System.Numerics.Vector4
			{
				X = rect.Position.X,
				Y = rect.Position.Y,
				Z = rect.End.X,
				W = rect.End.Y
			};
		}


		public static Rect2 ToRect2(this System.Numerics.Vector4 vector)
		{
			return new Rect2
			{
				Position = new Vector2
				{
					X = vector.X,
					Y = vector.Y
				},
				End = new Vector2
				{
					X = vector.Z,
					Y = vector.W
				}
			};
		}
	}
}
