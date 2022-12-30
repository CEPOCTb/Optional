using PK.Optional.FunctionalExtensions;
using PK.Result;

namespace PK.Optional.ResultExtensions;

public static class Extensions
{
	public static Optional<T> ToOptional<T>(this Result<T> result) =>
		result.IsSuccess ? result.Value.Some() : Optional.Empty<T>();

	public static Result<T> ToResult<T>(this Optional<T> option) =>
		option.Match(
			Result.Result.Success,
			() => Result.Result.Failed<T>(new Error("OptionHasNoValue", "Option value is missing"))
			);
}
