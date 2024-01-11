#if !NET6_0_OR_GREATER
#nullable enable

using System;
using Newtonsoft.Json;

namespace Hertzole.GameJolt
{
	internal sealed class UpdateDataResponseConverter : ResponseConverter<UpdateDataResponse>
	{
		protected override void WriteResponseJson(JsonWriter writer, UpdateDataResponse value, JsonSerializer serializer)
		{
			writer.WritePropertyName("data");
			StringOrNumberConverter.Instance.WriteJson(writer, value.data, serializer);
		}

		protected override UpdateDataResponse ReadResponseJson(JsonReader reader, JsonSerializer serializer)
		{
			string data = string.Empty;

			while (reader.TokenType != JsonToken.EndObject)
			{
				// Read the property name.
				string propertyName = (string) reader.Value!;

				if (propertyName.Equals("data", StringComparison.OrdinalIgnoreCase))
				{
					data = StringOrNumberConverter.Instance.ReadJson(reader, typeof(string), string.Empty, false, serializer) ?? string.Empty;
					break;
				}

				// Read the next property name.
				reader.Read();
			}

			return new UpdateDataResponse(false, null, data!);
		}

		protected override UpdateDataResponse CreateResponse(bool success, string? message, UpdateDataResponse existingData)
		{
			return new UpdateDataResponse(success, message, existingData.data);
		}
	}
}
#endif