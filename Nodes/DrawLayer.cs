using System;
using System.Collections.Generic;
using Godot;
using rosthouse.sharpest.addon;

namespace rosthouse.sharpest.addon
{


  [GlobalClass]
  public partial class DrawLayer : Control
  {
    private enum ItemType
    {
      Point, Line, Arrow,
      Arc,
      Disc
    }
    private struct Item
    {
      public Vector3[] points;
      public Color color;
      public ItemType type;
      public float width;
    }

    private readonly Vector2[] arrow = new Vector2[]{
    Vector2.Zero,
    new Vector2(1, 0),
    new Vector2(1,1),
    new Vector2(0,1),
    Vector2.Zero
  };

    private List<Item> items;

    public override void _Ready()
    {
      base._Ready();
      items = new List<Item>();
      RenderingServer.FramePostDraw += ClearItems;
    }

    public override void _ExitTree()
    {
      base._ExitTree();
      RenderingServer.FramePostDraw -= ClearItems;
    }

    private void ClearItems()
    {
      this.items.Clear();
    }

    public override void _Draw()
    {
      base._Draw();

      foreach (var item in items)
      {
        switch (item.type)
        {
          case ItemType.Point:
            this.DrawPoint(item);
            break;
          case ItemType.Line:
            this._DrawLine(item);
            break;
          case ItemType.Arrow:
            this.DrawArrow(item);
            break;
          case ItemType.Arc:
            this._DrawArc(item);
            break;
          case ItemType.Disc:
            this._DrawDisc(item);
            break;
        }
      }
    }

    private void _DrawArc(Item item)
    {
      var center = this.UnprojectPosition(item.points[0]);
      this.DrawArc(center, item.width, Mathf.Pi, Mathf.Pi * 2, 10, item.color);
    }

    private void _DrawDisc(Item item)
    {
      var center = this.UnprojectPosition(item.points[0]);
      var radius = (item.points[1] - item.points[0]).Length();

      var points = new Vector2[10];
      for (int i = 0; i < 10; i++)
      {
        // var p = item.points[0] +
      }

      this.DrawArc(center, radius, 0, Mathf.Pi * 2, 10, item.color, item.width, true);
      // this.DrawMultilineColors()
      // this.DrawCircle(center, radius, item.color);
    }

    private void DrawArrow(Item item)
    {
      throw new NotImplementedException();
    }

    private void _DrawLine(Item item)
    {
      var screenPosStart = this.UnprojectPosition(item.points[0]);
      var screenPosEnd = this.UnprojectPosition(item.points[1]);
      this.DrawLine(screenPosStart, screenPosEnd, item.color, item.width);
    }

    private void DrawPoint(Item item)
    {
      var screenPosStart = this.UnprojectPosition(item.points[0]);
      this.DrawCircle(screenPosStart, item.width, item.color);
    }

    public void DrawPoint(Vector3 position, Color c, float width = 1)
    {
      this.items.Add(new Item()
      {
        points = new Vector3[] { position },
        color = c,
        type = ItemType.Point,
        width = width
      });
      this.QueueRedraw();
    }

    public void Arrow(Vector3 position, Vector3 direction, Color color, float width = 1)
    {
      this.items.Add(new Item() { points = new Vector3[] { position, position + direction }, color = color, type = ItemType.Line, width = width });
      this.QueueRedraw();
    }

    public void Disc(Vector3 position, float radius, Color color, float width = 1)
    {
      this.items.Add(new Item { points = new Vector3[] { position, position + Vector3.Forward * radius }, color = color, type = ItemType.Disc, width = 50 });
      this.QueueRedraw();
    }

    public void Arc(Vector3 position, Vector3 normal, Color color)
    {
      this.items.Add(new Item() { points = new Vector3[] { position, normal }, color = color, type = ItemType.Arc, width = 50 });
      this.QueueRedraw();
    }
  }
}
