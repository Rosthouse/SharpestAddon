

using Godot;

public static class ImageExtensions
{
  public static Color GetPixelFromUV(this Image image, Vector2 uv)
  {
    var pos = image.GetSize() * uv;
    return image.GetPixelv(new Vector2I(
      Mathf.RoundToInt(pos.X),
      Mathf.RoundToInt(pos.Y)
    ));
  }


  public static void SetPixelFromUv(this Image image, Vector2 uv, Color c)
  {
    var pos = image.GetSize() * uv;
    image.SetPixelv(
      new Vector2I(Mathf.RoundToInt(pos.X), Mathf.RoundToInt(pos.Y)), c);
  }

  public static void DrawCircleOnTexture(this Image image, Vector2 uv, Color c, int r)
  {
    var pos = (image.GetSize() * uv).RountToInt();
    // int x = r + pos.X, y = 0 + pos.Y;
    var size = image.GetSize();

    int r2 = r * r;
    int area = r2 << 2;
    int rr = r << 1;

    for (int i = 0; i < area; i++)
    {
      int tx = (i % rr) - r;
      int ty = (i / rr) - r;

      if (tx * tx + ty * ty > r2)
      {
        continue;
      }
      var posX = pos.X + tx;
      var posY = pos.Y + ty;
      if (posX >= 0 && posX < size.X && posY >= 0 && posY < size.Y)
      {
        image.SetPixel(pos.X + tx, pos.Y + ty, c);
      }
    }
    return;
  }
}