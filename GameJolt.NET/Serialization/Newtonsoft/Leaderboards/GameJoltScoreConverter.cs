#if UNITY_2021_1_OR_NEWER || !NET6_0_OR_GREATER
#nullable enable
using System;
using Newtonsoft.Json;

namespace Hertzole.GameJolt
{
	internal sealed class GameJoltScoreConverter : JsonConverter<GameJoltScore>
	{
		public override void WriteJson(JsonWriter writer, GameJoltScore value, JsonSerializer serializer)
		{
			throw new NotSupportedException();
		}

		public override GameJoltScore ReadJson(JsonReader reader,
			Type objectType,
			GameJoltScore existingValue,
			bool hasExistingValue,
			JsonSerializer serializer)
		{
			// First, read the object start.
			reader.Read();

			// Read the properties.
			int sort = 0;
			string score = string.Empty;
			string? extraData = null;
			string? user = null;
			int? userId = null;
			string? guest = null;
			DateTime stored = DateTime.MinValue;

			while (reader.TokenType != JsonToken.EndObject)
			{
				// Read the property name.
				string propertyName = (string) reader.Value!;

				if (propertyName.Equals("sort", StringComparison.OrdinalIgnoreCase))
				{
					sort = reader.ReadAsInt32() ?? 0;
				}
				else if (propertyName.Equals("score", StringComparison.OrdinalIgnoreCase))
				{
					score = reader.ReadAsString() ?? string.Empty;
				}
				else if (propertyName.Equals("extra_data", StringComparison.OrdinalIgnoreCase))
				{
					extraData = reader.ReadAsString();
				}
				else if (propertyName.Equals("user", StringComparison.OrdinalIgnoreCase))
				{
					user = reader.ReadAsString();
				}
				else if (propertyName.Equals("user_id", StringComparison.OrdinalIgnoreCase))
				{
					userId = reader.ReadAsInt32();
				}
				else if (propertyName.Equals("guest", StringComparison.OrdinalIgnoreCase))
				{
					guest = reader.ReadAsString();
				}
				else if (propertyName.Equals("stored_timestamp", StringComparison.OrdinalIgnoreCase))
				{
					stored = DateTimeHelper.FromUnixTimestamp(reader.ReadAsInt32() ?? 0);
				}
				else if (propertyName.Equals("stored", StringComparison.OrdinalIgnoreCase)) // Ignore stored since we already have stored_timestamp. 
				{
					reader.Read();
				}
				else
				{
					throw new JsonSerializationException($"Unknown property: {propertyName}");
				}

				// Read the next property name.
				reader.Read();
			}

			return new GameJoltScore(sort, score, extraData, user, userId, guest, stored);
		}
	}
}
#endif