#if !NET6_0_OR_GREATER && !FORCE_SYSTEM_JSON
#nullable enable

using System;
using Newtonsoft.Json;

namespace Hertzole.GameJolt.Serialization.Newtonsoft
{
	internal sealed class GetScoresResponseConverter : ResponseConverter<GetScoresResponse>
	{
		protected override void WriteResponseJson(JsonWriter writer, GetScoresResponse value, JsonSerializer serializer)
		{
			writer.WritePropertyName("scores");
			serializer.Serialize(writer, value.scores);
		}

		protected override GetScoresResponse ReadResponseJson(JsonReader reader, JsonSerializer serializer)
		{
			ScoreInternal[]? scores = null;

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

				if (propertyName.Equals("scores", StringComparison.OrdinalIgnoreCase))
				{
					reader.Read();

					if (reader.TokenType == JsonToken.Null)
					{
						scores = Array.Empty<ScoreInternal>();
					}
					else
					{
						scores = serializer.Deserialize<ScoreInternal[]>(reader);
					}

					break;
				}

				// Read the next property name.
				reader.Read();
			}

			return new GetScoresResponse(false, null, scores);
		}

		protected override GetScoresResponse CreateResponse(bool success, string? message, GetScoresResponse existingData)
		{
			return new GetScoresResponse(success, message, existingData.scores);
		}
	}
}
#endif