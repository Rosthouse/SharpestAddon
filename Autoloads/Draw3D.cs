using Godot;
using System;

namespace rosthouse.sharpest.addons
{
  /// <summary>
  /// This class contains some simple functions to draw primitives to the screen
  /// </summary>
  public partial class Draw3D : Node
  {
    public static Draw3D Instance => _instance;
    private static readonly Color defaultColor;
    private static Draw3D _instance;

    static Draw3D()
    {
      defaultColor = Colors.WhiteSmoke;
    }

    public override void _EnterTree()
    {
      base._EnterTree();
      if (_instance != null)
      {
        this.QueueFree();
        return;
      }
      _instance = this;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
      RenderingServer.FramePostDraw += this.Clear;
    }

    public MeshInstance3D Line(Vector3 start, Vector3 end, Color color = new Color())
    {
      var mi = new MeshInstance3D();
      var sm = new ImmediateMesh();
      var mat = new OrmMaterial3D();
      mat.VertexColorUseAsAlbedo = true;

      mi.Mesh = sm;
      mi.CastShadow = GeometryInstance3D.ShadowCastingSetting.Off;

      sm.SurfaceBegin(Mesh.PrimitiveType.Lines);
      sm.SurfaceSetColor(color);
      sm.SurfaceAddVertex(start);
      sm.SurfaceAddVertex(end);
      sm.SurfaceEnd();

      sm.SurfaceSetMaterial(0, mat);
      this.AddChild(mi);
      return mi;
    }

    public MeshInstance3D Point(Vector3 pos, float radius = 0.05f, Color color = new Color())
    {
      var mi = new MeshInstance3D();
      var sm = new SphereMesh();
      var mat = new OrmMaterial3D();

      mi.Mesh = sm;
      mi.CastShadow = GeometryInstance3D.ShadowCastingSetting.Off;
      mi.Position = pos;

      sm.Radius = radius;
      sm.Height = radius * 2;
      sm.Material = mat;

      mat.ShadingMode = BaseMaterial3D.ShadingModeEnum.Unshaded;
      mat.AlbedoColor = color;

      this.AddChild(mi);
      return mi;
    }

    public Label3D Text(Vector3 pos, String text, Color c = new Color())
    {
      Label3D l = new Label3D();
      l.Text = text;
      l.Billboard = BaseMaterial3D.BillboardModeEnum.Enabled;

      this.AddChild(l);
      return l;
    }

    public void Clear()
    {
      foreach (var c in this.GetChildren())
      {
        c.QueueFree();
      }
    }
  }
}