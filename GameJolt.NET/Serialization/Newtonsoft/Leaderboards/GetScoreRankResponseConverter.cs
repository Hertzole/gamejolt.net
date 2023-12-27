#if UNITY_2021_1_OR_NEWER || !NET6_0_OR_GREATER
using System;
using Newtonsoft.Json;

namespace Hertzole.GameJolt
{
	internal sealed class GetScoreRankResponseConverter : ResponseConverter<GetScoreRankResponse>
	{
		protected override GetScoreRankResponse ReadResponse(JsonReader reader, JsonSerializer serializer)
		{
			bool success = false;
			string message = string.Empty;
			int rank = -1;

			while (reader.TokenType != JsonToken.EndObject)
			{
				// Read the property name.
				string propertyName = (string) reader.Value!;

				if (propertyName.Equals("success", StringComparison.OrdinalIgnoreCase))
				{
					bool? b = reader.ReadAsBoolean();
					if (b == null)
					{
						throw new JsonSerializationException("Expected boolean.");
					}

					success = b.Value;
				}
				else if(propertyName.Equals("message", StringComparison.OrdinalIgnoreCase))
				{
					message = reader.ReadAsString();
				}
				else if(propertyName.Equals("rank", StringComparison.OrdinalIgnoreCase))
				{
					rank = reader.ReadAsInt32() ?? 0;
				}
				else
				{
					throw new JsonSerializationException($"Unknown property: {propertyName}");
				}

				// Read the next property name.
				reader.Read();
			}
			
			return new GetScoreRankResponse(success, message, rank);
		}
	}
}
#endif