using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PK.Optional.Newtonsoft.Json;

public class OptionalContractResolver : DefaultContractResolver
{
	#region Overrides of DefaultContractResolver

	/// <inheritdoc />
	protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
	{
		var prop = base.CreateProperty(member, memberSerialization);
		if (!prop.PropertyType.IsOptional())
		{
			return prop;
		}

		prop.Converter = OptionalJsonConverter.Instance;
		prop.DefaultValueHandling = DefaultValueHandling.Ignore;

		return prop;
	}

	#endregion
}
