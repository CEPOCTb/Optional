using System;

namespace PK.Optional.FunctionalExtensions;

public static class Functional
{
	public static Option<T> Some<T>(this T value) => Option.Some(value);
	public static Option<T> None<T>(this T value) => Option.None<T>();
	public static Option<T> None<T>() => Option.None<T>();

	public static T ValueOrDefault<T>(this Option<T> option) => option.HasValue ? option.Value : default;
	public static T ValueOr<T>(this Option<T> option, T fallbackValue) => option.HasValue ? option.Value : fallbackValue;
	public static T ValueOr<T>(this Option<T> option, Func<T> fallbackValueFunc) =>
		fallbackValueFunc == null
			? throw new ArgumentNullException(nameof(fallbackValueFunc))
			: option.HasValue ? option.Value : fallbackValueFunc();
	
	public static Option<T> Or<T>(this Option<T> option, T anotherValue) => option.HasValue ? option : anotherValue.Some();
	public static Option<T> Or<T>(this Option<T> option, Func<T> anotherValueFunc) =>
		anotherValueFunc == null
			? throw new ArgumentNullException(nameof(anotherValueFunc))
			: option.HasValue ? option : anotherValueFunc().Some();
	
	public static Option<T> Else<T>(this Option<T> option, Option<T> anotherOption) => option.HasValue ? option : anotherOption;
	public static Option<T> Else<T>(this Option<T> option, Func<Option<T>> anotherOptionFunc) =>
		anotherOptionFunc == null
			? throw new ArgumentNullException(nameof(anotherOptionFunc))
			: option.HasValue ? option : anotherOptionFunc();

	public static TResult Match<T, TResult>(this Option<T> option, Func<T, TResult> someFunc, Func<TResult> noneFunc)
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

	public static void Match<T>(this Option<T> option, Action<T> someAction, Action noneAction)
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

	public static void MatchSome<T>(this Option<T> option, Action<T> someAction)
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

	public static void MatchNone<T>(this Option<T> option, Action noneAction)
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

	public static Option<TResult> Map<T, TResult>(this Option<T> option, Func<T, TResult> mapFunc) =>
		mapFunc == null
			? throw new ArgumentNullException(nameof(mapFunc))
			: option.Match(val => Some(mapFunc(val)), None<TResult>);

	public static Option<TResult> FlatMap<T, TResult>(this Option<T> option, Func<T, Option<TResult>> mapFunc) =>
		mapFunc == null
			? throw new ArgumentNullException(nameof(mapFunc))
			: option.Match(mapFunc, None<TResult>);

	public static Option<T> Filter<T>(this Option<T> option, bool condition) =>
		!option.HasValue || condition ? option : None<T>();

	public static Option<T> Filter<T>(this Option<T> option, Func<T, bool> predicate) =>
		predicate == null
			? throw new ArgumentNullException(nameof(predicate))
			: !option.HasValue || predicate(option.Value) ? option : None<T>();

	public static Option<T> NotNull<T>(this Option<T> option) =>
		!option.HasValue || option.GetValue() != null ? option : None<T>();
}
