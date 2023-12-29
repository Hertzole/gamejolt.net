#if !NET6_0_OR_GREATER
#nullable enable

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
			reader.Read();

			switch (reader.TokenType)
			{
				case JsonToken.Integer:
				case JsonToken.Float:
					return Convert.ToInt32((long) reader.Value!);
				case JsonToken.String:
					string? stringValue = reader.Value as string;

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

					throw new JsonSerializationException($"Could not parse string ({stringValue}) to int.");
				default:
					throw new JsonSerializationException($"Unexpected token type {reader.TokenType}. Expected Integer or String.");
			}
		}
	}
}
#endif