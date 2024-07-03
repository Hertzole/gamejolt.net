#if !NET6_0_OR_GREATER && !FORCE_SYSTEM_JSON
#nullable enable

using System;
using Hertzole.GameJolt.Serialization.Shared;
using Newtonsoft.Json;

namespace Hertzole.GameJolt.Serialization.Newtonsoft
{
	internal sealed class BooleanOrDateConverter : BaseBooleanConverter
	{
		public static readonly BooleanOrDateConverter Instance = new BooleanOrDateConverter();

		public override void WriteJson(JsonWriter writer, bool value, JsonSerializer serializer)
		{
			if (value)
			{
				writer.WriteValue("true");
			}
			else
			{
				writer.WriteValue(false);
			}
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
				{
					// This will fail if the string is a date, or not a boolean.
					if (TryReadString((string) reader.Value!, out bool result))
					{
						return result;
					}

					// So for now we'll just assume it's a date.
					return true;
				}
				default:
					throw new JsonSerializationException($"Can't convert to BooleanOrDate from {reader.TokenType}");
			}
		}
	}
}
#endif