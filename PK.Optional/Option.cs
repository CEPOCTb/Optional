using System;
using System.Collections.Generic;

namespace PK.Optional;

public readonly record struct Option<T> : IOption, IComparable<Option<T>>
{
	private readonly T _value;
	public bool HasValue { get; }
	public T Value => HasValue ? _value : throw new OptionValueMissingException();

	public object GetValue() => Value;
	
	public Option(T value)
	{
		_value = value;
		HasValue = true;
	}

	public Option()
	{
		HasValue = false;
		_value = default;
	}

	public static implicit operator Option<T>(T val) => new(val);
	public static implicit operator T(Option<T> optional) => optional.Value;

	public static bool operator <(Option<T> optional1, Option<T> optional2) => optional1.CompareTo(optional2) < 0;
	public static bool operator >(Option<T> optional1, Option<T> optional2) => optional1.CompareTo(optional2) > 0;

	public static bool operator <=(Option<T> optional1, Option<T> optional2) => optional1.CompareTo(optional2) <= 0;
	public static bool operator >=(Option<T> optional1, Option<T> optional2) => optional1.CompareTo(optional2) >= 0;

	#region Relational members

	/// <inheritdoc />
	public int CompareTo(Option<T> other) =>
		HasValue switch
		{
			true when !other.HasValue => 1,
			false when other.HasValue => -1,
			false when !other.HasValue => 0,
			_ => Comparer<T>.Default.Compare(Value, other.Value)
		};

	#endregion
}

public static class Option
{
	public static bool IsOption(this object o) => o is IOption;
	public static bool HasValue(object o) => o is IOption option ? option.HasValue : throw new ObjectNotAnOptionException();
	public static object GetValue(object o) => o is IOption option ? option.GetValue() : throw new ObjectNotAnOptionException();

	public static Option<T> Some<T>(T value) => new(value);
	public static Option<T> None<T>() => new();

}