#if NET6_0_OR_GREATER
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hertzole.GameJolt
{
	internal sealed class BooleanOrDateConverter : JsonConverter<bool>
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
						throw new JsonException("Expected string, it was empty or null.");
					}

					if (value.Equals("true", StringComparison.OrdinalIgnoreCase))
					{
						return true;
					}

					if (value.Equals("false", StringComparison.OrdinalIgnoreCase))
					{
						return false;
					}
					
					if(value.Equals("1", StringComparison.OrdinalIgnoreCase))
					{
						return true;
					}
					
					if(value.Equals("0", StringComparison.OrdinalIgnoreCase))
					{
						return false;
					}

					return true;
				default:
					throw new JsonException($"Can't convert to BooleanOrDate from {reader.TokenType} ({reader.ValueSpan.ToString()})");
			}
		}

		public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
		{
			if (value)
			{
				writer.WriteStringValue("true");
			}
			else
			{
				writer.WriteBooleanValue(false);
			}
		}
	}
}
#endif