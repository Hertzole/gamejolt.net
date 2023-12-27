#nullable enable

#if NET6_0_OR_GREATER
using JsonName = System.Text.Json.Serialization.JsonPropertyNameAttribute;
using JsonConverter = System.Text.Json.Serialization.JsonConverterAttribute;
using JsonConstructor = System.Text.Json.Serialization.JsonConstructorAttribute;
#endif

namespace Hertzole.GameJolt
{
	internal readonly struct FetchFriendsResponse : IResponse
	{
		[JsonName("friends")]
		public readonly FriendId[] friends;
		[JsonName("success")]
		[JsonConverter(typeof(GameJoltBooleanConverter))]
		public bool Success { get; }
		[JsonName("message")]
		public string? Message { get; }

		[JsonConstructor]
		public FetchFriendsResponse(bool success, string? message, FriendId[] friends)
		{
			this.friends = friends;
			Success = success;
			Message = message;
		}
	}
}