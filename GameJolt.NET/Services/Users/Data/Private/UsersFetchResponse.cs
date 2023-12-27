#nullable enable

#if NET6_0_OR_GREATER
using JsonName = System.Text.Json.Serialization.JsonPropertyNameAttribute;
using JsonConverter = System.Text.Json.Serialization.JsonConverterAttribute;
using JsonConstructor = System.Text.Json.Serialization.JsonConstructorAttribute;
#endif

namespace Hertzole.GameJolt
{
	internal readonly struct UsersFetchResponse : IResponse
	{
		[JsonName("success")]
		[JsonConverter(typeof(GameJoltBooleanConverter))]
		public bool Success { get; }
		[JsonName("message")]
		public string? Message { get; }

		[JsonName("users")]
		public User[] Users { get; }

		[JsonConstructor]
		public UsersFetchResponse(bool success, string? message, User[] users)
		{
			Success = success;
			Message = message;
			Users = users;
		}

		public UsersFetchResponse(bool success, string? message, User user)
		{
			Success = success;
			Message = message;
			Users = new[] { user };
		}
	}
}