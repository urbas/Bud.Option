using NUnit.Framework;

namespace Bud {
  public class EnumerableOptionExtensionsTest {
    [Test]
    public void Gather_returns_values()
      => Assert.AreEqual(new[] {1, 2},
                  new[] {Option.None<int>(), Option.Some(1), Option.None<int>(), Option.Some(2)}
                    .Gather());

    [Test]
    public void Gather_filters_and_selects()
      => Assert.AreEqual(new[] {"1", "3"},
                  new[] {1, 2, 3, 4}.Gather(number => number%2 == 1 ? Option.Some(number.ToString()) : Option.None<string>()));
  }
}