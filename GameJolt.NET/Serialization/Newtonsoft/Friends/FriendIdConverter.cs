#if !NET6_0_OR_GREATER
using System;
using Newtonsoft.Json;

namespace Hertzole.GameJolt.Serialization.Newtonsoft
{
	internal sealed class FriendIdConverter : JsonConverter<FriendId>
	{
		public override void WriteJson(JsonWriter writer, FriendId value, JsonSerializer serializer)
		{
			writer.WriteStartObject();
			writer.WritePropertyName("friend_id");
			GameJoltIntConverter.Instance.WriteJson(writer, value.id, serializer);
			writer.WriteEndObject();
		}

		public override FriendId ReadJson(JsonReader reader, Type objectType, FriendId existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			int id = 0;

			reader.Read();

			while (reader.TokenType != JsonToken.EndObject)
			{
				// Skip unknown types.
				if (reader.TokenType != JsonToken.PropertyName)
				{
					reader.Skip();
					reader.Read();
					continue;
				}

				// Read the property name.
				string propertyName = (string) reader.Value!;

				if (propertyName.Equals("friend_id", StringComparison.OrdinalIgnoreCase))
				{
					id = GameJoltIntConverter.Instance.ReadJson(reader, typeof(int), existingValue.id, false, serializer);
				}

				// Read the next property name.
				reader.Read();
			}

			return new FriendId(id);
		}
	}
}
#endif