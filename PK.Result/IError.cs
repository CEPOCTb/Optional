using System;
using System.Collections.Generic;

namespace PK.Result;

/// <summary>
/// Error information
/// </summary>
public interface IError
{
	/// <summary>
	/// Error code
	/// </summary>
	string Code { get; }

	/// <summary>
	/// Error description
	/// </summary>
	string Description { get; }
	
	/// <summary>
	/// Additional data
	/// </summary>
	IDictionary<object, object> Data { get; }

	/// <summary>
	/// Get error formatted as exception
	/// </summary>
	/// <returns>Error in form of exception</returns>
	Exception GetException();
}