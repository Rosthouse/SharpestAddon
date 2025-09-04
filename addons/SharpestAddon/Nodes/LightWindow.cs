using Godot;

namespace rosthouse.sharpest.addon;


public partial class LightWindow : Control
{
  private bool resize;
  private bool drag;
  private Vector2 offset;
  private Control contentContainer = null!;
  [Export] public bool Passthrough { get; set; } = false;
  [Export] public bool RespectContentMinSize { get; set; } = false;
  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
    GetNode<Button>("%CloseButton").Pressed += () => QueueFree();
    GetNode<Button>("%ResizeButton").ButtonDown += () => resize = true;
    GetNode<Button>("%ResizeButton").ButtonUp += () => resize = false;
    GetNode<Label>("%WindowTitle").GuiInput += OnTitleInput;
    contentContainer = GetNode<Control>("%Content");
    MouseEntered += () => GD.Print("Mouse entered");
    MouseExited += () => GD.Print("Mouse exited");
  }

  public override void _Input(InputEvent @event)
  {
    if (@event is InputEventMouseMotion iemm && resize)
    {
      Size = iemm.Position - Position;

      if (RespectContentMinSize)
      {
        var child = contentContainer.GetChild<Control>(0);
        var minSize = contentContainer.GetChild<Control>(0).CustomMinimumSize;
        Size = new Vector2(
          Mathf.Max(Size.X, minSize.X),
          Mathf.Max(Size.Y, minSize.Y)
        );
      }
      contentContainer.GetChild<Control>(0).Size = Size;
      GetViewport().SetInputAsHandled();
    }

    if (!Passthrough)
    {
      GetViewport().SetInputAsHandled();
    }
  }

  private void OnTitleInput(InputEvent @event)
  {
    if (@event is InputEventMouseButton iemb)
    {
      if (iemb.ButtonIndex == MouseButton.Left && iemb.Pressed)
      {
        drag = true;
        offset = iemb.Position - Position;
      }
      else if (iemb.ButtonIndex == MouseButton.Left && !iemb.Pressed)
      {
        drag = false;
      }
    }
    if (@event is InputEventMouseMotion iemm && drag)
    {
      Position += iemm.Relative;
    }
  }
  public void SetTitle(string title)
  {
    GetNode<Label>("%WindowTitle").Text = title;
  }
  public void SetContent(Control c, bool wrapContent = false)
  {
    contentContainer.AddChild(c);
    if (wrapContent)
    {
      Size = c.Size;
    }
    c.TreeExiting += () => QueueFree();
  }

}
