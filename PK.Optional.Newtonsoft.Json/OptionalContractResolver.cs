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

		var contract = _resolver.ResolveContract(type);
		if (contract is JsonObjectContract objectContract)
		{
			foreach (var property in objectContract.Properties)
			{
				if (!property.Ignored && property.PropertyType?.IsGenericType == true
					&& property.Readable && property.ValueProvider != null
					&& property.PropertyType.GetGenericTypeDefinition() == typeof(Optional<>))
				{
					var shouldSerialize = property.ShouldSerialize;
					property.ShouldSerialize = shouldSerialize != null
						? o => Optional.HasValue(property.ValueProvider.GetValue(o)) && shouldSerialize(o)
						: o =>Optional.HasValue(property.ValueProvider.GetValue(o));
				}
			}
		}
		else if (contract is JsonDynamicContract dynamicContract)
		{
			foreach (var property in dynamicContract.Properties)
			{
				if (!property.Ignored && property.PropertyType?.IsGenericType == true
					&& property.Readable && property.ValueProvider != null
					&& property.PropertyType.GetGenericTypeDefinition() == typeof(Optional<>))
				{
					var shouldSerialize = property.ShouldSerialize;
					property.ShouldSerialize = shouldSerialize != null
						? o => Optional.HasValue(property.ValueProvider.GetValue(o)) && shouldSerialize(o)
						: o =>Optional.HasValue(property.ValueProvider.GetValue(o));
				}
			}
		}

		return contract;
	}


}
