using Godot;
using System;

namespace rosthouse.sharpest.addons
{
  public partial class WindowManager : Node
  {

    private static WindowManager _instance;
    public static WindowManager Instance => _instance;


    public override void _EnterTree()
    {
      if (_instance != null)
      {
        this.QueueFree();
      }
      else
      {
        _instance = this;
      }

    }

    public void OpenWindow(Control n)
    {
      this.OpenWindow(n, GetViewport().GetVisibleRect().Size / 2);
    }

    public void OpenWindow(Control windowContent, Vector2 position)
    {
      var w = new Window();
      w.AddChild(windowContent);
      w.WrapControls = true;
      w.CloseRequested += () => w.QueueFree();
      windowContent.TreeExited += () => w.QueueFree();
      this.AddChild(w);
      w.Popup(new Rect2I(position.RountToInt(), w.Size));
    }

    public void OpenWindow(Control windowContent, Vector3 position)
    {
      var screenPos = GetWindow().GetCamera3D().UnprojectPosition(position);
      this.OpenWindow(windowContent, screenPos);
    }

    public void OpenWindowTruncated(Control windowContent, Vector3 position)
    {
      var screenPos = GetWindow().GetCamera3D().UnprojectPosition(position);
      if (GetWindow().GetVisibleRect().HasPoint(screenPos))
      {
        this.OpenWindow(windowContent, screenPos);
      }
      else
      {
        this.OpenWindow(windowContent, GetViewport().GetVisibleRect().Size / 2);
      }
    }

    public void OpenPopup(Popup windowContent, Vector3 position)
    {
      var screenPos = GetWindow().GetCamera3D().UnprojectPosition(position);
      this.OpenPopup(windowContent, screenPos);
    }

    public void OpenPopup(Popup popup, Vector2 position)
    {

      popup.FocusExited += () => popup.QueueFree();
      this.AddChild(popup);
      popup.Popup(new Rect2I(
       position.RountToInt(),
        popup.Size)
      );

    }
  }
}