using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PK.Result.Newtonsoft.Json;

/// <summary>
///  JsonConverter for <see cref="Result"/> and <see cref="Result{T}"/> instances
/// </summary>
public class ResultConverter : JsonConverter<Result>
{
	private readonly NamingStrategy _namingStrategy;

	public ResultConverter(NamingStrategy namingStrategy = null)
	{
		_namingStrategy = namingStrategy;
	}

	#region Overrides of JsonConverter<IResult>

	/// <inheritdoc />
	public override void WriteJson(JsonWriter writer, Result value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName(_namingStrategy?.GetPropertyName(nameof(Result.IsSuccess), false) ?? nameof(Result.IsSuccess));
		writer.WriteValue(value.IsSuccess);

		if (!value.IsSuccess || serializer.DefaultValueHandling != DefaultValueHandling.Ignore)
		{
			writer.WritePropertyName(_namingStrategy?.GetPropertyName(nameof(Result.IsCancelled), false) ?? nameof(Result.IsCancelled));
			writer.WriteValue(value.IsCancelled);

			writer.WritePropertyName(_namingStrategy?.GetPropertyName(nameof(Result.Errors), false) ?? nameof(Result.Errors));
			serializer.Serialize(writer, value.Errors);
		}

		if (value.IsSuccess)
		{
			if (value.TryGetValue(out var val))
			{
				writer.WritePropertyName(_namingStrategy?.GetPropertyName("Value", false) ?? "Value");
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

		var success = false;
		var cancelled = false;
		Error error = null;
		object value = null;
		var type = objectType.IsGenericType ? objectType.GetGenericArguments()[0] : null;

		while (reader.Read() && reader.TokenType != JsonToken.EndObject)
		{
			if (reader.TokenType == JsonToken.PropertyName)
			{
				switch (reader.Path.ToLowerInvariant())
				{
					case "issuccess":
						{
							success = reader.ReadAsBoolean() ?? false;
							break;
						}
					case "iscancelled":
						{
							cancelled = reader.ReadAsBoolean() ?? false;
							break;
						}
					case "errors":
						{
							reader.Read();
							error = serializer.Deserialize<Error>(reader);
							break;
						}
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
