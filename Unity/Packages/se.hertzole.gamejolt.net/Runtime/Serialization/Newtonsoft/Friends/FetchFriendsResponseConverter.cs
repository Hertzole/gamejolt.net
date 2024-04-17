#if !NET6_0_OR_GREATER
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
			FriendId[] friends = Array.Empty<FriendId>();

			while (reader.TokenType != JsonToken.EndObject)
			{
				// Read the property name.
				string propertyName = (string) reader.Value!;

				if (propertyName.Equals("friends", StringComparison.OrdinalIgnoreCase))
				{
					reader.Read();
					friends = serializer.Deserialize<FriendId[]>(reader) ?? Array.Empty<FriendId>();
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