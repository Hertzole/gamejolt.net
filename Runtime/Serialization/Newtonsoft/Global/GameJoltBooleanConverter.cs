#if !NET6_0_OR_GREATER
#nullable enable

using System;
using Hertzole.GameJolt.Serialization.Shared;
using Newtonsoft.Json;

namespace Hertzole.GameJolt.Serialization.Newtonsoft
{
	internal sealed class GameJoltBooleanConverter : BaseBooleanConverter
	{
		public static readonly GameJoltBooleanConverter Instance = new GameJoltBooleanConverter();

		public override void WriteJson(JsonWriter writer, bool value, JsonSerializer serializer)
		{
			writer.WriteValue(value);
		}

		public override bool ReadJson(JsonReader reader, Type objectType, bool existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			reader.Read();

			switch (reader.TokenType)
			{
				case JsonToken.Boolean:
					return (bool) reader.Value!;
				case JsonToken.Integer:
					return ReadNumber((long) reader.Value!);
				case JsonToken.String:
					if (!TryReadString((string) reader.Value!, out bool result))
					{
						throw new JsonSerializationException(INVALID_STRING);
					}

					return result;

				default:
					throw new JsonSerializationException($"Invalid token type. Expected boolean, number, or string. Got {reader.TokenType}.");
			}
		}
	}
}
#endif