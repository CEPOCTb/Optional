namespace PK.Optional;

internal interface IOption
{
	bool HasValue { get; }

	object GetValue();
}
