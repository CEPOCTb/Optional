using Newtonsoft.Json.Serialization;
using System;
using System.Linq;

namespace PK.Optional.Newtonsoft.Json;

public class OptionalContractResolver : IContractResolver
{
	private readonly IContractResolver _resolver;

	public OptionalContractResolver(IContractResolver resolver)
	{
		_resolver = resolver;
	}

	/// <inheritdoc />
	public JsonContract ResolveContract(Type type)
	{
		if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Optional<>))
		{
			return new JsonPrimitiveContract(type.GetGenericArguments().First())
			{
				Converter = OptionalJsonConverter.Instance
			};
		}

		return _resolver.ResolveContract(type);
	}
}
