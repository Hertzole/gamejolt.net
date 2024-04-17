#if !NET6_0_OR_GREATER
#nullable enable

using System;
using Newtonsoft.Json;

namespace Hertzole.GameJolt.Serialization.Newtonsoft
{
	internal sealed class BooleanOrDateConverter : JsonConverter<bool>
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
					long intValue = (long) reader.Value!;
					switch (intValue)
					{
						case 0:
							return false;
						case 1:
							return true;
						default:
							throw new JsonSerializationException($"Can't convert to BooleanOrDate from {reader.TokenType}");
					}
				case JsonToken.String:
				{
					string? value = (string) reader.Value!;
					if (string.IsNullOrEmpty(value))
					{
						throw new JsonSerializationException("Expected string, it was empty or null.");
					}

					if (value.Equals("true", StringComparison.OrdinalIgnoreCase))
					{
						return true;
					}

					if (value.Equals("false", StringComparison.OrdinalIgnoreCase))
					{
						return false;
					}

					if (value.Equals("1", StringComparison.OrdinalIgnoreCase))
					{
						return true;
					}

					if (value.Equals("0", StringComparison.OrdinalIgnoreCase))
					{
						return false;
					}

					return true;
				}
				default:
					throw new JsonSerializationException($"Can't convert to BooleanOrDate from {reader.TokenType}");
			}
		}
	}
}
#endif