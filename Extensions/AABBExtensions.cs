using Godot;

namespace rosthouse.sharpest.addon
{
  public static class AABBExtensions
  {
    public static Vector3 NormalizePoint(this Aabb aabb, Vector3 position)
    {
      return (position - aabb.Position) / aabb.Size;
    }
  }
}
