#if UNITY_2021_1_OR_NEWER || !NET6_0_OR_GREATER
#nullable enable
using System;
using Newtonsoft.Json;

namespace Hertzole.GameJolt
{
	internal sealed class GameJoltUserConverter : JsonConverter<GameJoltUser>
	{
		public override void WriteJson(JsonWriter writer, GameJoltUser value, JsonSerializer serializer)
		{
			throw new NotSupportedException();
		}

		public override GameJoltUser ReadJson(JsonReader reader, Type objectType, GameJoltUser existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			// First, read the start object token.
			reader.Read();

			int id = 0;
			UserType type = UserType.User;
			string username = string.Empty;
			string avatarUrl = string.Empty;
			DateTime signedUp = DateTime.MinValue;
			DateTime lastLoggedIn = DateTime.MinValue;
			bool onlineNow = false;
			UserStatus status = UserStatus.Active;
			string displayName = string.Empty;
			string? userWebsite = null;
			string userDescription = string.Empty;

			while (reader.TokenType != JsonToken.EndObject)
			{
				// Read the property name.
				string propertyName = (string) reader.Value!;

				switch (propertyName)
				{
					case "id":
						id = reader.ReadAsInt32() ?? 0;
						break;
					case "type":
						type = GetUserType(reader.ReadAsString()!);
						break;
					case "username":
						username = reader.ReadAsString()!;
						break;
					case "avatar_url":
						avatarUrl = reader.ReadAsString()!;
						break;
					case "signed_up_timestamp":
						signedUp = DateTimeHelper.FromUnixTimestamp(reader.ReadAsInt32()!.Value);
						break;
					case "last_logged_in":
						string lastLoggedInString = reader.ReadAsString()!;
						if (lastLoggedInString.Equals("online now", StringComparison.OrdinalIgnoreCase))
						{
							onlineNow = true;
						}

						break;
					case "last_logged_in_timestamp":
						lastLoggedIn = DateTimeHelper.FromUnixTimestamp(reader.ReadAsInt32()!.Value);
						break;
					case "status":
						status = GetUserStatus(reader.ReadAsString()!);
						break;
					case "developer_name":
						displayName = reader.ReadAsString()!;
						break;
					case "developer_website":
						userWebsite = reader.ReadAsString();
						break;
					case "developer_description":
						userDescription = reader.ReadAsString()!;
						break;
					case "signed_up": // Just ignore this as we only use the timestamps.
						reader.Read();
						break;
					default:
						throw new JsonSerializationException($"Unknown property: {propertyName}");
				}

				// Read the next property name.
				reader.Read();
			}

			// return new GameJoltUser(id, type, username, avatarUrl, signedUp, lastLoggedIn, onlineNow, status, displayName, userWebsite, userDescription);
			return default;
		}

		private static UserType GetUserType(string type)
		{
			if (type.Equals("user", StringComparison.OrdinalIgnoreCase))
			{
				return UserType.User;
			}

			if (type.Equals("developer", StringComparison.OrdinalIgnoreCase))
			{
				return UserType.Developer;
			}

			if (type.Equals("moderator", StringComparison.OrdinalIgnoreCase))
			{
				return UserType.Moderator;
			}

			if (type.Equals("administrator", StringComparison.OrdinalIgnoreCase))
			{
				return UserType.Administrator;
			}

			throw new JsonSerializationException($"Unknown user type: {type}");
		}

		private static UserStatus GetUserStatus(string status)
		{
			if (status.Equals("active", StringComparison.OrdinalIgnoreCase))
			{
				return UserStatus.Active;
			}

			if (status.Equals("banned", StringComparison.OrdinalIgnoreCase))
			{
				return UserStatus.Banned;
			}

			throw new JsonSerializationException($"Unknown user status: {status}");
		}
	}
}
#endif