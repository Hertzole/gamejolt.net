#if !NET6_0_OR_GREATER
#nullable enable
using System;
using Newtonsoft.Json;

namespace Hertzole.GameJolt
{
	internal abstract class ResponseConverter<T> : JsonConverter<T> where T : struct, IResponse
	{
		public sealed override bool CanRead
		{
			get { return true; }
		}

		public sealed override bool CanWrite
		{
			get { return false; }
		}

		public sealed override void WriteJson(JsonWriter writer, T value, JsonSerializer serializer)
		{
			// Serialize { "success": true, "message": "Success!" }

			writer.WriteStartObject();

			writer.WritePropertyName("success");
			writer.WriteValue(value.Success);

			if (!string.IsNullOrEmpty(value.Message))
			{
				writer.WritePropertyName("message");
				writer.WriteValue(value.Message);
			}

			WriteResponseJson(writer, value, serializer);

			writer.WriteEndObject();
		}

		protected virtual void WriteResponseJson(JsonWriter writer, T value, JsonSerializer serializer) { }

		public sealed override T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			bool success = false;
			string? message = string.Empty;
			T existingData = default;

			while (reader.TokenType != JsonToken.EndObject)
			{
				// Read the property name.
				string propertyName = (string) reader.Value!;

				if (propertyName.Equals("success", StringComparison.OrdinalIgnoreCase))
				{
					success = GameJoltBooleanConverter.Instance.ReadJson(reader, typeof(bool), false, false, serializer);
				}
				else if (propertyName.Equals("message", StringComparison.OrdinalIgnoreCase))
				{
					message = reader.ReadAsString();
				}
				else
				{
					existingData = ReadResponseJson(reader, serializer);

					// If the token type is end object, we're done.
					if (reader.TokenType == JsonToken.EndObject)
					{
						break;
					}
				}

				// Read the next property name.
				reader.Read();
			}

			return CreateResponse(success, message, existingData);
		}

		protected virtual T ReadResponseJson(JsonReader reader, JsonSerializer serializer)
		{
			return default;
		}

		protected abstract T CreateResponse(bool success, string? message, T existingData);
	}
}
#endif