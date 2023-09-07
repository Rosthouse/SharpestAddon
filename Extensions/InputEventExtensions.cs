

using System.Reflection.Metadata.Ecma335;
using Godot;

public static class InputEventExtensions
{
  public static bool IsAnyAction(this InputEvent @event, params string[] actions)
  {
    foreach (var action in actions)
    {
      if (@event.IsAction(action))
      {
        return true;
      }
    }
    return false;
  }
}
