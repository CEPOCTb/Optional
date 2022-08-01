using System;

namespace PK.Result;

/// <summary>
/// Generic operation result
/// </summary>
public interface IResult
{
	/// <summary>
	/// Operation was completed successfully
	/// </summary>
	bool Success { get; }

	/// <summary>
	/// Operation was cancelled
	/// </summary>
	bool Cancelled { get; }

	/// <summary>
	/// Error information
	/// </summary>
	IError Error { get; }
}

/// <summary>
/// Generic result for operation returning value
/// </summary>
/// <typeparam name="T">Value type</typeparam>
public interface IResult<out T> : IResult
{
	/// <summary>
	/// Returned value
	/// </summary>
	/// <exception cref="InvalidOperationException"></exception>
	T Value { get; }
}