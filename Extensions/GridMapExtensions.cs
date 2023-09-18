

using Godot;

public static class GridMapExtensions
{
  public static Vector3 ToGlobalCell(this GridMap gridMap, Vector3 position)
  {
    var cell = gridMap.LocalToMap(gridMap.ToLocal(position));
    var local = gridMap.MapToLocal(cell);
    return gridMap.ToGlobal(local);
  }

  public static Vector3I GlobalToMap(this GridMap gridMap, Vector3 position)
  {
    var local = gridMap.ToLocal(position);
    return gridMap.LocalToMap(local);
  }
}
