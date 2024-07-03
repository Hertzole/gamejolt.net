#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

#if NET6_0_OR_GREATER || FORCE_SYSTEM_JSON
#nullable enable

using System;
using System.Text.Json;
using Hertzole.GameJolt.Serialization.Shared;

namespace Hertzole.GameJolt.Serialization.System
{
	internal sealed class GameJoltBooleanConverter : BaseBooleanConverter
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
					string? value = reader.GetString();

					if (!TryReadString(value, out bool result))
					{
						throw new JsonException(INVALID_STRING);
					}

					return result;
					
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
#endif // DISABLE_GAMEJOLT