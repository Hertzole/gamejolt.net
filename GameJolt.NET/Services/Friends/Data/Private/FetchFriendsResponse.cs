#nullable enable

using System;
#if NET6_0_OR_GREATER
using JsonName = System.Text.Json.Serialization.JsonPropertyNameAttribute;
using JsonConverter = System.Text.Json.Serialization.JsonConverterAttribute;
using JsonConstructor = System.Text.Json.Serialization.JsonConstructorAttribute;
using Hertzole.GameJolt.Serialization.System;
#else
using JsonName = Newtonsoft.Json.JsonPropertyAttribute;
using JsonConverter = Newtonsoft.Json.JsonConverterAttribute;
using JsonConstructor = Newtonsoft.Json.JsonConstructorAttribute;
using Hertzole.GameJolt.Serialization.Newtonsoft;
#endif

namespace Hertzole.GameJolt
{
	internal readonly struct FetchFriendsResponse : IResponse, IEquatable<FetchFriendsResponse>
	{
		[JsonName("friends")]
		public readonly FriendId[] friends;
		[JsonName("success")]
		[JsonConverter(typeof(GameJoltBooleanConverter))]
		public bool Success { get; }
		[JsonName("message")]
		public string? Message { get; }

		[JsonConstructor]
		public FetchFriendsResponse(bool success, string? message, FriendId[]? friends)
		{
			this.friends = friends ?? Array.Empty<FriendId>();
			Success = success;
			Message = message;
		}

		public bool Equals(FetchFriendsResponse other)
		{
			return EqualityHelper.ResponseEquals(this, other) && EqualityHelper.ArrayEquals(friends, other.friends);
		}

		public override bool Equals(object? obj)
		{
			return obj is FetchFriendsResponse other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = EqualityHelper.ResponseHashCode(0, this);
				hashCode = (hashCode * 397) ^ friends.GetHashCode();
				return hashCode;
			}
		}

		public static bool operator ==(FetchFriendsResponse left, FetchFriendsResponse right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(FetchFriendsResponse left, FetchFriendsResponse right)
		{
			return !left.Equals(right);
		}

		public override string ToString()
		{
			return
				$"{nameof(FetchFriendsResponse)} ({nameof(Success)}: {Success}, {nameof(Message)}: {Message}, {nameof(friends)}: {friends.ToCommaSeparatedString()})";
		}
	}
}