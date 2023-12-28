#if !NET6_0_OR_GREATER
#nullable enable
using System;
using Newtonsoft.Json;

namespace Hertzole.GameJolt
{
	internal sealed class ScoreInternalConverter : JsonConverter<ScoreInternal>
	{
		public override void WriteJson(JsonWriter writer, ScoreInternal value, JsonSerializer serializer)
		{
			writer.WriteStartObject();

			writer.WritePropertyName("sort");
			GameJoltIntConverter.Instance.WriteJson(writer, value.sort, serializer);
			writer.WritePropertyName("score");
			writer.WriteValue(value.score);
			if (!string.IsNullOrEmpty(value.extraData))
			{
				writer.WritePropertyName("extra_data");
				writer.WriteValue(value.extraData);
			}

			if (!string.IsNullOrEmpty(value.username))
			{
				writer.WritePropertyName("user");
				writer.WriteValue(value.username);
			}

			if (value.userId != 0)
			{
				writer.WritePropertyName("user_id");
				GameJoltIntConverter.Instance.WriteJson(writer, value.userId, serializer);
			}

			if (!string.IsNullOrEmpty(value.guestName))
			{
				writer.WritePropertyName("guest");
				writer.WriteValue(value.guestName);
			}

			writer.WritePropertyName("stored_timestamp");
			GameJoltLongConverter.Instance.WriteJson(writer, value.storedTimestamp, serializer);
			writer.WritePropertyName("stored");
			writer.WriteValue(value.stored);

			writer.WriteEndObject();
		}

		public override ScoreInternal ReadJson(JsonReader reader,
			Type objectType,
			ScoreInternal existingValue,
			bool hasExistingValue,
			JsonSerializer serializer)
		{
			// First, read the object start.
			reader.Read();

			// Read the properties.
			int sort = 0;
			string score = string.Empty;
			string extraData = string.Empty;
			string user = string.Empty;
			int userId = 0;
			string guest = string.Empty;
			long storedTimestamp = 0;
			string stored = string.Empty;

			while (reader.TokenType != JsonToken.EndObject)
			{
				// Read the property name.
				string propertyName = (string) reader.Value!;

				if (propertyName.Equals("sort", StringComparison.OrdinalIgnoreCase))
				{
					sort = GameJoltIntConverter.Instance.ReadJson(reader, typeof(int), 0, false, serializer);
				}
				else if (propertyName.Equals("score", StringComparison.OrdinalIgnoreCase))
				{
					score = reader.ReadAsString() ?? string.Empty;
				}
				else if (propertyName.Equals("extra_data", StringComparison.OrdinalIgnoreCase))
				{
					extraData = reader.ReadAsString() ?? string.Empty;
				}
				else if (propertyName.Equals("user", StringComparison.OrdinalIgnoreCase))
				{
					user = reader.ReadAsString() ?? string.Empty;
				}
				else if (propertyName.Equals("user_id", StringComparison.OrdinalIgnoreCase))
				{
					userId = GameJoltIntConverter.Instance.ReadJson(reader, typeof(int), 0, false, serializer);
				}
				else if (propertyName.Equals("guest", StringComparison.OrdinalIgnoreCase))
				{
					guest = reader.ReadAsString() ?? string.Empty;
				}
				else if (propertyName.Equals("stored_timestamp", StringComparison.OrdinalIgnoreCase))
				{
					storedTimestamp = GameJoltLongConverter.Instance.ReadJson(reader, typeof(long), 0, false, serializer);
				}
				else if (propertyName.Equals("stored", StringComparison.OrdinalIgnoreCase))
				{
					stored = reader.ReadAsString() ?? string.Empty;
				}
				else
				{
					throw new JsonSerializationException($"Unknown property: {propertyName}");
				}

				// Read the next property name.
				reader.Read();
			}

			return new ScoreInternal(sort, score, extraData, user, userId, guest, stored, storedTimestamp);
		}
	}
}
#endif