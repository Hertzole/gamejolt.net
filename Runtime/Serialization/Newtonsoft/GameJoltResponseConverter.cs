#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

#if !NET6_0_OR_GREATER && !FORCE_SYSTEM_JSON
using System;
using Newtonsoft.Json;

namespace Hertzole.GameJolt.Serialization.Newtonsoft
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

		public override GameJoltResponse<T> ReadJson(JsonReader reader,
			Type objectType,
			GameJoltResponse<T> existingValue,
			bool hasExistingValue,
			JsonSerializer serializer)
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
#endif // DISABLE_GAMEJOLT