using Godot;

namespace rosthouse.sharpest.addons
{
  public static class DecalExtensions
  {
    public static Color GetColorAtPosition(this Decal d, Vector3 position)
    {
      var aabb = d.GetAabb();
      if (aabb.HasPoint(position))
      {
        var uv = d.GetUv(position);
        Texture2D albedo = d.TextureAlbedo;
        return albedo.GetImage().GetPixelFromUV(uv);
      }
      return Colors.Magenta;
    }

    /// <summary>
    /// Calculates a UV coordinate on a decal, given a Position in world coordinates.
    /// /// </summary>
    /// <param name="d">The decal for which we want the UV position.</param>
    /// <param name="position">A coordinate in World Space</param>
    /// <returns>Either a Vector2 in the range [0,1], or Vector2.Inf, should the position not be inside the decal</returns>
    public static Vector2 GetUv(this Decal d, Vector3 position)
    {
      if (d.GetAabb().HasPoint(position))
      {
        var p = d.GetAabb().NormalizePoint(position);
        return new Vector2(p.X, p.Z);
      }
      else
      {
        return Vector2.Inf;
      }
    }

    public static void SetColorAtPosition(this Decal d, Vector3 position, Color color, int radius)
    {
      var aabb = d.GetAabb();
      if (aabb.HasPoint(position))
      {

        Texture2D albedo = d.TextureAlbedo;
        var img = albedo.GetImage();
        img.DrawCircleOnTexture(d.GetUv(position), color, radius);
        d.TextureAlbedo = ImageTexture.CreateFromImage(img);
      }
    }
  }
}
