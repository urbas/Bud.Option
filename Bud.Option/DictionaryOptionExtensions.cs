using System.Collections.Generic;

namespace Bud {
  public static class DictionaryOptionExtensions {
    /// <typeparam name="TKey">the type keys in this dictionary.</typeparam>
    /// <typeparam name="TValue">
    ///   the type of values in this dictionary.
    ///   This is also the type of the returned value in the option.
    /// </typeparam>
    /// <param name="dict">this dictionary (from which to fetch the value).</param>
    /// <param name="key">the value of the key to fetch.</param>
    /// <returns>
    ///   returns a <c>none</c> option if this dictionary does not contain the key.
    ///   If the dictionary contains the key, then this method returns the value
    ///   contained in an option.
    /// </returns>
    public static Option<TValue> Get<TKey, TValue>(this IDictionary<TKey, TValue> dict,
                                                   TKey key) {
      TValue value;
      return dict.TryGetValue(key, out value) ? value : Option.None<TValue>();
    }
  }
}