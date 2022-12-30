using System;
using System.Collections.Generic;
using System.Linq;

namespace PK.Optional.Linq;

public static class LinqExtensions
{
	public static Optional<IEnumerable<TOut>> Select<TIn, TOut>(this Optional<IEnumerable<TIn>> enumerable, Func<TIn, TOut> mapFunc) => 
		!enumerable.HasValue ? Optional.Empty<IEnumerable<TOut>>() : Optional.FromValue(enumerable.Value.Select(mapFunc));
}
