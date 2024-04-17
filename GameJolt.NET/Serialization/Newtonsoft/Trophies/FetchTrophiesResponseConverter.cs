#if !NET6_0_OR_GREATER
#nullable enable

using System;
using Newtonsoft.Json;

namespace Hertzole.GameJolt.Serialization.Newtonsoft
{
	internal sealed class FetchTrophiesResponseConverter : ResponseConverter<FetchTrophiesResponse>
	{
		protected override void WriteResponseJson(JsonWriter writer, FetchTrophiesResponse value, JsonSerializer serializer)
		{
			writer.WritePropertyName("trophies");
			serializer.Serialize(writer, value.trophies);
		}

		protected override FetchTrophiesResponse ReadResponseJson(JsonReader reader, JsonSerializer serializer)
		{
			TrophyInternal[] trophies = Array.Empty<TrophyInternal>();

			while (reader.TokenType != JsonToken.EndObject)
			{
				// Read the property name.
				string propertyName = (string) reader.Value!;

				if (propertyName.Equals("trophies", StringComparison.OrdinalIgnoreCase))
				{
					reader.Read();
					trophies = serializer.Deserialize<TrophyInternal[]>(reader)!;
					break;
				}

				// Read the next property name.
				reader.Read();
			}

			return new FetchTrophiesResponse(true, string.Empty, trophies);
		}

		protected override FetchTrophiesResponse CreateResponse(bool success, string? message, FetchTrophiesResponse existingData)
		{
			return new FetchTrophiesResponse(success, message, existingData.trophies);
		}
	}
}
#endif