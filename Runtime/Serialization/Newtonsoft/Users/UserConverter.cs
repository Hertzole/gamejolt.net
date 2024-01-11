#if !NET6_0_OR_GREATER
#nullable enable
using System;
using Newtonsoft.Json;

namespace Hertzole.GameJolt
{
	internal sealed class UserConverter : JsonConverter<User>
	{
		public override void WriteJson(JsonWriter writer, User value, JsonSerializer serializer)
		{
			writer.WriteStartObject();

			writer.WritePropertyName("id");
			GameJoltIntConverter.Instance.WriteJson(writer, value.id, serializer);
			writer.WritePropertyName("type");
			GameJoltUserTypeConverter.Instance.WriteJson(writer, value.type, serializer);
			writer.WritePropertyName("username");
			serializer.Serialize(writer, value.username);
			writer.WritePropertyName("avatar_url");
			serializer.Serialize(writer, value.avatarUrl);
			writer.WritePropertyName("signed_up");
			serializer.Serialize(writer, value.signedUp);
			writer.WritePropertyName("signed_up_timestamp");
			GameJoltLongConverter.Instance.WriteJson(writer, value.signedUpTimestamp, serializer);
			writer.WritePropertyName("last_logged_in");
			serializer.Serialize(writer, value.lastLoggedIn);
			writer.WritePropertyName("last_logged_in_timestamp");
			GameJoltLongConverter.Instance.WriteJson(writer, value.lastLoggedInTimestamp, serializer);
			writer.WritePropertyName("status");
			GameJoltStatusConverter.Instance.WriteJson(writer, value.status, serializer);
			writer.WritePropertyName("developer_name");
			serializer.Serialize(writer, value.displayName);
			writer.WritePropertyName("developer_website");
			serializer.Serialize(writer, value.userWebsite);
			writer.WritePropertyName("developer_description");
			serializer.Serialize(writer, value.userDescription);

			writer.WriteEndObject();
		}

		public override User ReadJson(JsonReader reader, Type objectType, User existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			// First, read the start object token.
			reader.Read();

			int id = 0;
			UserType type = UserType.User;
			string username = string.Empty;
			string avatarUrl = string.Empty;
			string signedUp = string.Empty;
			long signedUpTimestamp = 0;
			string lastLoggedIn = string.Empty;
			long lastLoggedInTimestamp = 0;
			UserStatus status = UserStatus.Active;
			string displayName = string.Empty;
			string? userWebsite = null;
			string userDescription = string.Empty;

			while (reader.TokenType != JsonToken.EndObject)
			{
				// Read the property name.
				string propertyName = (string) reader.Value!;

				if (propertyName.Equals("id", StringComparison.OrdinalIgnoreCase))
				{
					id = GameJoltIntConverter.Instance.ReadJson(reader, typeof(int), 0, false, serializer);
				}
				else if (propertyName.Equals("type", StringComparison.OrdinalIgnoreCase))
				{
					type = GameJoltUserTypeConverter.Instance.ReadJson(reader, typeof(UserType), UserType.User, false, serializer);
				}
				else if (propertyName.Equals("username", StringComparison.OrdinalIgnoreCase))
				{
					username = reader.ReadAsString() ?? string.Empty;
				}
				else if (propertyName.Equals("avatar_url", StringComparison.OrdinalIgnoreCase))
				{
					avatarUrl = reader.ReadAsString() ?? string.Empty;
				}
				else if (propertyName.Equals("signed_up", StringComparison.OrdinalIgnoreCase))
				{
					signedUp = reader.ReadAsString() ?? string.Empty;
				}
				else if (propertyName.Equals("signed_up_timestamp", StringComparison.OrdinalIgnoreCase))
				{
					signedUpTimestamp = GameJoltLongConverter.Instance.ReadJson(reader, typeof(long), 0, false, serializer);
				}
				else if (propertyName.Equals("last_logged_in", StringComparison.OrdinalIgnoreCase))
				{
					lastLoggedIn = reader.ReadAsString() ?? string.Empty;
				}
				else if (propertyName.Equals("last_logged_in_timestamp", StringComparison.OrdinalIgnoreCase))
				{
					lastLoggedInTimestamp = GameJoltLongConverter.Instance.ReadJson(reader, typeof(long), 0, false, serializer);
				}
				else if (propertyName.Equals("status", StringComparison.OrdinalIgnoreCase))
				{
					status = GameJoltStatusConverter.Instance.ReadJson(reader, typeof(UserStatus), UserStatus.Active, false, serializer);
				}
				else if (propertyName.Equals("developer_name", StringComparison.OrdinalIgnoreCase))
				{
					displayName = serializer.Deserialize<string>(reader)!;
				}
				else if (propertyName.Equals("developer_website", StringComparison.OrdinalIgnoreCase))
				{
					userWebsite = serializer.Deserialize<string>(reader);
				}
				else if (propertyName.Equals("developer_description", StringComparison.OrdinalIgnoreCase))
				{
					userDescription = serializer.Deserialize<string>(reader)!;
				}
				else
				{
					throw new JsonSerializationException($"Unknown property: {propertyName}");
				}

				// Read the next property name.
				reader.Read();
			}

			return new User(id, type, username, avatarUrl, signedUp, signedUpTimestamp, lastLoggedIn, lastLoggedInTimestamp, status, displayName, userWebsite,
				userDescription);
		}
	}
}
#endif