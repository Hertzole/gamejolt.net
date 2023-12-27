#if UNITY_2021_1_OR_NEWER || !NET6_0_OR_GREATER
using System;
using Newtonsoft.Json;

namespace Hertzole.GameJolt
{
	internal abstract class ResponseConverter<T> : JsonConverter<T>
	{
		public override bool CanWrite
		{
			get { return false; }
		}

		public override void WriteJson(JsonWriter writer, T value, JsonSerializer serializer)
		{
			throw new NotSupportedException("This converter is only used for reading.");
		}

		public override T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			// Deserialize { "response": { "success": true, "message": "Success!" } }

			// First, read the start object token.
			reader.Read();
			// Then read the property name.
			reader.Read();
			// Then read the start object token.
			reader.Read();

			T response = ReadResponse(reader, serializer);

			// We're now at the end of the response object.
			// Read the end object token.
			reader.Read();
			// Read the end object token.
			reader.Read();

			return response;
		}

		protected abstract T ReadResponse(JsonReader reader, JsonSerializer serializer);
	}
}
#endif