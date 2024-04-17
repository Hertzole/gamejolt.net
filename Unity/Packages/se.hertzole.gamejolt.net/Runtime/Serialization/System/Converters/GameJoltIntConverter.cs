#if NET6_0_OR_GREATER
using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hertzole.GameJolt.Serialization.System
{
	internal sealed class GameJoltIntConverter : JsonConverter<int>
	{
		public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			switch (reader.TokenType)
			{
				case JsonTokenType.Number:
					return reader.GetInt32();
				case JsonTokenType.String:
					if (int.TryParse(reader.GetString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out int result))
					{
						return result;
					}

					throw new JsonException("Could not parse string to int.");
				default:
					throw new JsonException("Expected number or string.");
			}
		}

		public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
		{
			writer.WriteNumberValue(value);
		}
	}
}
#endif