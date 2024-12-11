

using System;
using Godot;

namespace rosthouse.sharpest.addon
{
  public static class NodeExtensions
  {

    public static Godot.Collections.Array<T> GetChildren<[MustBeVariant] T>(this Node n, bool includeInternal = false, bool recursive = false) where T : Node
    {
      var arr = new Godot.Collections.Array<T>();
      foreach (var c in n.GetChildren(includeInternal))
      {
        if (c is T t)
        {
          arr.Add(t);
        }

        if (recursive)
        {
          arr.AddRange(c.GetChildren<T>(includeInternal, recursive));
        }
      }
      return arr;
    }

    public static bool HasChild<[MustBeVariant] T>(this Node n, bool includeInternal = false) where T : Node
    {
      foreach (var c in n.GetChildren(includeInternal))
      {
        if (c is T)
        {
          return true;
        }
      }
      return false;
    }

    public static T GetChildByType<[MustBeVariant] T>(this Node n, bool includeInternal = false) where T : Node
    {
      foreach (var c in n.GetChildren(includeInternal))
      {
        if (c is T)
        {
          return (T)c;
        }
      }
      return null;
    }


    public static Godot.Collections.Array<Node> GetChildrenRecursive(this Node n, bool includeInternal = false)
    {
      var children = new Godot.Collections.Array<Node>();
      foreach (var c in n.GetChildren(includeInternal))
      {
        children.Add(c);
        children.AddRange(c.GetChildrenRecursive(includeInternal));
      }
      return children;
    }

    public static T GetChildRecursive<[MustBeVariant] T>(this Node n, bool includeInternal = false) where T : Node
    {
      var children = new Godot.Collections.Array<Node>();
      foreach (var c in n.GetChildren(includeInternal))
      {
        if (c is T)
        {
          return (T)c;
        }
        var t = c.GetChildRecursive<T>(includeInternal);
        if (t != null)
        {
          return t;
        }
      }
      return null;
    }

    public static Godot.Collections.Array<T> GetChildrenRecursive<[MustBeVariant] T>(this Node n, bool includeInternal = false) where T : Node
    {
      var children = new Godot.Collections.Array<T>();
      foreach (var c in n.GetChildren(includeInternal))
      {
        if (c is T)
        {
          children.Add((T)c);
        }
        var t = c.GetChildrenRecursive<T>(includeInternal);
        children.AddRange(t);
      }
      return children;
    }


    public static void FreeChildren(this Node n, bool includeInternal = false)
    {
      foreach (var c in n.GetChildren(includeInternal))
      {
        c.Free();
      }
    }

    public static void QueueFreeChildren(this Node n, bool includeInternal = false)
    {
      foreach (var c in n.GetChildren(includeInternal))
      {
        c.QueueFree();
      }
    }
  }
}
