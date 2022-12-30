using System;
using Newtonsoft.Json;

namespace PK.Optional.Newtonsoft.Json;

public class OptionalJsonConverter : JsonConverter
{
	private static readonly Type OptionType = typeof(Optional<>);
	
	public static readonly OptionalJsonConverter Instance = new();
	
	#region Overrides of JsonConverter

	/// <inheritdoc />
	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		writer.WriteValue(Optional.GetValue(value));
	}

	/// <inheritdoc />
	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) =>
		Activator.CreateInstance(OptionType.MakeGenericType(objectType.GetGenericArguments()), reader.Value);

	/// <inheritdoc />
	public override bool CanConvert(Type objectType) => objectType.IsOptional();
	
	
	#endregion
}
