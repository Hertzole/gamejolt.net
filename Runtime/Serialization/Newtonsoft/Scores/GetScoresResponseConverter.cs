#if !NET6_0_OR_GREATER
#nullable enable

using System;
using Newtonsoft.Json;

namespace Hertzole.GameJolt
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
			ScoreInternal[] scores = Array.Empty<ScoreInternal>();

			while (reader.TokenType != JsonToken.EndObject)
			{
				// Read the property name.
				string propertyName = (string) reader.Value!;

				if (propertyName.Equals("scores", StringComparison.OrdinalIgnoreCase))
				{
					reader.Read();
					scores = serializer.Deserialize<ScoreInternal[]>(reader)! ?? Array.Empty<ScoreInternal>();
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