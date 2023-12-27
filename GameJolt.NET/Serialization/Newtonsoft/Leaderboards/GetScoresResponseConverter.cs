#if UNITY_2021_1_OR_NEWER || !NET6_0_OR_GREATER
using System;
using Newtonsoft.Json;

namespace Hertzole.GameJolt
{
	internal sealed class GetScoresResponseConverter : ResponseConverter<GetScoresResponse>
	{
		protected override GetScoresResponse ReadResponse(JsonReader reader, JsonSerializer serializer)
		{
			bool success = false;
			string message = string.Empty;
			GameJoltScore[] scores = Array.Empty<GameJoltScore>();

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
					case "scores":
						reader.Read();
						scores = serializer.Deserialize<GameJoltScore[]>(reader)! ?? Array.Empty<GameJoltScore>();
						break;
					default:
						throw new JsonSerializationException($"Unknown property: {propertyName}");
				}

				// Read the next property name.
				reader.Read();
			}

			return new GetScoresResponse(success, message, scores);
		}
	}
}
#endif