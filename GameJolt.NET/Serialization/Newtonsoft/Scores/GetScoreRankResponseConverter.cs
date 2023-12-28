﻿#if !NET6_0_OR_GREATER
using System;
using Newtonsoft.Json;

namespace Hertzole.GameJolt
{
	internal sealed class GetScoreRankResponseConverter : ResponseConverter<GetScoreRankResponse>
	{
		protected override void WriteResponseJson(JsonWriter writer, GetScoreRankResponse value, JsonSerializer serializer)
		{
			writer.WritePropertyName("rank");
			writer.WriteValue(value.rank);
		}

		protected override GetScoreRankResponse ReadResponseJson(JsonReader reader, JsonSerializer serializer)
		{
			int rank = -1;

			while (reader.TokenType != JsonToken.EndObject)
			{
				// Read the property name.
				string propertyName = (string) reader.Value!;

				if (propertyName.Equals("rank", StringComparison.OrdinalIgnoreCase))
				{
					rank = GameJoltIntConverter.Instance.ReadJson(reader, typeof(int), 0, false, serializer);
					break;
				}

				// Read the next property name.
				reader.Read();
			}

			return new GetScoreRankResponse(true, string.Empty, rank);
		}

		protected override GetScoreRankResponse CreateResponse(bool success, string? message, GetScoreRankResponse existingData)
		{
			return new GetScoreRankResponse(success, message, existingData.rank);
		}
	}
}
#endif