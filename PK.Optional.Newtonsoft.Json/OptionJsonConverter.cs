using System;
using Newtonsoft.Json;

namespace PK.Optional.Newtonsoft.Json;

public class OptionJsonConverter : JsonConverter
{
	private static readonly Type OptionType = typeof(Option<>);
	
	public static readonly OptionJsonConverter Instance = new();
	
	#region Overrides of JsonConverter

	/// <inheritdoc />
	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		writer.WriteValue(Option.GetValue(value));
	}

	/// <inheritdoc />
	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) =>
		Activator.CreateInstance(OptionType.MakeGenericType(objectType.GetGenericArguments()), reader.Value);

	/// <inheritdoc />
	public override bool CanConvert(Type objectType) => objectType.IsOption();
	
	
	#endregion
}
