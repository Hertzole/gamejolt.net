#if !NET6_0_OR_GREATER
#nullable enable

using System;
using System.Globalization;
using Newtonsoft.Json;

namespace Hertzole.GameJolt.Serialization.Newtonsoft
{
	internal sealed class GameJoltLongConverter : JsonConverter<long>
	{
		public static readonly GameJoltLongConverter Instance = new GameJoltLongConverter();

		public override void WriteJson(JsonWriter writer, long value, JsonSerializer serializer)
		{
			writer.WriteValue(value);
		}

		public override long ReadJson(JsonReader reader, Type objectType, long existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			reader.Read();

			switch (reader.TokenType)
			{
				case JsonToken.Integer:
					return (long) reader.Value!;
				case JsonToken.Float:
					return Convert.ToInt64((double) reader.Value!);
			}

			string? stringValue = reader.Value as string;

			if (string.IsNullOrEmpty(stringValue))
			{
				throw new JsonSerializationException("Value cannot be null or empty.");
			}

			if (long.TryParse(stringValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out long intValue))
			{
				return intValue;
			}

			if (double.TryParse(stringValue, NumberStyles.Float, CultureInfo.InvariantCulture, out double doubleValue))
			{
				return (int) doubleValue;
			}

			throw new JsonSerializationException("Value is not a number.");
		}
	}
}
#endif