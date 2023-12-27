#if UNITY_2021_1_OR_NEWER || !NET6_0_OR_GREATER
using System;
using Newtonsoft.Json;

namespace Hertzole.GameJolt
{
	internal sealed class GetLeaderboardsResponseConverter : ResponseConverter<GetLeaderboardsResponse>
	{
		protected override GetLeaderboardsResponse ReadResponse(JsonReader reader, JsonSerializer serializer)
		{
			bool success = false;
			string message = string.Empty;
			GameJoltLeaderboard[] leaderboards = Array.Empty<GameJoltLeaderboard>();

			while (reader.TokenType != JsonToken.EndObject)
			{
				// Read the property name.
				string propertyName = (string) reader.Value!;

				switch (propertyName)
				{
					case "success":
						bool? b = reader.ReadAsBoolean();
						if (b == null)
						{
							throw new JsonSerializationException("Expected boolean.");
						}

						success = b.Value;
						break;
					case "message":
						message = reader.ReadAsString();
						break;
					case "tables":
						reader.Read();
						leaderboards = serializer.Deserialize<GameJoltLeaderboard[]>(reader)! ?? Array.Empty<GameJoltLeaderboard>();
						break;
					default:
						throw new JsonSerializationException($"Unknown property: {propertyName}");
				}

				// Read the next property name.
				reader.Read();
			}

			return new GetLeaderboardsResponse(success, message, leaderboards);
		}
	}
}
#endif