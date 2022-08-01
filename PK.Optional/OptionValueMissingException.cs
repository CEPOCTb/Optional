using System;

namespace PK.Optional;

public class OptionValueMissingException : Exception
{
	/// <inheritdoc />
	public OptionValueMissingException() : this("Option value is missing. Check HasValue property before attempting to get value.")
	{
	}

	/// <inheritdoc />
	public OptionValueMissingException(string message) : base(message)
	{
	}
}
