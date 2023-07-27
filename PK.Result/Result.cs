using System;
using System.Collections.Generic;
using System.Linq;

namespace PK.Result;

/// <summary>
/// Generic operation result
/// </summary>
public abstract class Result
{
	/// <summary>
	/// Operation was completed successfully
	/// </summary>
	public abstract bool IsSuccess { get; }

	/// <summary>
	/// Operation was cancelled
	/// </summary>
	public abstract bool IsCancelled { get; }

	/// <summary>
	/// Errors information
	/// </summary>
	public abstract IReadOnlyCollection<IError> Errors { get; }

	/// <summary>
	/// Get value type contained by result, if any
	/// </summary>
	/// <returns>Value type or null</returns>
	protected internal virtual Type GetValueType() => null;
	
	/// <summary>
	/// Get value from result, if any.
	/// </summary>
	/// <returns>Contained value or exception</returns>
	protected internal virtual object GetValue() => throw new ResultException("InvalidOperation", "This result type can contain no value");
	
	/// <summary>
	/// Returns success result
	/// </summary>
	/// <returns>Success result</returns>
	public static Result Success() => SuccessResult.Instance;

	/// <summary>
	/// Returns success result with a given value
	/// </summary>
	/// <param name="result">Result value</param>
	/// <typeparam name="T">Result value type</typeparam>
	/// <returns>Success result with a given value</returns>
	public static Result<T> Success<T>(T result) => new SuccessResult<T>(result);

	/// <summary>
	/// Returns success result with a given value
	/// </summary>
	/// <param name="result">Result value</param>
	/// <param name="resultType">Result type</param>
	/// <returns>Success result with a given value</returns>
	public static Result Success(object result, Type resultType = null)
	{
		if (result == null && resultType == null)
		{
			throw new ArgumentNullException(nameof(resultType), "Can't determine type. resultType must be specified for null result value");
		}
		
		return Activator.CreateInstance(typeof(SuccessResult<>).MakeGenericType(resultType ?? result.GetType()), result) as Result;
	}

	/// <summary>
	/// Returns failed result
	/// </summary>
	/// <param name="error">Error, which describes failure reason</param>
	/// <returns>Failed result</returns>
	public static Result Failed(IError error) => new FailedResult(error);
	
	/// <summary>
	/// Returns failed result
	/// </summary>
	/// <param name="error">Error, which describes failure reason</param>
	/// <typeparam name="T">Expected result value type</typeparam>
	/// <returns>Failed result</returns>
	public static Result<T> Failed<T>(IError error) => new FailedResult<T>(error);

	/// <summary>
	/// Returns failed result
	/// </summary>
	/// <param name="errors">Errors, which describes failure reason</param>
	/// <returns>Failed result</returns>
	public static Result Failed(IEnumerable<IError> errors) => new FailedResult(errors);

	/// <summary>
	/// Returns failed result
	/// </summary>
	/// <param name="errors">Errors, which describes failure reason</param>
	/// <typeparam name="T">Expected result value type</typeparam>
	/// <returns>Failed result</returns>
	public static Result<T> Failed<T>(IEnumerable<IError> errors) => new FailedResult<T>(errors);

	/// <summary>
	/// Returns failed result
	/// </summary>
	/// <param name="errors">Errors, which describes failure reason</param>
	/// <returns>Failed result</returns>
	public static Result Failed(params IError[] errors) => new FailedResult(errors);

	/// <summary>
	/// Returns failed result
	/// </summary>
	/// <param name="errors">Errors, which describes failure reason</param>
	/// <typeparam name="T">Expected result value type</typeparam>
	/// <returns>Failed result</returns>
	public static Result<T> Failed<T>(params IError[] errors) => new FailedResult<T>(errors);

	/// <summary>
	/// Returns failed result from exception
	/// </summary>
	/// <param name="exception">Exception</param>
	/// <returns>Failed result</returns>
	public static Result Failed(Exception exception) => new FailedResult(Error.FromException(exception));

	/// <summary>
	/// Returns failed result from exception
	/// </summary>
	/// <param name="exception">Exception</param>
	/// <typeparam name="T">Expected result value type</typeparam>
	/// <returns>Failed result</returns>
	public static Result<T> Failed<T>(Exception exception) => new FailedResult<T>(Error.FromException(exception));

	/// <summary>
	/// Returns failed result
	/// </summary>
	/// <param name="error">Error, which describes failure reason</param>
	/// <param name="resultType">Result type</param>
	/// <returns>Failed result</returns>
	public static Result Failed(IError error, Type resultType)
	{
		if (resultType == null)
		{
			throw new ArgumentNullException(nameof(resultType), "Can't determine type. resultType must be specified");
		}

		return Activator.CreateInstance(typeof(FailedResult<>).MakeGenericType(resultType), error) as Result;
	}

	/// <summary>
	/// Returns failed result
	/// </summary>
	/// <param name="error">Error, which describes failure reason</param>
	/// <param name="resultType">Result type</param>
	/// <returns>Failed result</returns>
	public static Result Failed(IEnumerable<IError> errors, Type resultType)
	{
		if (resultType == null)
		{
			throw new ArgumentNullException(nameof(resultType), "Can't determine type. resultType must be specified");
		}

		return Activator.CreateInstance(typeof(FailedResult<>).MakeGenericType(resultType), errors) as Result;
	}
	
	/// <summary>
	/// Returns cancelled result
	/// </summary>
	/// <returns>Cancelled result</returns>
	public static Result Cancelled() => CancelledResult.Instance;
	
	/// <summary>
	/// Returns cancelled result
	/// </summary>
	/// <typeparam name="T">Expected result value type</typeparam>
	/// <returns>Cancelled result</returns>
	public static Result<T> Cancelled<T>() => CancelledResult<T>.Instance;

	/// <summary>
	/// Returns cancelled result
	/// </summary>
	/// <param name="resultType">Result type</param>
	/// <returns>Cancelled result</returns>
	public static Result Cancelled(Type resultType)
	{
		if (resultType == null)
		{
			throw new ArgumentNullException(nameof(resultType), "Can't determine type. resultType must be specified");
		}

		return Activator.CreateInstance(typeof(CancelledResult<>).MakeGenericType(resultType)) as Result;
	}

}

/// <summary>
/// Generic result for operation returning value
/// </summary>
/// <typeparam name="T">Value type</typeparam>
public abstract class Result<T> : Result
{
	/// <summary>
	/// Returned value
	/// </summary>
	/// <exception cref="InvalidOperationException"></exception>
	public abstract T Value { get; }

	#region Overrides of IResult

	/// <inheritdoc />
	protected internal override Type GetValueType() => typeof(T);

	/// <inheritdoc />
	protected internal override object GetValue() => Value;
	#endregion
}

internal sealed class SuccessResult : Result
{
	internal static readonly SuccessResult Instance = new();

	public override bool IsSuccess => true;
	public override bool IsCancelled => false;
	public override IReadOnlyCollection<IError> Errors => null;
}

internal sealed class SuccessResult<T> : Result<T>
{
	public override bool IsSuccess => true;
	public override bool IsCancelled => false;
	public override IReadOnlyCollection<IError> Errors => null;
	public override T Value { get; }

	public SuccessResult(T value) => Value = value;
}

internal sealed class FailedResult : Result
{
	public override bool IsSuccess => false;
	public override bool IsCancelled => false;
	public override IReadOnlyCollection<IError> Errors { get; }

	public FailedResult(IError error) => Errors = new []{ error };
	public FailedResult(IEnumerable<IError> errors) => Errors = errors.ToArray();
}

internal sealed class FailedResult<T> : Result<T>
{
	public override bool IsSuccess => false;
	public override bool IsCancelled => false;
	public override IReadOnlyCollection<IError> Errors { get; }

	public override T Value => Errors is { Count: > 0 }
		? Errors.Count == 1
			? throw Errors.First().GetException()
			: throw new AggregateException(Errors.Select(e => e.GetException()))
		: throw new ResultException("OperationFailed", "Failed result has no value");

	public FailedResult(IError error) => Errors = new[] { error };
	public FailedResult(IEnumerable<IError> errors) => Errors = errors.ToArray();

}

internal sealed class CancelledResult : Result
{
	internal static readonly IReadOnlyCollection<IError> CancelledError = new[] { new Error("OperationCancelled", "Operation was cancelled") };
	internal static readonly CancelledResult Instance = new();

	public override bool IsSuccess => false;
	public override bool IsCancelled => true;
	public override IReadOnlyCollection<IError> Errors => CancelledError;
}

internal sealed class CancelledResult<T> : Result<T>
{
	internal static readonly CancelledResult<T> Instance = new();

	public override bool IsSuccess => false;
	public override bool IsCancelled => true;
	public override IReadOnlyCollection<IError> Errors => CancelledResult.CancelledError;
	public override T Value => throw new OperationCanceledException(CancelledResult.CancelledError.First().Description);
}