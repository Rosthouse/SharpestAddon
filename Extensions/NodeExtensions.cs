

using System;
using Godot;

namespace rosthouse.sharpest.addons
{
    public static class NodeExtensions
    {

        public static Error Connect(this Node node, Type handler, Godot.Object target, FuncRef func, Godot.Collections.Array binds = null, uint flags = 0)
        {
            return node.Connect(nameof(handler), target, nameof(func), binds, flags);
        }

        public static Godot.Collections.Array<T> GetChildren<T>(this Node n) where T : Node
        {
            Godot.Collections.Array<T> typed = new Godot.Collections.Array<T>();
            foreach (Node c in n.GetChildren())
            {
                if (c is T)
                {
                    typed.Add(c as T);
                }
            }
            return typed;
        }
    }
}