using Godot;
using System;

#if TOOLS

namespace rosthouse.sharpest.addons
{

  [Tool]
  public partial class SharpestAddon : EditorPlugin
  {

    public override void _EnterTree()
    {
      this.AddCustomType("Quit", "Node", GD.Load<Script>("res://addons/SharpestAddon/Nodes/Quit.cs"), GD.Load<Texture2D>("res://addons/SharpestAddon/Nodes/quit.svg"));
      this.AddCustomType("Draw3D", "Node", GD.Load<Script>("res://addons/SharpestAddon/Autoloads/Draw3D.cs"), GD.Load<Texture2D>("res://addons/SharpestAddon/Nodes/draw3d.svg"));
      this.AddCustomType("WindowManager", "Node", GD.Load<Script>("res://addons/SharpestAddon/Autoloads/WindowManager.cs"), GD.Load<Texture2D>("res://addons/SharpestAddon/Autoloads/windowmanager.svg"));
      this.AddAutoloadSingleton("Draw3D", "res://addons/SharpestAddon/Nodes/Draw3D.cs");
      this.AddAutoloadSingleton("WindowManager", "res://addons/SharpestAddon/Autoloads/WindowManager.cs");
    }

    public override void _ExitTree()
    
    {
      this.RemoveAutoloadSingleton("Draw3D");
      this.RemoveAutoloadSingleton("WindowManager");
      this.RemoveCustomType("Quit");
      this.RemoveCustomType("Draw3D");
      this.RemoveCustomType("WindowManager");
    }
  }

}

#endif