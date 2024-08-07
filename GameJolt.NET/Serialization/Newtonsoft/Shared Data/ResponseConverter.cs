﻿#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

#if !NET6_0_OR_GREATER && !FORCE_SYSTEM_JSON
#nullable enable
using System;
using Newtonsoft.Json;

namespace Hertzole.GameJolt.Serialization.Newtonsoft
{
	internal abstract class ResponseConverter<T> : JsonConverter<T> where T : struct, IResponse
	{
		public sealed override bool CanRead
		{
			get { return true; }
		}

		public sealed override bool CanWrite
		{
			get { return true; }
		}

		public sealed override void WriteJson(JsonWriter writer, T value, JsonSerializer serializer)
		{
			// Serialize { "success": true, "message": "Success!" }

			writer.WriteStartObject();

			writer.WritePropertyName("success");
			writer.WriteValue(value.Success);
			writer.WritePropertyName("message");

			if (!string.IsNullOrEmpty(value.Message))
			{
				writer.WriteValue(value.Message);
			}
			else
			{
				writer.WriteNull();
			}

			WriteResponseJson(writer, value, serializer);

			writer.WriteEndObject();
		}

		protected abstract void WriteResponseJson(JsonWriter writer, T value, JsonSerializer serializer);

		public sealed override T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			bool hasReadResponse = false;

			bool success = false;
			string? message = null;
			T existingData = default;

			reader.Read();

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
					reader.Read();
					if (reader.TokenType == JsonToken.Null)
					{
						message = null;
					}
					else
					{
						message = (string) reader.Value!;
					}
				}
				else if (!hasReadResponse)
				{
					existingData = ReadResponseJson(reader, serializer);
					hasReadResponse = true;

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

		protected abstract T ReadResponseJson(JsonReader reader, JsonSerializer serializer);

		protected abstract T CreateResponse(bool success, string? message, T existingData);
	}

	internal sealed class ResponseConverter : ResponseConverter<Response>
	{
		protected override void WriteResponseJson(JsonWriter writer, Response value, JsonSerializer serializer) { }

		protected override Response ReadResponseJson(JsonReader reader, JsonSerializer serializer)
		{
			throw new NotSupportedException(
				"This method should not be called. This probably means that the response has extra data that is not being handled.");
		}

		protected override Response CreateResponse(bool success, string? message, Response existingData)
		{
			return new Response(success, message);
		}
	}
}
#endif
#endif // DISABLE_GAMEJOLT