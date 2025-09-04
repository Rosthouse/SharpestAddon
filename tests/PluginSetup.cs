using GdUnit4;
using Godot;
using static GdUnit4.Assertions;

namespace rosthouse.sharpest.addon.test;

[TestSuite]
public class PluginSetup
{
  [TestCase]
  public void AllAutoloadsLoaded()
  {
    AssertObject(Draw3D.Instance).IsNotNull();
    AssertObject(WindowManager.Instance).IsNotNull();
  }
}