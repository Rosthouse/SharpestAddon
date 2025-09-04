using GdUnit4;
using rosthouse.sharpest.addon.utils;
using static GdUnit4.Assertions;

namespace rosthouse.sharpestaddon.test.utils;

[TestSuite]
public class MathUtilsTest
{

  [TestCase(1, 3, 2, true)]
  [TestCase(1, 2, 1, true)]
  [TestCase(1, 2, 2, false)]
  [TestCase(1, 2, 0, false)]
  [TestCase(1, 2, 3, false)]
  public void BetweenInt(int a, int b, int c, bool expect)
  {
    AssertThat(MathUtils.Between(a, b, c)).IsEqual(expect);
  }

  [TestCase(1.0f, 3.0f, 2.0f, true)]
  [TestCase(1.0f, 2.0f, 1.0f, true)]
  [TestCase(1.0f, 2.0f, 2.0f, false)]
  [TestCase(1.0f, 2.0f, 0.0f, false)]
  [TestCase(1.0f, 2.0f, 3.0f, false)]
  public void BetweenFloat(float a, float b, float c, bool expect)
  {
    AssertThat(MathUtils.Between(a, b, c)).IsEqual(expect);
  }
}