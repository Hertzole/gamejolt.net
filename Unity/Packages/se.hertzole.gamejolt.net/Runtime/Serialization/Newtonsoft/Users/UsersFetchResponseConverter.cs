#if !NET6_0_OR_GREATER
#nullable enable

using System;
using Newtonsoft.Json;

namespace Hertzole.GameJolt.Serialization.Newtonsoft
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
			User[]? users = null;

			while (reader.TokenType != JsonToken.EndObject)
			{
				if (reader.TokenType != JsonToken.PropertyName)
				{
					reader.Skip();
					reader.Read();
					continue;
				}

				// Read the property name.
				string propertyName = (string) reader.Value!;

				if (propertyName.Equals("users", StringComparison.OrdinalIgnoreCase))
				{
					reader.Read();
					if (reader.TokenType == JsonToken.Null)
					{
						users = Array.Empty<User>();
					}
					else
					{
						users = serializer.Deserialize<User[]>(reader);
					}

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