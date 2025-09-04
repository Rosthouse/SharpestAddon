using Godot;

#if TOOLS

namespace rosthouse.sharpest.addon;

[Tool]
public partial class SharpestAddon : EditorPlugin
{

  public override void _EnterTree()
  {
    AddCustomType(nameof(Quit), "Node", GD.Load<Script>("res://addons/SharpestAddon/Nodes/Quit.cs"), GD.Load<Texture2D>("res://addons/SharpestAddon/Nodes/quit.svg"));
    AddCustomType(nameof(ExtendedRemoteTransform3D), nameof(Node3D), GD.Load<Script>("res://addons/SharpestAddon/Nodes/ExtendedRemoteTransform3D.cs"), GD.Load<Texture2D>("res://addons/SharpestAddon/Nodes/ExtendedRemoteTransform3D.svg"));

    AddCustomType(nameof(Draw3D), nameof(Node), GD.Load<Script>("res://addons/SharpestAddon/Autoloads/Draw3D.cs"), GD.Load<Texture2D>("res://addons/SharpestAddon/Autoloads/draw3d.svg"));
    AddCustomType(nameof(WindowManager), nameof(Node), GD.Load<Script>("res://addons/SharpestAddon/Autoloads/WindowManager.cs"), GD.Load<Texture2D>("res://addons/SharpestAddon/Autoloads/windowmanager.svg"));

    AddAutoloadSingleton(nameof(Draw3D), "res://addons/SharpestAddon/Autoloads/Draw3D.cs");
    AddAutoloadSingleton(nameof(WindowManager), "res://addons/SharpestAddon/Autoloads/WindowManager.cs");
  }

  public override void _ExitTree()

  {
    RemoveAutoloadSingleton(nameof(Draw3D));
    RemoveAutoloadSingleton(nameof(WindowManager));
    RemoveCustomType(nameof(Draw3D));
    RemoveCustomType(nameof(WindowManager));

    RemoveCustomType(nameof(Quit));
    RemoveCustomType(nameof(ExtendedRemoteTransform3D));
  }
}

#endif
