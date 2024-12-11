
using Godot;

namespace rosthouse.sharpest.addon;

public static class StringNameExtensions
{

  public static bool StartsWith(this StringName stringName, string start)
  {
    return stringName.ToString().StartsWith(start);
  }
}