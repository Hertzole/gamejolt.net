#if NET6_0_OR_GREATER
#nullable enable

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hertzole.GameJolt
{
	internal sealed class GameJoltBooleanConverter : JsonConverter<bool>
	{
		public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			switch (reader.TokenType)
			{
				case JsonTokenType.True:
					return true;
				case JsonTokenType.False:
					return false;
				case JsonTokenType.Number:
					return reader.GetInt32() != 0;
				case JsonTokenType.String:
					string? value = reader.GetString();
					if (string.IsNullOrEmpty(value))
					{
						throw new JsonException("Empty string is not a valid boolean.");
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

					throw new JsonException($"Invalid boolean value ({value})");
				default:
					throw new JsonException("Invalid token type. Expected boolean, number, or string.");
			}
		}

		public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
		{
			writer.WriteBooleanValue(value);
		}
	}
}
#endif