

using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;

public static class GodotCollectionsExtensions
{
  public static Array<T> Permutation<[MustBeVariant] T>(this Array<T> a, bool deep = false)
  {
    var t = a.Duplicate(deep);
    t.Shuffle();
    return t;
  }

  public static Array<T> ToGodotArray<[MustBeVariant] T>(this IEnumerable<T> t)
  {
    return new Array<T>(t.ToArray());
  }
}
