using System;

namespace PK.Optional;

public class OptionalValueMissingException : Exception
{
	/// <inheritdoc />
	internal OptionalValueMissingException() : this("Optional value is missing. Check HasValue property before attempting to get value.")
	{
	}

	/// <inheritdoc />
	internal OptionalValueMissingException(string message) : base(message)
	{
	}
}
