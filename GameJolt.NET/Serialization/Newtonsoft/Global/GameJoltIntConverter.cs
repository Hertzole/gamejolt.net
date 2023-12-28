#if !NET6_0_OR_GREATER
using System;
using Newtonsoft.Json;

namespace Hertzole.GameJolt
{
	internal sealed class GameJoltIntConverter : JsonConverter<int>
	{
		public static readonly GameJoltIntConverter Instance = new GameJoltIntConverter();

		public override bool CanRead
		{
			get { return true; }
		}

		public override bool CanWrite
		{
			get { return true; }
		}

		public override void WriteJson(JsonWriter writer, int value, JsonSerializer serializer)
		{
			writer.WriteValue(value);
		}

		public override int ReadJson(JsonReader reader, Type objectType, int existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			switch (reader.TokenType)
			{
				case JsonToken.Integer:
					return reader.ReadAsInt32()!.Value;
				case JsonToken.Float:
					return (int) reader.ReadAsDouble()!.Value;
			}

			string? stringValue = reader.ReadAsString();

			if (string.IsNullOrEmpty(stringValue))
			{
				throw new JsonSerializationException("Value cannot be null or empty.");
			}

			if (int.TryParse(stringValue, out int intValue))
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