using Godot;
using System;


namespace rosthouse.sharpest.addons
{

	[Tool]
	public partial class SharpestAddon : EditorPlugin
	{
		
		public override void _EnterTree(){

			this.AddCustomType("Quit", "Node", GD.Load<Script>("res://addons/SharpestAddon/Nodes/Quit.cs"), GD.Load<Texture2D>("res://addons/SharpestAddon/Nodes/quit.svg"));
		}

		public override void _ExitTree(){
			this.RemoveCustomType("Quit");
		}
	}

}