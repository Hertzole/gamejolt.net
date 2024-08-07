﻿#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

#if !NET6_0_OR_GREATER && !FORCE_SYSTEM_JSON
#nullable enable

using System;
using Newtonsoft.Json;

namespace Hertzole.GameJolt.Serialization.Newtonsoft
{
	internal sealed class FetchFriendsResponseConverter : ResponseConverter<FetchFriendsResponse>
	{
		protected override void WriteResponseJson(JsonWriter writer, FetchFriendsResponse value, JsonSerializer serializer)
		{
			writer.WritePropertyName("friends");
			serializer.Serialize(writer, value.friends);
		}

		protected override FetchFriendsResponse ReadResponseJson(JsonReader reader, JsonSerializer serializer)
		{
			FriendId[]? friends = null;

			while (reader.TokenType != JsonToken.EndObject)
			{
				// Skip unknown types.
				if (reader.TokenType != JsonToken.PropertyName)
				{
					reader.Skip();
					reader.Read();
					continue;
				}

				// Read the property name.
				string propertyName = (string) reader.Value!;

				if (propertyName.Equals("friends", StringComparison.OrdinalIgnoreCase))
				{
					reader.Read();

					if (reader.TokenType == JsonToken.Null)
					{
						friends = Array.Empty<FriendId>();
					}
					else
					{
						friends = serializer.Deserialize<FriendId[]>(reader);
					}

					break;
				}

				// Read the next property name.
				reader.Read();
			}

			return new FetchFriendsResponse(false, null, friends);
		}

		protected override FetchFriendsResponse CreateResponse(bool success, string? message, FetchFriendsResponse existingData)
		{
			return new FetchFriendsResponse(success, message, existingData.friends);
		}
	}
}
#endif
#endif // DISABLE_GAMEJOLT