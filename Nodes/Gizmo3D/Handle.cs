

using Godot;

namespace rosthouse.sharpest.addon
{
  public partial class Handle : Area3D
  {
    [Export] private Gizmo3D.ActionType mode;

    public Gizmo3D.ActionType Mode => mode;
  }
}
