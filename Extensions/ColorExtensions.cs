using Godot;

namespace rosthouse.sharpest.addon
{

  public static class ColorExtensions
  {

    /// <summary>
    /// Converts a color to a corresponding grayscale value. Note that only the RGB values are used for the conversion, the Alpha value is ignored.
    /// /// </summary>
    /// <param name="c">The color to convert</param>
    /// <returns>The converted color.</returns>
    public static Color ToGrayscale(this Color c)
    {
      var gray = (c.R + c.G + c.B) / 3;
      return new Color(gray, gray, gray, c.A);
    }
  }
}
