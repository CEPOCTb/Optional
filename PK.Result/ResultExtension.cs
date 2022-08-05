using System;

namespace PK.Result;

/// <summary>
/// Result helper clas
/// </summary>
public static class ResultExtension
{
	
	/// <summary>
	/// Returns result value from <see cref="Result{T}"/> instance or throws an exception
	/// </summary>
	/// <param name="result"><see cref="Result{T}"/> instance</param>
	/// <typeparam name="T">Value type</typeparam>
	/// <returns>Value</returns>
	/// <exception cref="ArgumentNullException">In case of <see cref="Result{T}"/> instance is null</exception>
	/// <exception cref="ResultException">In case of <see cref="Result{T}"/> is not success</exception>
	public static T Unwrap<T>(this Result<T> result) =>
		result switch
		{
			null => throw new ArgumentNullException(nameof(result)),
			{ IsSuccess: true } => result.Value,
			_ => throw new ResultException(result.Error?.Code ?? "Unknown", result.Error?.Description ?? "Unknown error")
		};

	/// <summary>
	/// Maps non success <see cref="PK.Result.Result"/> instance to a typed <see cref="Result{T}"/> instance
	/// </summary>
	/// <param name="result">Non success <see cref="PK.Result.Result"/> instance</param>
	/// <typeparam name="T">Returning result value type</typeparam>
	/// <returns>Typed copy of the non-success result</returns>
	/// <exception cref="InvalidOperationException">In case <see cref="PK.Result.Result"/> instance is a success instance</exception>
	public static Result<T> MapNonSuccess<T>(this Result result) =>
		result switch
		{
			{ IsCancelled: true } => Result.Cancelled<T>(),
			{ IsSuccess: false } => Result.Failed<T>(result.Error),
			_ => throw new InvalidOperationException("Can't map success result")
		};

	/// <summary>
	/// Helper function to check if <see cref="PK.Result.Result"/> is a failed result
	/// </summary>
	/// <param name="result"><see cref="PK.Result.Result"/> instance</param>
	/// <returns>True, if <see cref="PK.Result.Result"/> instance is a failed result</returns>
	public static bool IsFailed(this Result result) => !(result.IsSuccess || result.IsCancelled);

	/// <summary>
	/// Helper extension method to convert object to success result
	/// </summary>
	/// <param name="obj">Result value</param>
	/// <typeparam name="T">Result value type</typeparam>
	/// <returns>Value wrapped into <see cref="Result{T}"/> instance</returns>
	public static Result<T> ToSuccessResult<T>(this T obj) => Result.Success(obj);

	/// <summary>
	/// Helper extension method to convert exception to failed result
	/// </summary>
	/// <param name="e">Exception</param>
	/// <typeparam name="T">Result value type</typeparam>
	/// <returns>Failed <see cref="Result{T}"/> instance</returns>
	public static Result<T> AsFailedResult<T>(this Exception e, string code = null, string description = null) =>
		Result.Failed<T>(Error.FromException(e, code, description));

	/// <summary>
	/// Helper extension method to convert exception to failed result
	/// </summary>
	/// <param name="e">Exception</param>
	/// <returns>Failed <see cref="PK.Result.Result"/> instance</returns>
	public static Result AsFailedResult(this Exception e, string code = null, string description = null) =>
		Result.Failed(Error.FromException(e, code, description));

	/// <summary>
	/// Check if result can contain a value and return it's type
	/// </summary>
	/// <param name="result"><see cref="PK.Result.Result"/> instance</param>
	/// <param name="type">Value type</param>
	/// <returns>True if result can contain a value, False otherwise</returns>
	public static bool TryGetValueType(this Result result, out Type type)
	{
		type = result.GetValueType();
		return type != null;
	}

	/// <summary>
	/// Check if result contains a value and return it
	/// </summary>
	/// <param name="result"><see cref="PK.Result.Result"/> instance</param>
	/// <param name="value">Value</param>
	/// <returns>True if result contains a value, False otherwise</returns>
	public static bool TryGetValue(this Result result, out object value)
	{
		if (result.GetValueType() != null && result.IsSuccess)
		{
			value = result.GetValue();
			return true;
		}

		value = null;
		return false;
	}
}
