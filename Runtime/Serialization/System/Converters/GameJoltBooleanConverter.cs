#if NET6_0_OR_GREATER
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
					long number = reader.GetInt64();
					return ReadNumber(number);
				case JsonTokenType.String:
					string? value = reader.GetString();
					return ReadString(value);
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