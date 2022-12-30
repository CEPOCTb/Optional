using System;

namespace PK.Optional.FunctionalExtensions;

public static class Functional
{
	public static Optional<T> Some<T>(this T value) => Optional.FromValue(value);
	public static Optional<T> None<T>(this T value) => Optional.Empty<T>();
	public static Optional<T> None<T>() => Optional.Empty<T>();

	public static T ValueOrDefault<T>(this Optional<T> option) => option.HasValue ? option.Value : default;
	public static T ValueOr<T>(this Optional<T> option, T fallbackValue) => option.HasValue ? option.Value : fallbackValue;
	public static T ValueOr<T>(this Optional<T> option, Func<T> fallbackValueFunc) =>
		fallbackValueFunc == null
			? throw new ArgumentNullException(nameof(fallbackValueFunc))
			: option.HasValue ? option.Value : fallbackValueFunc();
	
	public static Optional<T> Or<T>(this Optional<T> option, T anotherValue) => option.HasValue ? option : anotherValue.Some();
	public static Optional<T> Or<T>(this Optional<T> option, Func<T> anotherValueFunc) =>
		anotherValueFunc == null
			? throw new ArgumentNullException(nameof(anotherValueFunc))
			: option.HasValue ? option : anotherValueFunc().Some();
	
	public static Optional<T> Else<T>(this Optional<T> option, Optional<T> anotherOption) => option.HasValue ? option : anotherOption;
	public static Optional<T> Else<T>(this Optional<T> option, Func<Optional<T>> anotherOptionFunc) =>
		anotherOptionFunc == null
			? throw new ArgumentNullException(nameof(anotherOptionFunc))
			: option.HasValue ? option : anotherOptionFunc();

	public static TResult Match<T, TResult>(this Optional<T> option, Func<T, TResult> someFunc, Func<TResult> noneFunc)
	{
		if (someFunc == null)
		{
			throw new ArgumentNullException(nameof(someFunc));
		}

		if (noneFunc == null)
		{
			throw new ArgumentNullException(nameof(noneFunc));
		}

		return option.HasValue ? someFunc(option.Value) : noneFunc();
	}

	public static void Match<T>(this Optional<T> option, Action<T> someAction, Action noneAction)
	{
		if (someAction == null)
		{
			throw new ArgumentNullException(nameof(someAction));
		}

		if (noneAction == null)
		{
			throw new ArgumentNullException(nameof(noneAction));
		}

		if (option.HasValue)
		{
			someAction(option.Value);
		}
		else
		{
			noneAction();
		}
	}

	public static void MatchSome<T>(this Optional<T> option, Action<T> someAction)
	{
		if (someAction == null)
		{
			throw new ArgumentNullException(nameof(someAction));
		}

		if (option.HasValue)
		{
			someAction(option.Value);
		}
	}

	public static void MatchNone<T>(this Optional<T> option, Action noneAction)
	{
		if (noneAction == null)
		{
			throw new ArgumentNullException(nameof(noneAction));
		}

		if (!option.HasValue)
		{
			noneAction();
		}
	}

	public static Optional<TResult> Map<T, TResult>(this Optional<T> option, Func<T, TResult> mapFunc) =>
		mapFunc == null
			? throw new ArgumentNullException(nameof(mapFunc))
			: option.Match(val => Some(mapFunc(val)), None<TResult>);

	public static Optional<TResult> FlatMap<T, TResult>(this Optional<T> option, Func<T, Optional<TResult>> mapFunc) =>
		mapFunc == null
			? throw new ArgumentNullException(nameof(mapFunc))
			: option.Match(mapFunc, None<TResult>);

	public static Optional<T> Filter<T>(this Optional<T> option, bool condition) =>
		!option.HasValue || condition ? option : None<T>();

	public static Optional<T> Filter<T>(this Optional<T> option, Func<T, bool> predicate) =>
		predicate == null
			? throw new ArgumentNullException(nameof(predicate))
			: !option.HasValue || predicate(option.Value) ? option : None<T>();

	public static Optional<T> NotNull<T>(this Optional<T> option) =>
		!option.HasValue || option.GetValue() != null ? option : None<T>();
}
