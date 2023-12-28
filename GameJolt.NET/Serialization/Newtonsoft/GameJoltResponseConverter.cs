#if UNITY_2021_1_OR_NEWER || !NET6_0_OR_GREATER
using System;
using Newtonsoft.Json;

namespace Hertzole.GameJolt
{
	internal sealed class GameJoltResponseConverter<T> : JsonConverter<GameJoltResponse<T>>
	{
		public override bool CanWrite
		{
			get { return true; }
		}

		public override void WriteJson(JsonWriter writer, GameJoltResponse<T> value, JsonSerializer serializer)
		{
			// Serialize { "response": { "success": true, "message": "Success!" } }

			writer.WriteStartObject();
			writer.WritePropertyName("response");
			serializer.Serialize(writer, value.response, typeof(T));
			writer.WriteEndObject();
		}

		public override GameJoltResponse<T> ReadJson(JsonReader reader, Type objectType, GameJoltResponse<T> existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			// Deserialize { "response": { "success": true, "message": "Success!" } }

			// First, read the start object token.
			reader.Read();
			// Then read the property name.
			reader.Read();

			T response = serializer.Deserialize<T>(reader);
			
			// Read the end object token.
			reader.Read();

			return new GameJoltResponse<T>(response);
		}
	}
}
#endif