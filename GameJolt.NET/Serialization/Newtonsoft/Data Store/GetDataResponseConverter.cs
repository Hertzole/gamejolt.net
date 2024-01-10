#if !NET6_0_OR_GREATER
#nullable enable

using System;
using Newtonsoft.Json;

namespace Hertzole.GameJolt
{
	internal sealed class GetDataResponseConverter : ResponseConverter<GetDataResponse>
	{
		protected override void WriteResponseJson(JsonWriter writer, GetDataResponse value, JsonSerializer serializer)
		{
			writer.WritePropertyName("data");
			writer.WriteValue(value.data);
		}

		protected override GetDataResponse ReadResponseJson(JsonReader reader, JsonSerializer serializer)
		{
			string data = string.Empty;

			while (reader.TokenType != JsonToken.EndObject)
			{
				// Read the property name.
				string propertyName = (string) reader.Value!;

				if (propertyName.Equals("data", StringComparison.OrdinalIgnoreCase))
				{
					data = reader.ReadAsString() ?? string.Empty;
					break;
				}

				// Read the next property name.
				reader.Read();
			}

			return new GetDataResponse(false, null, data);
		}

		protected override GetDataResponse CreateResponse(bool success, string? message, GetDataResponse existingData)
		{
			return new GetDataResponse(success, message, existingData.data);
		}
	}
}
#endif