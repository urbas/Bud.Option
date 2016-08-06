using System;
using System.Collections.Generic;
using System.Linq;

namespace Bud {
  public static class EnumerableOptionExtensions {
    /// <typeparam name="T">the type of options contained in the enumerable.</typeparam>
    /// <param name="enumerable">an enumerable of options.</param>
    /// <returns>
    ///   returns the values contained in the options in the given
    ///   <paramref name="enumerable" />.
    /// </returns>
    public static IEnumerable<T> Gather<T>(this IEnumerable<Option<T>> enumerable)
      => enumerable.Where(optional => optional.HasValue)
                   .Select(optional => optional.Value);

    /// <summary>
    ///   This method chains <see cref="Enumerable.Select{TSource,TResult}(IEnumerable{TSource},Func{TSource,TResult})" />
    ///   and <see cref="Gather{T}(IEnumerable{Option{T}})" />.
    /// </summary>
    /// <typeparam name="TSource">the type of values in the given <paramref name="enumerable" />.</typeparam>
    /// <typeparam name="TResult">the type of values in the returned enumerable.</typeparam>
    /// <param name="enumerable">the enumerable on which to run the given <paramref name="selector" />.</param>
    /// <param name="selector">
    ///   this function is called on each element of the given <paramref name="enumerable" />.
    ///   It should return an option.
    /// </param>
    /// <returns>
    ///   An enumerable of elements returned by the <paramref name="selector" /> function. <c>none</c>
    ///   options are ignored.
    /// </returns>
    public static IEnumerable<TResult> Gather<TSource, TResult>(this IEnumerable<TSource> enumerable,
                                                                Func<TSource, Option<TResult>> selector)
      => enumerable.Select(selector).Gather();
  }
}