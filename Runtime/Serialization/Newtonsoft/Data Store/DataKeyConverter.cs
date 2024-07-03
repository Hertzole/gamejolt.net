#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

#if !NET6_0_OR_GREATER && !FORCE_SYSTEM_JSON
#nullable enable

using System;
using Newtonsoft.Json;

namespace Hertzole.GameJolt.Serialization.Newtonsoft
{
	internal sealed class DataKeyConverter : JsonConverter<DataKey>
	{
		public override void WriteJson(JsonWriter writer, DataKey value, JsonSerializer serializer)
		{
			writer.WriteStartObject();
			writer.WritePropertyName("key");
			writer.WriteValue(value.key);
			writer.WriteEndObject();
		}

		public override DataKey ReadJson(JsonReader reader, Type objectType, DataKey existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			string key = string.Empty;

			reader.Read();

			while (reader.TokenType != JsonToken.EndObject)
			{
				if (reader.TokenType != JsonToken.PropertyName)
				{
					reader.Skip();
					reader.Read();
					continue;
				}

				// Read the property name.
				string propertyName = (string) reader.Value!;

				if (propertyName.Equals("key", StringComparison.OrdinalIgnoreCase))
				{
					key = reader.ReadAsString() ?? string.Empty;
				}

				// Read the next property name.
				reader.Read();
			}

			return new DataKey(key);
		}
	}
}
#endif
#endif // DISABLE_GAMEJOLT