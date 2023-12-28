#if !NET6_0_OR_GREATER
#nullable enable

using System;
using System.Diagnostics;
using Newtonsoft.Json;

namespace Hertzole.GameJolt
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
			Debug.Assert(reader.TokenType == JsonToken.StartObject, "reader.TokenType == JsonToken.StartObject");

			// First, read the start object token.
			reader.Read();

			// Then read the property name.
			reader.Read();

			string? key = reader.ReadAsString();
			
			if(string.IsNullOrEmpty(key))
			{
				throw new JsonSerializationException("Key cannot be null or empty.");
			}

			// Read the end object token.
			reader.Read();

			return new DataKey(key!);
		}
	}
}
#endif