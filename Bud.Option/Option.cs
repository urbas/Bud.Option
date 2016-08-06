using System;
using System.Collections.Generic;

namespace Bud {
  /// <summary>
  ///   An option can be in two states:
  ///   <para>
  ///     - it can contain a value, in which case the <see cref="HasValue" /> property
  ///     will return <c>true</c> and the <see cref="Value" /> property returns the
  ///     value contained in this option.
  ///   </para>
  ///   <para>
  ///     - it can be without a value, in which case the <see cref="HasValue" /> property
  ///     will return <c>false</c> and the <see cref="Value" /> property will behave
  ///     in an undefined manner.
  ///   </para>
  /// </summary>
  /// <typeparam name="T">the type of the value contained in this option.</typeparam>
  /// <remarks>
  ///   Options are particularly useful as a replacement for <c>null</c> values. Say, in functions
  ///   <see cref="Option.None{T}()" /> can be used instead of returning <c>null</c> to denote an invalid
  ///   or unkown outcome. Options can also be used for optional parameters.
  /// </remarks>
  public struct Option<T> {
    /// <summary>
    ///   If <see cref="HasValue" /> returns true, then this property returns the value contained by this option.
    ///   Otherwise the behaviour of this property is undefined.
    /// </summary>
    public T Value { get; }

    /// <summary>
    ///   Indicates whether this option contains a value or not.
    /// </summary>
    public bool HasValue { get; }

    /// <summary>
    ///   Initialises an option that will contain the given value.
    ///   Note that <paramref name="value" /> can be <c>null</c>.
    ///   In any case, the <see cref="HasValue" /> property of the resulting option return <c>true</c>.
    /// </summary>
    /// <param name="value">the value to be stored in this option.</param>
    public Option(T value) {
      Value = value;
      HasValue = true;
    }

    /// <param name="defaultValue">the value this function should return if this option has no value.</param>
    /// <returns>
    ///   if this option's <see cref="HasValue" /> is true, then this function will return <see cref="Value" />,
    ///   otherwise it returns the given default value.
    /// </returns>
    public T GetOrElse(T defaultValue) => HasValue ? Value : defaultValue;

    /// <param name="defaultValue">
    ///   the function that produces the value that will be returned by this function
    ///   if this option has no value.
    /// </param>
    /// <returns>
    ///   if this option's <see cref="HasValue" /> is true, then this function will return <see cref="Value" />,
    ///   otherwise this function will return the value produced by a call to <paramref name="defaultValue" />.
    /// </returns>
    /// <remarks>
    ///   The function will not be called if <see cref="HasValue" /> is true.
    /// </remarks>
    public T GetOrElse(Func<T> defaultValue) => HasValue ? Value : defaultValue();

    /// <summary>
    ///   This implicit conversion can convert any value of any type to an option. The resulting
    ///   option's <see cref="HasValue" /> will return true and the <see cref="Value" /> property
    ///   will be equal to the given parameter <paramref name="value" />.
    /// </summary>
    /// <param name="value">the value to be converted to an option.</param>
    public static implicit operator Option<T>(T value) => new Option<T>(value);

    /// <param name="defaultValue">the value this function should return if this option has no value.</param>
    /// <returns>
    ///   if this option's <see cref="HasValue" /> is true, then this function will return this option,
    ///   otherwise this function will return an option that contains <paramref name="defaultValue" />.
    /// </returns>
    /// <remarks>
    ///   The function will not be called if <see cref="HasValue" /> is true.
    /// </remarks>
    public Option<T> OrElse(T defaultValue) => HasValue ? this : defaultValue;

    /// <param name="defaultValue">the value this function should return if this option has no value.</param>
    /// <returns>
    ///   if this option's <see cref="HasValue" /> is true, then this function will return this option,
    ///   otherwise this function will return <paramref name="defaultValue" />.
    /// </returns>
    public Option<T> OrElse(Option<T> defaultValue) => HasValue ? this : defaultValue;

    /// <param name="defaultValue">the value of function will be returned if this option has no value.</param>
    /// <returns>
    ///   if this option's <see cref="HasValue" /> is true, then this function will return this option,
    ///   otherwise this function will return an option that contains the value produced by
    ///   <paramref name="defaultValue" />.
    /// </returns>
    /// <remarks>
    ///   The function <paramref name="defaultValue" /> will not be called if <see cref="HasValue" /> is true.
    /// </remarks>
    public Option<T> OrElse(Func<T> defaultValue) => HasValue ? this : defaultValue();

    /// <param name="defaultValue">the value of function will be returned if this option has no value.</param>
    /// <returns>
    ///   if this option's <see cref="HasValue" /> is true, then this function will return this option,
    ///   otherwise this function will return the option produced by <paramref name="defaultValue" />.
    /// </returns>
    /// <remarks>
    ///   The function <paramref name="defaultValue" /> will not be called if <see cref="HasValue" /> is true.
    /// </remarks>
    public Option<T> OrElse(Func<Option<T>> defaultValue) => HasValue ? this : defaultValue();

    /// <typeparam name="TResult">the type of the value in the resulting option.</typeparam>
    /// <param name="mapFunc">the function that takes the value of this option and produces a new value.</param>
    /// <returns>
    ///   if this option's <see cref="HasValue" /> is <c>true</c>, then the function <paramref name="mapFunc" />
    ///   will be called and it's result will be returned in an option. Othewrise, this method returns
    ///   <see cref="Option.None{T}()" />.
    /// </returns>
    public Option<TResult> Map<TResult>(Func<T, TResult> mapFunc)
      => HasValue ? mapFunc(Value) : Option.None<TResult>();

    /// <param name="other">the other option to compare to this one.</param>
    /// <returns>
    ///   <c>true</c> if and only if the options both have the equal <see cref="HasValue" />
    ///   and equal <see cref="Value" />.
    /// </returns>
    public bool Equals(Option<T> other)
      => HasValue == other.HasValue
         && EqualityComparer<T>.Default.Equals(Value, other.Value);

    /// <param name="obj">the other object to compare to this one.</param>
    /// <returns>
    ///   <c>true</c> if and only if <paramref name="obj" /> is not <c>null</c>,
    ///   it is of the same type as this option, and their contents equal.
    /// </returns>
    public override bool Equals(object obj)
      => !ReferenceEquals(null, obj)
         && obj is Option<T>
         && Equals((Option<T>) obj);

    public override int GetHashCode() {
      unchecked {
        return (HasValue.GetHashCode()*397) ^ EqualityComparer<T>.Default.GetHashCode(Value);
      }
    }

    public override string ToString()
      => HasValue
           ? $"Some<{GetType().GetGenericArguments()[0]}>({Value})"
           : $"None<{GetType().GetGenericArguments()[0]}>";
  }

  public static class Option {
    /// <typeparam name="T">the type of the resulting option.</typeparam>
    /// <returns>an option without a value.</returns>
    /// <remarks>
    ///   This is equivalent to calling <c>default(Option&lt;T&gt;)</c>.
    /// </remarks>
    public static Option<T> None<T>() => default(Option<T>);

    /// <typeparam name="T">the type of the resulting option.</typeparam>
    /// <param name="value">the value to wrap in an option.</param>
    /// <returns>an option that wraps the given <paramref name="value" />.</returns>
    public static Option<T> Some<T>(T value) => new Option<T>(value);

    /// <typeparam name="T">the type of the value contained in this option.</typeparam>
    /// <param name="nestedOption">an option that contains an option.</param>
    /// <returns>
    ///   the option contained in <paramref name="nestedOption" /> or
    ///   <c>none</c> if it does not contain anything.
    /// </returns>
    public static Option<T> Flatten<T>(this Option<Option<T>> nestedOption)
      => nestedOption.HasValue ? nestedOption.Value : None<T>();
  }
}