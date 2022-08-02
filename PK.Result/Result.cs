using System;

namespace PK.Result;

internal class SuccessResult : IResult
{
	internal static readonly SuccessResult Instance = new();
	
	public bool Success => true;
	public bool Cancelled => false;
	public IError Error => null;
}

internal class SuccessResult<T> : IResult<T>
{
	public bool Success => true;
	public bool Cancelled => false;
	public IError Error => null;
	public T Value { get; }

	public SuccessResult(T value) => Value = value;
}

internal class FailedResult : IResult
{
	public bool Success => false;
	public bool Cancelled => false;
	public IError Error { get; }

	public FailedResult(IError error) => Error = error;
}

internal class FailedResult<T> : IResult<T>
{
	public bool Success => false;
	public bool Cancelled => false;
	public IError Error { get; }
	public T Value => Error != null
		? throw Error.GetException()
		: throw new ResultException("OperationFailed", "Failed result has no value");

	public FailedResult(IError error)
	{
		Error = error;
	}
}

internal class CancelledResult : IResult
{
	internal static readonly Error CancelledError = new Error("OperationCancelled", "Operation was cancelled");
	internal static readonly CancelledResult Instance = new();
	
	public bool Success => false;
	public bool Cancelled => true;
	public IError Error => CancelledError;
}

internal class CancelledResult<T> : IResult<T>
{
	internal static readonly CancelledResult<T> Instance = new();

	public bool Success => false;
	public bool Cancelled => true;
	public IError Error => CancelledResult.CancelledError;
	public T Value => throw new OperationCanceledException(CancelledResult.CancelledError.Description);
}

/// <summary>
/// Result helper clas
/// </summary>
public static class Result
{
	/// <summary>
	/// Returns success result
	/// </summary>
	/// <returns>Success result</returns>
	public static IResult Success() => SuccessResult.Instance;

	/// <summary>
	/// Returns success result with a given value
	/// </summary>
	/// <param name="result">Result value</param>
	/// <typeparam name="T">Result value type</typeparam>
	/// <returns>Success result with a given value</returns>
	public static IResult<T> Success<T>(T result) => new SuccessResult<T>(result);

	/// <summary>
	/// Returns failed result
	/// </summary>
	/// <param name="error">Error, which describes failure reason</param>
	/// <returns>Failed result</returns>
	public static IResult Failed(IError error) => new FailedResult(error);
	
	/// <summary>
	/// Returns failed result
	/// </summary>
	/// <param name="error">Error, which describes failure reason</param>
	/// <typeparam name="T">Expected result value type</typeparam>
	/// <returns>Failed result</returns>
	public static IResult<T> Failed<T>(IError error) => new FailedResult<T>(error);

	/// <summary>
	/// Returns cancelled result
	/// </summary>
	/// <returns>Cancelled result</returns>
	public static IResult Cancelled() => CancelledResult.Instance;
	
	/// <summary>
	/// Returns cancelled result
	/// </summary>
	/// <typeparam name="T">Expected result value type</typeparam>
	/// <returns>Cancelled result</returns>
	public static IResult<T> Cancelled<T>() => CancelledResult<T>.Instance;

	/// <summary>
	/// Returns result value from <see cref="IResult{T}"/> instance or throws an exception
	/// </summary>
	/// <param name="result"><see cref="IResult{T}"/> instance</param>
	/// <typeparam name="T">Value type</typeparam>
	/// <returns>Value</returns>
	/// <exception cref="ArgumentNullException">In case of <see cref="IResult{T}"/> instance is null</exception>
	/// <exception cref="ResultException">In case of <see cref="IResult{T}"/> is not success</exception>
	public static T Unwrap<T>(this IResult<T> result) =>
		result switch
		{
			null => throw new ArgumentNullException(nameof(result)),
			{ Success: true } => result.Value,
			_ => throw new ResultException(result.Error?.Code ?? "Unknown", result.Error?.Description ?? "Unknown error")
		};

	/// <summary>
	/// Maps non success <see cref="IResult"/> instance to a typed <see cref="IResult{T}"/> instance
	/// </summary>
	/// <param name="result">Non success <see cref="IResult"/> instance</param>
	/// <typeparam name="T">Returning result value type</typeparam>
	/// <returns>Typed copy of the non-success result</returns>
	/// <exception cref="InvalidOperationException">In case <see cref="IResult"/> instance is a success instance</exception>
	public static IResult<T> MapNonSuccess<T>(this IResult result) =>
		result switch
		{
			{ Cancelled: true } => Cancelled<T>(),
			{ Success: false } => Failed<T>(result.Error),
			_ => throw new InvalidOperationException("Can't map success result")
		};

	/// <summary>
	/// Helper function to check if <see cref="IResult"/> is a failed result
	/// </summary>
	/// <param name="result"><see cref="IResult"/> instance</param>
	/// <returns>True, if <see cref="IResult"/> instance is a failed result</returns>
	public static bool IsFailed(this IResult result) => !(result.Success || result.Cancelled);

	/// <summary>
	/// Helper extension method to convert object to success result
	/// </summary>
	/// <param name="obj">Result value</param>
	/// <typeparam name="T">Result value type</typeparam>
	/// <returns>Value wrapped into <see cref="IResult{T}"/> instance</returns>
	public static IResult<T> ToSuccessResult<T>(this T obj) => Success(obj);

	/// <summary>
	/// Helper extension method to convert exception to failed result
	/// </summary>
	/// <param name="e">Exception</param>
	/// <typeparam name="T">Result value type</typeparam>
	/// <returns>Failed <see cref="IResult{T}"/> instance</returns>
	public static IResult<T> AsFailedResult<T>(this Exception e, string code = null, string description = null) =>
		Failed<T>(Error.FromException(e, code, description));

	/// <summary>
	/// Helper extension method to convert exception to failed result
	/// </summary>
	/// <param name="e">Exception</param>
	/// <returns>Failed <see cref="IResult"/> instance</returns>
	public static IResult AsFailedResult(this Exception e, string code = null, string description = null) =>
		Failed(Error.FromException(e, code, description));

}