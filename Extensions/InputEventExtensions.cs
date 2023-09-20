

using Godot;

namespace rosthouse.sharpest.addon
{


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

    public static bool IsAnyAction(params string[] actions)
    {
      foreach (var action in actions)
      {
        if (Input.IsActionPressed(action) || Input.IsActionJustPressed(action) || Input.IsActionJustReleased(action))
        {
          return true;
        }
      }
      return false;
    }
  }

}
