using System;

namespace PK.Optional;

public class ObjectNotAnOptionException : Exception
{
	/// <inheritdoc />
	public ObjectNotAnOptionException() : this("Object is not an option.")
	{
	}

	/// <inheritdoc />
	public ObjectNotAnOptionException(string message) : base(message)
	{
	}
}
