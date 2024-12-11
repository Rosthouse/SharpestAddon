

using System;
using System.Collections;

namespace rosthouse.sharpest.addon.utils
{
	public static class MathUtils
	{
		public static bool Between<T>(T min, T max, T t) where T : IComparable
		{
			return t.CompareTo(min) >= 0 && t.CompareTo(max) < 0;
		}
	}
}
