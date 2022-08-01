using System;
using System.Collections.Generic;
using System.Linq;

namespace PK.Result;

/// <inheritdoc />
public class Error : IError
{
	private Exception _exception;

	/// <inheritdoc />
	public string Code { get; }

	/// <inheritdoc />
	public string Description { get; }

	/// <inheritdoc />
	public IDictionary<object, object> Data { get; }

	/// <inheritdoc />
	public Exception GetException() => new ResultException(Code, Description, _exception);

	public Error(string code, string description, IDictionary<object, object> data = null)
	{
		Code = code;
		Description = description;
		Data = data;
	}
	
	/// <summary>
	/// Create error from exception
	/// </summary>
	/// <param name="e">Exception</param>
	/// <param name="code">Optional code</param>
	/// <param name="description">Optional description</param>
	/// <returns></returns>
	public static IError FromException(Exception e, string code = null, string description = null) =>
		new Error(
			code ?? e.GetType().Name,
			description switch
			{
				null when e is AggregateException ae =>
					$"Aggregate exception '{ae.Message}': [{string.Join(",", ae.InnerExceptions.Select(inner => $"'{inner.Message}'"))}]",
				null => e.Message,
				_ => description
			}) { _exception = e };
}