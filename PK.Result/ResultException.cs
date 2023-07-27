using System;
using System.Runtime.Serialization;

namespace PK.Result;

/// <summary>
/// Result exception
/// </summary>
public class ResultException : Exception
{
	/// <summary>
	/// Error code
	/// </summary>
	public string Code { get; }

	/// <inheritdoc />
	public ResultException(string code)
	{
		Code = code;
	}

	/// <inheritdoc />
	public ResultException(string code, string message) : base(message)
	{
		Code = code;
	}

	/// <inheritdoc />
	public ResultException(string code, string message, Exception innerException) : base(message, innerException)
	{
		Code = code;
	}

	/// <inheritdoc />
	protected ResultException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
		Code = info.GetValue(nameof(Code), typeof(string)) as string;
	}
}