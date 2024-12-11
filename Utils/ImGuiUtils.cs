


using System;
using System.Collections;
using ImGuiNET;

namespace rosthouse.sharpest.addon.utils
{
  public static class ImGuiUtils
  {

    public static bool InputInt64(string label, ref long value)
    {
      unsafe
      {
        fixed (long* valPtr = &value)
        {
          return ImGui.InputScalar(label, ImGuiDataType.S64, (IntPtr)valPtr);
        }
      }
    }
  }
}
