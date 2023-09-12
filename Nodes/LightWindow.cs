using Godot;
using System;

public partial class LightWindow : Control
{
  private bool resize;
  private bool drag;
  private Vector2 offset;
  private Control contentContainer;

  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
    GetNode<Button>("%CloseButton").Pressed += () => this.QueueFree();
    GetNode<Button>("%ResizeButton").ButtonDown += () => this.resize = true;
    GetNode<Button>("%ResizeButton").ButtonUp += () => this.resize = false;
    GetNode<Label>("%WindowTitle").GuiInput += this.OnTitleInput;
    this.contentContainer = GetNode<Control>("%Content");
  }

  private void OnTitleInput(InputEvent @event)
  {
    if (@event is InputEventMouseButton iemb)
    {
      if (iemb.ButtonIndex == MouseButton.Left && iemb.Pressed)
      {
        this.drag = true;
        this.offset = iemb.Position - this.Position;
      }
      else if (iemb.ButtonIndex == MouseButton.Left && !iemb.Pressed)
      {
        this.drag = false;
      }
    }
    if (@event is InputEventMouseMotion iemm && this.drag)
    {
      this.Position = iemm.Position - this.offset;
    }
  }

  public override void _Input(InputEvent @event)
  {
    if (@event is InputEventMouseMotion iemm && this.resize)
    {
      this.Size = iemm.Position - this.Position;
      this.contentContainer.GetChild<Control>(0).Size = this.Size;
    }
  }
  public void SetTitle(string title)
  {
    GetNode<Label>("%WindowTitle").Text = title;
  }

  public void SetContent(Control c, bool wrapContent = false)
  {
    this.contentContainer.AddChild(c);
    if (wrapContent)
    {
      this.Size = c.Size;
    }
    c.TreeExiting += () => this.QueueFree();
  }

}
