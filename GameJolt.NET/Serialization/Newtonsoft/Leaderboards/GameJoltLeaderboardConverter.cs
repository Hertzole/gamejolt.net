#if UNITY_2021_1_OR_NEWER || !NET6_0_OR_GREATER
using System;
using Newtonsoft.Json;

namespace Hertzole.GameJolt
{
	internal sealed class GameJoltLeaderboardConverter : JsonConverter<GameJoltLeaderboard>
	{
		public override void WriteJson(JsonWriter writer, GameJoltLeaderboard value, JsonSerializer serializer)
		{
			throw new NotSupportedException();
		}

		public override GameJoltLeaderboard ReadJson(JsonReader reader,
			Type objectType,
			GameJoltLeaderboard existingValue,
			bool hasExistingValue,
			JsonSerializer serializer)
		{
			// First, read the start object token.
			reader.Read();

			int id = -1;
			string name = string.Empty;
			string description = string.Empty;
			bool isPrimary = false;

			while (reader.TokenType != JsonToken.EndObject)
			{
				// Read the property name.
				string propertyName = (string) reader.Value!;

				if (propertyName.Equals("id", StringComparison.OrdinalIgnoreCase))
				{
					id = reader.ReadAsInt32() ?? 0;
				}
				else if (propertyName.Equals("name", StringComparison.OrdinalIgnoreCase))
				{
					name = reader.ReadAsString()!;
				}
				else if (propertyName.Equals("description", StringComparison.OrdinalIgnoreCase))
				{
					description = reader.ReadAsString()!;
				}
				else if (propertyName.Equals("primary", StringComparison.OrdinalIgnoreCase))
				{
					isPrimary = reader.ReadAsBooleanWithGameJolt();
				}
				else
				{
					throw new JsonSerializationException($"Unknown property: {propertyName}");
				}

				// Read the next property name.
				reader.Read();
			}

			return new GameJoltLeaderboard(id, name, description, isPrimary);
		}
	}
}
#endif