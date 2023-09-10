


using Godot;

public static class ColorExtensions
{


  public static Color ToGrayscale(this Color c)
  {
    var gray = (c.R + c.G + c.B) / 3;
    return new Color(gray, gray, gray);
  }
}
