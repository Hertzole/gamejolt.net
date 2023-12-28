#if !NET6_0_OR_GREATER
using System;
using Newtonsoft.Json;

namespace Hertzole.GameJolt
{
	internal sealed class FetchTimeResponseConverter : ResponseConverter<FetchTimeResponse>
	{
		protected override void WriteResponseJson(JsonWriter writer, FetchTimeResponse value, JsonSerializer serializer)
		{
			writer.WritePropertyName("timestamp");
			GameJoltLongConverter.Instance.WriteJson(writer, value.timestamp, serializer);
			writer.WritePropertyName("timezone");
			writer.WriteValue(value.timezone);
			writer.WritePropertyName("year");
			GameJoltIntConverter.Instance.WriteJson(writer, value.year, serializer);
			writer.WritePropertyName("month");
			GameJoltIntConverter.Instance.WriteJson(writer, value.month, serializer);
			writer.WritePropertyName("day");
			GameJoltIntConverter.Instance.WriteJson(writer, value.day, serializer);
			writer.WritePropertyName("hour");
			GameJoltIntConverter.Instance.WriteJson(writer, value.hour, serializer);
			writer.WritePropertyName("minute");
			GameJoltIntConverter.Instance.WriteJson(writer, value.minute, serializer);
			writer.WritePropertyName("second");
			GameJoltIntConverter.Instance.WriteJson(writer, value.second, serializer);
		}

		protected override FetchTimeResponse ReadResponseJson(JsonReader reader, JsonSerializer serializer)
		{
			long timestamp = 0;
			string timezone = string.Empty;
			int year = 0;
			int month = 0;
			int day = 0;
			int hour = 0;
			int minute = 0;
			int second = 0;

			while (reader.TokenType != JsonToken.EndObject)
			{
				// Read the property name.
				string propertyName = (string) reader.Value!;

				if (propertyName.Equals("timestamp", StringComparison.OrdinalIgnoreCase))
				{
					timestamp = GameJoltLongConverter.Instance.ReadJson(reader, typeof(long), 0, false, serializer)!;
				}
				else if (propertyName.Equals("timezone", StringComparison.OrdinalIgnoreCase))
				{
					timezone = reader.ReadAsString() ?? string.Empty;
				}
				else if (propertyName.Equals("year", StringComparison.OrdinalIgnoreCase))
				{
					year = GameJoltIntConverter.Instance.ReadJson(reader, typeof(int), 0, false, serializer)!;
				}
				else if (propertyName.Equals("month", StringComparison.OrdinalIgnoreCase))
				{
					month = GameJoltIntConverter.Instance.ReadJson(reader, typeof(int), 0, false, serializer)!;
				}
				else if (propertyName.Equals("day", StringComparison.OrdinalIgnoreCase))
				{
					day = GameJoltIntConverter.Instance.ReadJson(reader, typeof(int), 0, false, serializer)!;
				}
				else if (propertyName.Equals("hour", StringComparison.OrdinalIgnoreCase))
				{
					hour = GameJoltIntConverter.Instance.ReadJson(reader, typeof(int), 0, false, serializer)!;
				}
				else if (propertyName.Equals("minute", StringComparison.OrdinalIgnoreCase))
				{
					minute = GameJoltIntConverter.Instance.ReadJson(reader, typeof(int), 0, false, serializer)!;
				}
				else if (propertyName.Equals("second", StringComparison.OrdinalIgnoreCase))
				{
					second = GameJoltIntConverter.Instance.ReadJson(reader, typeof(int), 0, false, serializer)!;
				}
				else
				{
					reader.Skip();
				}

				// Read the next property name.
				reader.Read();
			}

			return new FetchTimeResponse(timestamp, timezone, year, month, day, hour, minute, second, false, null);
		}

		protected override FetchTimeResponse CreateResponse(bool success, string? message, FetchTimeResponse existingData)
		{
			return new FetchTimeResponse(existingData.timestamp, existingData.timezone, existingData.year, existingData.month, existingData.day,
				existingData.hour, existingData.minute, existingData.second, success, message);
		}
	}
}
#endif