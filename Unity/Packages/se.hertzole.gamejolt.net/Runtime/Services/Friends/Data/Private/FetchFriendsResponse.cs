#nullable enable

using System;
#if NET6_0_OR_GREATER
using JsonProperty = System.Text.Json.Serialization.JsonPropertyNameAttribute;
using System.Text.Json.Serialization;
using Hertzole.GameJolt.Serialization.System;
#else
using Newtonsoft.Json;
using Hertzole.GameJolt.Serialization.Newtonsoft;
#endif

namespace Hertzole.GameJolt
{
	internal readonly struct FetchFriendsResponse : IResponse, IEquatable<FetchFriendsResponse>
	{
		[JsonProperty("friends")]
		public readonly FriendId[] friends;
		[JsonProperty("success")]
		[JsonConverter(typeof(GameJoltBooleanConverter))]
		public bool Success { get; }
		[JsonProperty("message")]
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