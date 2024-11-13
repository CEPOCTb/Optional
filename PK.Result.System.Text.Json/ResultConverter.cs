using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PK.Result.System.Text.Json;

public class ResultConverter : JsonConverter<Result>
{

	#region Overrides of JsonConverter<Result>
	/// <inheritdoc />
	public override Result Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (reader.TokenType == JsonTokenType.Null)
		{
			return null;
		}

		var success = false;
		var cancelled = false;
		IError[] error = null;
		object value = null;
		var type = typeToConvert.IsGenericType ? typeToConvert.GetGenericArguments()[0] : null;

		while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
		{
			if (reader.TokenType == JsonTokenType.PropertyName)
			{
				var propertyName = reader.GetString()?.ToLowerInvariant();

				reader.Read();

				switch (propertyName)
				{
					case "issuccess":
					case "is_success":
					{
						success = reader.GetBoolean();
						break;
					}
					case "iscancelled":
					case "is_cancelled":
					{
						cancelled = reader.GetBoolean();
						break;
					}
					case "errors":
					{
						error = JsonSerializer.Deserialize<Error[]>(ref reader)?.Cast<IError>().ToArray();
						break;
					}
					case "value":
					{
						if (type != null)
						{
							value = JsonSerializer.Deserialize(ref reader, type);
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

	/// <inheritdoc />
	public override void Write(Utf8JsonWriter writer, Result value, JsonSerializerOptions options)
	{
		writer.WriteStartObject();
		writer.WritePropertyName(options.PropertyNamingPolicy?.ConvertName(nameof(Result.IsSuccess)) ?? nameof(Result.IsSuccess));
		writer.WriteBooleanValue(value.IsSuccess);

		if (!value.IsSuccess || options.DefaultIgnoreCondition != JsonIgnoreCondition.Never)
		{
			writer.WritePropertyName(options.PropertyNamingPolicy?.ConvertName(nameof(Result.IsCancelled)) ?? nameof(Result.IsCancelled));
			writer.WriteBooleanValue(value.IsCancelled);

			writer.WritePropertyName(options.PropertyNamingPolicy?.ConvertName(nameof(Result.Errors)) ?? nameof(Result.Errors));
			writer.WriteRawValue(JsonSerializer.SerializeToUtf8Bytes(value.Errors, options));
		}

		if (value.IsSuccess)
		{
			if (value.TryGetValue(out var val))
			{
				writer.WritePropertyName(options.PropertyNamingPolicy?.ConvertName("Value") ?? "Value");
				writer.WriteRawValue(JsonSerializer.SerializeToUtf8Bytes(val, options));
			}
		}

		writer.WriteEndObject();
	}
	#endregion
}
