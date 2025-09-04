using Godot;

namespace rosthouse.sharpest.addon;

public static class GridMapExtensions
{
  /// <summary>
  /// Maps any world coordinate to a global cell coordinate in a <see cref="GridMap"/> 
  /// </summary>
  /// <param name="gridMap">The GridMap to query</param>
  /// <param name="position">A <see cref="Vector3"/> in world coordinates.</param>
  /// <returns>The position of the cell.</returns>
  public static Vector3 ToGlobalCell(this GridMap gridMap, Vector3 position)
  {
    var cell = gridMap.LocalToMap(gridMap.ToLocal(position));
    var local = gridMap.MapToLocal(cell);

    return gridMap.ToGlobal(local);
  }

  /// <summary>
  /// Retrieves a cell index for a cell in a <see cref="GridMap"/> by its world coordinate.
  /// </summary>
  /// <param name="gridMap">The GridMap to query</param>
  /// <param name="position">A <see cref="Vector3"/> in world coordinates.</param>
  /// <returns>The index of the cell.</returns>
  public static Vector3I GlobalToMap(this GridMap gridMap, Vector3 position)
  {
    var local = gridMap.ToLocal(position);
    return gridMap.LocalToMap(local);
  }
}
