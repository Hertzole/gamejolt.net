#if !NET6_0_OR_GREATER
#nullable enable

using System;
using System.Globalization;
using Newtonsoft.Json;

namespace Hertzole.GameJolt.Serialization.Newtonsoft
{
	internal sealed class GameJoltIntConverter : JsonConverter<int>
	{
		public static readonly GameJoltIntConverter Instance = new GameJoltIntConverter();

		public override void WriteJson(JsonWriter writer, int value, JsonSerializer serializer)
		{
			writer.WriteValue(value);
		}

		public override int ReadJson(JsonReader reader, Type objectType, int existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			reader.Read();

			switch (reader.TokenType)
			{
				case JsonToken.Integer:
					return Convert.ToInt32((long) reader.Value!);
				case JsonToken.Float:
					return Convert.ToInt32((double) reader.Value!);
				case JsonToken.String:
					string? stringValue = reader.Value as string;

					if (string.IsNullOrEmpty(stringValue))
					{
						throw new JsonSerializationException("Value cannot be null or empty.");
					}

					if (int.TryParse(stringValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out int intValue))
					{
						return intValue;
					}

					if (double.TryParse(stringValue, NumberStyles.Float, CultureInfo.InvariantCulture, out double doubleValue))
					{
						return (int) doubleValue;
					}

					throw new JsonSerializationException($"Could not parse string ({stringValue}) to int.");
				default:
					throw new JsonSerializationException($"Unexpected token type {reader.TokenType}. Expected Integer or String.");
			}
		}
	}
}
#endif