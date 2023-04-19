using System;
using System.Collections.Generic;

namespace PK.Optional;

public readonly record struct Optional<T> : IOptional, IComparable<Optional<T>>
{
	private readonly T _value;
	public bool HasValue { get; }
	public T Value => HasValue ? _value : throw new OptionalValueMissingException();

	public object GetValue() => Value;
	
	public Optional(T value)
	{
		_value = value;
		HasValue = true;
	}

	public Optional()
	{
		HasValue = false;
		_value = default;
	}

	public static implicit operator Optional<T>(T val) => new(val);
	public static implicit operator T(Optional<T> optional) => optional.Value;

	public static bool operator <(Optional<T> optional1, Optional<T> optional2) => optional1.CompareTo(optional2) < 0;
	public static bool operator >(Optional<T> optional1, Optional<T> optional2) => optional1.CompareTo(optional2) > 0;

	public static bool operator <=(Optional<T> optional1, Optional<T> optional2) => optional1.CompareTo(optional2) <= 0;
	public static bool operator >=(Optional<T> optional1, Optional<T> optional2) => optional1.CompareTo(optional2) >= 0;

	#region Relational members

	/// <inheritdoc />
	public int CompareTo(Optional<T> other) =>
		HasValue switch
		{
			true when !other.HasValue => 1,
			false when other.HasValue => -1,
			false when !other.HasValue => 0,
			_ => Comparer<T>.Default.Compare(Value, other.Value)
		};

	#endregion
}

public static class Optional
{
	public static bool IsOptional(this object o) => o is IOptional;
	public static bool IsOptional(this Type t) => typeof(IOptional).IsAssignableFrom(t);
	public static bool HasValue(object o) => o is IOptional option ? option.HasValue : throw new ObjectNotAnOptionalException();
	public static object GetValue(object o) => o is IOptional option ? option.GetValue() : throw new ObjectNotAnOptionalException();

	public static Optional<T> FromValue<T>(T value) => new(value);
	public static Optional<T> Empty<T>() => new();

}