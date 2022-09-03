using System;
using System.Collections.Generic;
using System.Linq;

namespace PK.Optional.Linq;

public static class LinqExtensions
{
	public static Option<IEnumerable<TOut>> Select<TIn, TOut>(this Option<IEnumerable<TIn>> enumerable, Func<TIn, TOut> mapFunc) => 
		!enumerable.HasValue ? Option.None<IEnumerable<TOut>>() : Option.Some(enumerable.Value.Select(mapFunc));
}
