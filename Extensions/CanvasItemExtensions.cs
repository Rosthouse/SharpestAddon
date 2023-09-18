

using System.Globalization;
using Godot;

public static class CanvasItemExtensions
{

  public static Vector2 UnprojectPosition(this CanvasItem cvi, Vector3 position)
  {
    return cvi.GetViewport().GetCamera3D().UnprojectPosition(position);
  }
}
