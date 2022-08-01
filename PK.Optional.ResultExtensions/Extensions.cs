using PK.Optional.FunctionalExtensions;
using PK.Result;

namespace PK.Optional.ResultExtensions;

public static class Extensions
{
	public static Option<T> ToOption<T>(this IResult<T> result) =>
		result.Success ? result.Value.Some() : Option.None<T>();

	public static IResult<T> ToResult<T>(this Option<T> option) =>
		option.Match(
			Result.Result.Success,
			() => Result.Result.Failed<T>(new Error("OptionHasNoValue", "Option value is missing"))
			);
}
