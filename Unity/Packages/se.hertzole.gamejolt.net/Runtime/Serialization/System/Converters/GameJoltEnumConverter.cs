#if NET6_0_OR_GREATER
#nullable enable

using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hertzole.GameJolt.Serialization.System
{
	internal abstract class GameJoltEnumConverter<T> : JsonConverter<T> where T : struct, Enum
	{
		public sealed override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			switch (reader.TokenType)
			{
				case JsonTokenType.Number:
					int numberValue = reader.GetInt32();

					if (GetValueFromInt(numberValue, out T numberResult))
					{
						return numberResult;
					}

					throw new JsonException($"Could not parse number ({numberValue}) to enum.");
				case JsonTokenType.String:
					string? value = reader.GetString();

					if (string.IsNullOrEmpty(value))
					{
						throw new JsonException("Expected non-empty string.");
					}

					if (GetValueFromString(value, out T result))
					{
						return result;
					}

					if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int intValue) && GetValueFromInt(intValue, out T intResult))
					{
						return intResult;
					}

					throw new JsonException($"Could not parse string ({value}) to enum.");
				default:
					throw new JsonException("Expected number or string.");
			}
		}

		public sealed override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
		{
			writer.WriteStringValue(value.ToString());
		}

		protected abstract bool GetValueFromString(string value, out T result);

		protected abstract bool GetValueFromInt(int value, out T result);
	}
}
#endif