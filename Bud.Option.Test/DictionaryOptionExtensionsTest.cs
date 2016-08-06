using System.Collections.Generic;
using NUnit.Framework;

namespace Bud {
  public class DictionaryOptionExtensionsTest {
    [Test]
    public void Get_on_empty_dictionary_returns_None()
      => Assert.AreEqual(Option.None<int>(), new Dictionary<int, int>().Get(42));

    [Test]
    public void Get_on_existing_key_in_dictionary_returns_Some()
      => Assert.AreEqual(Option.Some(1), new Dictionary<int, int> {{42, 1}}.Get(42));
  }
}