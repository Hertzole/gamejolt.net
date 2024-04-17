#if !NET6_0_OR_GREATER
#nullable enable

using System;
using System.Globalization;
using Newtonsoft.Json;

namespace Hertzole.GameJolt.Serialization.Newtonsoft
{
	internal abstract class GameJoltEnumConverter<T> : JsonConverter<T> where T : struct, Enum
	{
		public sealed override void WriteJson(JsonWriter writer, T value, JsonSerializer serializer)
		{
			writer.WriteValue(value.ToString());
		}

		public sealed override T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			reader.Read();

			switch (reader.TokenType)
			{
				case JsonToken.Integer:
					long numberValue = (long) reader.Value!;

					if (GetValueFromInt(Convert.ToInt32(numberValue), out T numberResult))
					{
						return numberResult;
					}

					throw new JsonSerializationException($"Could not parse number ({numberValue}) to enum.");
				case JsonToken.String:
					string? value = (string) reader.Value!;

					if (string.IsNullOrEmpty(value))
					{
						throw new JsonSerializationException("Expected non-empty string.");
					}

					if (GetValueFromString(value, out T result))
					{
						return result;
					}

					if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int intValue) && GetValueFromInt(intValue, out T intResult))
					{
						return intResult;
					}

					throw new JsonSerializationException($"Could not parse string ({value}) to enum.");
				default:
					throw new JsonSerializationException($"Expected number or string, got {reader.TokenType}.");
			}
		}

		protected abstract bool GetValueFromString(string value, out T result);

		protected abstract bool GetValueFromInt(int value, out T result);
	}
}
#endif