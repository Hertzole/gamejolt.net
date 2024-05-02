#if !NET6_0_OR_GREATER
using System;
using Newtonsoft.Json;

namespace Hertzole.GameJolt.Serialization.Newtonsoft
{
	internal sealed class TableInternalConverter : JsonConverter<TableInternal>
	{
		public override void WriteJson(JsonWriter writer, TableInternal value, JsonSerializer serializer)
		{
			writer.WriteStartObject();

			writer.WritePropertyName("id");
			GameJoltIntConverter.Instance.WriteJson(writer, value.id, serializer);
			writer.WritePropertyName("name");
			writer.WriteValue(value.name);
			writer.WritePropertyName("description");
			writer.WriteValue(value.description);
			writer.WritePropertyName("primary");
			GameJoltBooleanConverter.Instance.WriteJson(writer, value.isPrimary, serializer);

			writer.WriteEndObject();
		}

		public override TableInternal ReadJson(JsonReader reader,
			Type objectType,
			TableInternal existingValue,
			bool hasExistingValue,
			JsonSerializer serializer)
		{
			// First, read the start object token.
			reader.Read();

			int id = -1;
			string name = string.Empty;
			string description = string.Empty;
			bool isPrimary = false;

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

				if (propertyName.Equals("id", StringComparison.OrdinalIgnoreCase))
				{
					id = GameJoltIntConverter.Instance.ReadJson(reader, typeof(int), 0, false, serializer);
				}
				else if (propertyName.Equals("name", StringComparison.OrdinalIgnoreCase))
				{
					name = reader.ReadAsString() ?? string.Empty;
				}
				else if (propertyName.Equals("description", StringComparison.OrdinalIgnoreCase))
				{
					description = reader.ReadAsString()! ?? string.Empty;
				}
				else if (propertyName.Equals("primary", StringComparison.OrdinalIgnoreCase))
				{
					isPrimary = GameJoltBooleanConverter.Instance.ReadJson(reader, typeof(bool), false, false, serializer);
				}

				// Read the next property name.
				reader.Read();
			}

			return new TableInternal(id, name, description, isPrimary);
		}
	}
}
#endif