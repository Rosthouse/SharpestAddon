

using Godot;


namespace rosthouse.sharpest.addons
{
	public static class RectExtensions
	{
		public static Vector2 RandomPointInside(this Rect2 rect)
		{
			return new Vector2(
				(float)GD.RandRange(rect.Position.x, rect.End.x),
				(float)GD.RandRange(rect.Position.y, rect.End.y)
			);
		}
	}
}