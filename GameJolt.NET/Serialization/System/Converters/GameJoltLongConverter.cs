using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hertzole.GameJolt
{
	internal sealed class GameJoltLongConverter : JsonConverter<long>
	{
		public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			switch (reader.TokenType)
			{
				case JsonTokenType.Number:
					return reader.GetInt64();
				case JsonTokenType.String:
					if (long.TryParse(reader.GetString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out long result))
					{
						return result;
					}

					throw new JsonException("Could not parse string to long.");
				default:
					throw new JsonException("Expected number or string.");
			}
		}

		public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
		{
			writer.WriteNumberValue(value);
		}
	}
}