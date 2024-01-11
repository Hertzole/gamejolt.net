#if !NET6_0_OR_GREATER
#nullable enable

using System;
using Newtonsoft.Json;

namespace Hertzole.GameJolt
{
	internal sealed class UsersFetchResponseConverter : ResponseConverter<UsersFetchResponse>
	{
		protected override void WriteResponseJson(JsonWriter writer, UsersFetchResponse value, JsonSerializer serializer)
		{
			writer.WritePropertyName("users");
			serializer.Serialize(writer, value.Users, typeof(User[]));
		}

		protected override UsersFetchResponse ReadResponseJson(JsonReader reader, JsonSerializer serializer)
		{
			User[] users = Array.Empty<User>();

			while (reader.TokenType != JsonToken.EndObject)
			{
				// Read the property name.
				string propertyName = (string) reader.Value!;

				if (propertyName.Equals("users", StringComparison.OrdinalIgnoreCase))
				{
					reader.Read();
					users = serializer.Deserialize<User[]>(reader)! ?? Array.Empty<User>();
					break;
				}

				// Read the next property name.
				reader.Read();
			}

			return new UsersFetchResponse(true, string.Empty, users);
		}

		protected override UsersFetchResponse CreateResponse(bool success, string? message, UsersFetchResponse existingData)
		{
			return new UsersFetchResponse(success, message, existingData.Users);
		}
	}
}
#endif