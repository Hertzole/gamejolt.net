#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

#if NET6_0_OR_GREATER || FORCE_SYSTEM_JSON
using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hertzole.GameJolt.Serialization.System
{
	internal sealed class GameJoltLongConverter : JsonConverter<long>
	{
		public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			switch (reader.TokenType)
			{
				case JsonTokenType.Number:
					if (reader.TryGetInt64(out long longValue))
					{
						return longValue;
					}

					return Convert.ToInt64(reader.GetDouble());
				case JsonTokenType.String:
					if (long.TryParse(reader.GetString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out long result))
					{
						return result;
					}

					if (double.TryParse(reader.GetString(), NumberStyles.Float, CultureInfo.InvariantCulture, out double doubleValue))
					{
						return Convert.ToInt64(doubleValue);
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
#endif
#endif // DISABLE_GAMEJOLT