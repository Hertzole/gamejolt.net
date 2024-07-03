#if !NET6_0_OR_GREATER && !FORCE_SYSTEM_JSON
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
			TrophyInternal[]? trophies = null;

			while (reader.TokenType != JsonToken.EndObject)
			{
				// Skip unknown types.
				if (reader.TokenType != JsonToken.PropertyName)
				{
					reader.Skip();
					reader.Read();
					continue;
				}

				// Read the property name.
				string propertyName = (string) reader.Value!;

				if (propertyName.Equals("trophies", StringComparison.OrdinalIgnoreCase))
				{
					reader.Read();

					if (reader.TokenType == JsonToken.Null)
					{
						trophies = Array.Empty<TrophyInternal>();
					}
					else
					{
						trophies = serializer.Deserialize<TrophyInternal[]>(reader);
					}

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