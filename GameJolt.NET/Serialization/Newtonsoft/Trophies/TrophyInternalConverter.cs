#if !NET6_0_OR_GREATER
using System;
using Newtonsoft.Json;

namespace Hertzole.GameJolt
{
	internal sealed class TrophyInternalConverter : JsonConverter<TrophyInternal>
	{
		public override void WriteJson(JsonWriter writer, TrophyInternal value, JsonSerializer serializer)
		{
			writer.WriteStartObject();

			writer.WritePropertyName("id");
			GameJoltIntConverter.Instance.WriteJson(writer, value.id, serializer);
			writer.WritePropertyName("title");
			writer.WriteValue(value.title);
			writer.WritePropertyName("description");
			writer.WriteValue(value.description);
			writer.WritePropertyName("difficulty");
			GameJoltTrophyDifficultyConverter.Instance.WriteJson(writer, value.difficulty, serializer);
			writer.WritePropertyName("image_url");
			writer.WriteValue(value.imageUrl);
			writer.WritePropertyName("achieved");
			BooleanOrDateConverter.Instance.WriteJson(writer, value.achieved, serializer);
			
			writer.WriteEndObject();
		}

		public override TrophyInternal ReadJson(JsonReader reader, Type objectType, TrophyInternal existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			int id = 0;
			string title = string.Empty;
			string description = string.Empty;
			TrophyDifficulty difficulty = TrophyDifficulty.Bronze;
			string imageUrl = string.Empty;
			bool achieved = false;

			while (reader.TokenType != JsonToken.EndObject)
			{
				// Read the property name.
				string propertyName = (string) reader.Value!;

				if (propertyName.Equals("id", StringComparison.OrdinalIgnoreCase))
				{
					reader.Read();
					id = GameJoltIntConverter.Instance.ReadJson(reader, typeof(int), 0, false, serializer);
				}
				else if (propertyName.Equals("title", StringComparison.OrdinalIgnoreCase))
				{
					reader.Read();
					title = (string) reader.Value!;
				}
				else if (propertyName.Equals("description", StringComparison.OrdinalIgnoreCase))
				{
					reader.Read();
					description = (string) reader.Value!;
				}
				else if (propertyName.Equals("difficulty", StringComparison.OrdinalIgnoreCase))
				{
					reader.Read();
					difficulty = GameJoltTrophyDifficultyConverter.Instance.ReadJson(reader, typeof(TrophyDifficulty), TrophyDifficulty.Bronze, false, serializer);
				}
				else if (propertyName.Equals("image_url", StringComparison.OrdinalIgnoreCase))
				{
					reader.Read();
					imageUrl = (string) reader.Value!;
				}
				else if (propertyName.Equals("achieved", StringComparison.OrdinalIgnoreCase))
				{
					reader.Read();
					achieved = BooleanOrDateConverter.Instance.ReadJson(reader, typeof(bool), false, false, serializer);
				}
				else
				{
					// Read the next property name.
					reader.Read();
				}
			}

			return new TrophyInternal(id, title, description, difficulty, imageUrl, achieved);
		}
	}
}
#endif