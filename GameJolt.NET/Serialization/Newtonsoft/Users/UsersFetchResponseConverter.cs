#if UNITY_2021_1_OR_NEWER || !NET6_0_OR_GREATER
using System;
using Newtonsoft.Json;

namespace Hertzole.GameJolt
{
	internal sealed class UsersFetchResponseConverter : ResponseConverter<UsersFetchResponse>
	{
		protected override UsersFetchResponse ReadResponse(JsonReader reader, JsonSerializer serializer)
		{
			bool success = false;
			string message = string.Empty;
			GameJoltUser[] users = Array.Empty<GameJoltUser>();

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
					case "users":
						reader.Read();
						users = serializer.Deserialize<GameJoltUser[]>(reader)! ?? Array.Empty<GameJoltUser>();
						break;
					default:
						throw new JsonSerializationException($"Unknown property: {propertyName}");
				}

				// Read the next property name.
				reader.Read();
			}

			return new UsersFetchResponse(success, message, users);
		}
	}
}
#endif