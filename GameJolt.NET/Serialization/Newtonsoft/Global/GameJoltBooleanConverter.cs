#if !NET6_0_OR_GREATER
using System;
using Newtonsoft.Json;

namespace Hertzole.GameJolt
{
	internal sealed class GameJoltBooleanConverter : JsonConverter<bool>
	{
		public static readonly GameJoltBooleanConverter Instance = new GameJoltBooleanConverter();

		public override void WriteJson(JsonWriter writer, bool value, JsonSerializer serializer)
		{
			writer.WriteValue(value);
		}

		public override bool ReadJson(JsonReader reader, Type objectType, bool existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			switch (reader.TokenType)
			{
				case JsonToken.Boolean:
					return (bool) reader.Value!;
				case JsonToken.Integer:
					return (int) reader.Value! != 0;
				case JsonToken.String:
					string? value = reader.ReadAsString();

					if (string.IsNullOrEmpty(value))
					{
						throw new JsonSerializationException("Value cannot be null or empty.");
					}

					if (value.Equals("true", StringComparison.OrdinalIgnoreCase))
					{
						return true;
					}

					if (value.Equals("false", StringComparison.OrdinalIgnoreCase))
					{
						return false;
					}

					if (value.Equals("0", StringComparison.OrdinalIgnoreCase))
					{
						return false;
					}

					if (value.Equals("1", StringComparison.OrdinalIgnoreCase))
					{
						return true;
					}

					if (value.Equals("yes", StringComparison.OrdinalIgnoreCase))
					{
						return true;
					}

					if (value.Equals("no", StringComparison.OrdinalIgnoreCase))
					{
						return false;
					}

					throw new JsonSerializationException($"Unknown boolean value: {value}");
				default:
					throw new JsonSerializationException($"Invalid token type. Expected boolean, number, or string. Got {reader.TokenType}.");
			}
		}
	}
}
#endif