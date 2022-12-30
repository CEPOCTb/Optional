using System;

namespace PK.Optional;

public class ObjectNotAnOptionalException : Exception
{
	/// <inheritdoc />
	public ObjectNotAnOptionalException() : this("Object is not an Optional.")
	{
	}

	/// <inheritdoc />
	public ObjectNotAnOptionalException(string message) : base(message)
	{
	}
}
