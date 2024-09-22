

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
	}
}
