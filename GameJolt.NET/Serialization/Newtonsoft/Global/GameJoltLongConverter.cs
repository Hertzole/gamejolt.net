#if !NET6_0_OR_GREATER
using System;
using Newtonsoft.Json;

namespace Hertzole.GameJolt
{
	internal sealed class GameJoltLongConverter : JsonConverter<long>
	{
		public static readonly GameJoltLongConverter Instance = new GameJoltLongConverter();

		public override bool CanRead
		{
			get { return true; }
		}

		public override bool CanWrite
		{
			get { return true; }
		}

		public override void WriteJson(JsonWriter writer, long value, JsonSerializer serializer)
		{
			writer.WriteValue(value);
		}

		public override long ReadJson(JsonReader reader, Type objectType, long existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			switch (reader.TokenType)
			{
				case JsonToken.Integer:
					return (long) reader.Value!;
				case JsonToken.Float:
					return (int) reader.ReadAsDouble()!.Value;
			}

			string? stringValue = reader.ReadAsString();

			if (string.IsNullOrEmpty(stringValue))
			{
				throw new JsonSerializationException("Value cannot be null or empty.");
			}

			if (long.TryParse(stringValue, out long intValue))
			{
				return intValue;
			}

			if (double.TryParse(stringValue, out double doubleValue))
			{
				return (int) doubleValue;
			}

			throw new JsonSerializationException("Value is not a number.");
		}
	}
}
#endif