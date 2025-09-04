using Godot;
using System;

namespace rosthouse.sharpest.addon;
public partial class WindowManager : Node
{
  public static string SingletonName => "WindowManager";
  private static WindowManager _instance = null!;
  public static WindowManager Instance => _instance;


  public override void _EnterTree()
  {
    if (_instance != null)
    {
      QueueFree();
    }
    else
    {
      _instance = this;
    }
  }

  public void OpenWindow(Control n)
  {
    OpenWindow(n, GetViewport().GetVisibleRect().Size / 2);
  }

  public void OpenWindow(Control windowContent, Vector2 position, string title = "")
  {
    var lw = GD.Load<PackedScene>("res://addons/SharpestAddon/Nodes/light_window.tscn").Instantiate<LightWindow>();
    AddChild(lw);
    lw.SetContent(windowContent, true);
    lw.SetTitle(title);
    lw.Passthrough = true;
    lw.RespectContentMinSize = true;
    lw.Position = position;
  }

  public void OpenWindow(Control windowContent, Vector3 position)
  {
    var screenPos = GetWindow().GetCamera3D().UnprojectPosition(position);
    OpenWindow(windowContent, screenPos);
  }

  public void OpenWindowTruncated(Control windowContent, Vector3 position)
  {
    var screenPos = GetWindow().GetCamera3D().UnprojectPosition(position);
    if (GetWindow().GetVisibleRect().HasPoint(screenPos))
    {
      OpenWindow(windowContent, screenPos);
    }
    else
    {
      OpenWindow(windowContent, GetViewport().GetVisibleRect().Size / 2);
    }
  }

  public void OpenPopup(Popup windowContent, Vector3 position)
  {
    var screenPos = GetWindow().GetCamera3D().UnprojectPosition(position);
    OpenPopup(windowContent, screenPos);
  }

  public void OpenPopup(Popup popup, Vector2 position)
  {

    popup.FocusExited += () => popup.QueueFree();
    AddChild(popup);
    popup.Popup(new Rect2I(
     position.RountToInt(),
      popup.Size)
    );

  }
}