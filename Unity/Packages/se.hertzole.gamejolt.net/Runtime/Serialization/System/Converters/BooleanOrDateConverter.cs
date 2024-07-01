#if NET6_0_OR_GREATER
#nullable enable

using System;
using System.Text.Json;
using Hertzole.GameJolt.Serialization.Shared;

namespace Hertzole.GameJolt.Serialization.System
{
	internal sealed class BooleanOrDateConverter : BaseBooleanConverter
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
					return ReadNumber(reader.GetInt64());
				case JsonTokenType.String:
					// This will fail if the string is a date, or not a boolean.
					if (TryReadString(reader.GetString(), out bool result))
					{
						return result;
					}

					// So for now we'll just assume it's a date.
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