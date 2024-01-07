#if !NET6_0_OR_GREATER
#nullable enable

using System;
using Newtonsoft.Json;

namespace Hertzole.GameJolt
{
	internal sealed class GetKeysResponseConverter : ResponseConverter<GetKeysResponse>
	{
		protected override void WriteResponseJson(JsonWriter writer, GetKeysResponse value, JsonSerializer serializer)
		{
			writer.WritePropertyName("keys");
			serializer.Serialize(writer, value.keys);
		}

		protected override GetKeysResponse ReadResponseJson(JsonReader reader, JsonSerializer serializer)
		{
			DataKey[] keys = Array.Empty<DataKey>();
            
			while (reader.TokenType != JsonToken.EndObject)
			{
				// Read the property name.
				string propertyName = (string) reader.Value!;

				if (propertyName.Equals("keys", StringComparison.OrdinalIgnoreCase))
				{
					reader.Read();
					
					keys = serializer.Deserialize<DataKey[]>(reader) ?? Array.Empty<DataKey>();
					break;
				}

				// Read the next property name.
				reader.Read();
			}

			return new GetKeysResponse(false, null, keys);
		}

		protected override GetKeysResponse CreateResponse(bool success, string? message, GetKeysResponse existingData)
		{
			return new GetKeysResponse(success, message, existingData.keys);
		}
	}
}
#endif