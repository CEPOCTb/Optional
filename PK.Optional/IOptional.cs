namespace PK.Optional;

internal interface IOptional
{
	bool HasValue { get; }

	object GetValue();
}
