using System;
using Moq;
using NUnit.Framework;
using static NUnit.Framework.Assert;
using static Bud.Option;

namespace Bud {
  public class OptionTest {
    [Test]
    public void Equals_returns_false_when_comparing_none_with_some()
      => IsFalse(Some(42).Equals(None<int>()));

    [Test]
    public void Equals_returns_true_when_value_equals()
      => IsTrue(Some(42).Equals(Some(42)));

    [Test]
    public void Equals_returns_false_when_values_differ()
      => IsFalse(Some(42).Equals(Some(9001)));

    [Test]
    public void Nones_of_different_types_are_not_equal()
      => AreNotEqual(None<int>(), None<string>());

    [Test]
    public void Nones_of_same_type_are_equal()
      => AreEqual(None<int>(), None<int>());

    [Test]
    public void Some_of_equal_values_are_equal()
      => AreEqual(Some(42), Some(42));

    [Test]
    public void Some_of_different_values_are_not_equal()
      => AreNotEqual(Some(42), Some(9001));

    [Test]
    public void Some_of_different_types_are_not_equal()
      => AreNotEqual(Some(42), Some<long>(42));

    [Test]
    public void Hash_code_equals_for_equal_values()
      => AreEqual(Some(42).GetHashCode(), Some(42).GetHashCode());

    [Test]
    public void Hash_code_does_not_equal_when_values_different()
      => AreNotEqual(Some(42).GetHashCode(), Some(9001).GetHashCode());

    [Test]
    public void ToString_returns_none_with_type_information()
      => AreEqual("None<System.Int32>", None<int>().ToString());

    [Test]
    public void ToString_returns_some_with_value_and_type_information()
      => AreEqual("Some<System.Int32>(42)", Some(42).ToString());

    [Test]
    public void GetOrElse_returns_the_contained_value()
      => AreEqual(42, Some(42).GetOrElse(9001));

    [Test]
    public void GetOrElse_returns_the_default_value()
      => AreEqual(9001, None<int>().GetOrElse(9001));

    [Test]
    public void Lazy_GetOrElse_returns_the_contained_value()
      => AreEqual(42, Some(42).GetOrElse(() => 9001));

    [Test]
    public void Lazy_GetOrElse_returns_the_default_value()
      => AreEqual(9001, None<int>().GetOrElse(() => 9001));

    [Test]
    public void Lazy_GetOrElse_does_not_invoke_the_callback()
      => DoesNotThrow(
        () => Some(42).GetOrElse(new Mock<Func<int>>(MockBehavior.Strict).Object));

    [Test]
    public void OrElse_returns_the_contained_value()
      => AreEqual(Some(42), Some(42).OrElse(9001));

    [Test]
    public void OrElse_returns_the_default_value()
      => AreEqual(Some(9001), None<int>().OrElse(9001));

    [Test]
    public void OrElse_with_option_fallback_returns_the_contained_value()
      => AreEqual(Some(42), Some(42).OrElse(Some(9001)));

    [Test]
    public void OrElse_with_option_fallback_returns_the_fallback()
      => AreEqual(Some(9001), None<int>().OrElse(Some(9001)));

    [Test]
    public void Lazy_OrElse_returns_the_contained_value()
      => AreEqual(Some(42), Some(42).OrElse(() => 9001));

    [Test]
    public void Lazy_OrElse_returns_the_default_value()
      => AreEqual(Some(9001), None<int>().OrElse(() => 9001));

    [Test]
    public void Lazy_OrElse_with_option_fallback_returns_the_contained_value()
      => AreEqual(Some(42), Some(42).OrElse(() => Some(9001)));

    [Test]
    public void Lazy_OrElse_with_option_fallback_returns_the_fallback()
      => AreEqual(Some(9001), None<int>().OrElse(() => Some(9001)));

    [Test]
    public void Lazy_OrElse_does_not_invoke_the_callback()
      => DoesNotThrow(
        () => Some(42).OrElse(new Mock<Func<int>>(MockBehavior.Strict).Object));

    [Test]
    public void Map_returns_None_for_None()
      => AreEqual(None<int>(), None<string>().Map(int.Parse));

    [Test]
    public void Map_returns_Some_with_new_value_for_Some()
      => AreEqual(Some(42), Some("42").Map(int.Parse));

    [Test]
    public void Flatten_returns_none_when_given_a_nested_none()
      => AreEqual(None<int>(),
                  Some(None<int>()).Flatten());

    [Test]
    public void Flatten_returns_none_when_given_none()
      => AreEqual(None<int>(),
                  None<Option<int>>().Flatten());

    [Test]
    public void Flatten_returns_none_when_given_a_nested_some_option()
      => AreEqual(Some(42),
                  Some(Some(42)).Flatten());

    [Test]
    public void None_equals_to_default() => AreEqual(default(Option<int>), None<int>());
  }
}