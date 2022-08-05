using System;
using Newtonsoft.Json;

namespace PK.Result.Newtonsoft.Json;

public class ResultConverter : JsonConverter<Result>
{
	#region Overrides of JsonConverter<IResult>

	/// <inheritdoc />
	public override void WriteJson(JsonWriter writer, Result value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName(nameof(Result.IsSuccess));
		writer.WriteValue(value.IsSuccess);

		if (!value.IsSuccess || serializer.DefaultValueHandling != DefaultValueHandling.Ignore)
		{
			writer.WritePropertyName(nameof(Result.Cancelled));
			writer.WriteValue(value.IsCancelled);

			writer.WritePropertyName(nameof(Result.Error));
			serializer.Serialize(writer, value.Error);
		}

		if (value.IsSuccess)
		{
			if (value.TryGetValue(out var val))
			{
				writer.WritePropertyName("Value");
				serializer.Serialize(writer, val);
			}
		}

		writer.WriteEndObject();
	}

	/// <inheritdoc />
	public override Result ReadJson(JsonReader reader, Type objectType, Result existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		if (reader.TokenType == JsonToken.Null)
		{
			return null;
		}

		bool success = false;
		bool cancelled = false;
		Error error = null;
		object value = null;
		Type type = objectType.IsGenericTypeDefinition ? objectType.GetGenericArguments()[0] : null;

		while (reader.Read() && reader.TokenType != JsonToken.EndObject)
		{
			if (reader.TokenType == JsonToken.PropertyName)
			{
				switch (reader.Path)
				{
					case "Success":
					case "success":
						{
							success = reader.ReadAsBoolean() ?? false;
							break;
						}
					case "Cancelled":
					case "cancelled":
						{
							cancelled = reader.ReadAsBoolean() ?? false;
							break;
						}
					case "Error":
					case "error":
						{
							reader.Read();
							error = serializer.Deserialize<Error>(reader);
							break;
						}
					case "Value":
					case "value":
						{
							if (reader.Read() && type != null)
							{
								value = serializer.Deserialize(reader, type);
							}
							break;
						}
				}
			}
		}

		if (type != null)
		{
			return cancelled ? Result.Cancelled(type) : !success ? Result.Failed(error, type) : Result.Success(value, type);
		}

		return cancelled ? Result.Cancelled() : !success ? Result.Failed(error) : Result.Success();
	}

	#endregion
}

