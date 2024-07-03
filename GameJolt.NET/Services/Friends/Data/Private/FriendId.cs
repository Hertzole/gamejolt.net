using System;
#if NET6_0_OR_GREATER || FORCE_SYSTEM_JSON
using JsonProperty = System.Text.Json.Serialization.JsonPropertyNameAttribute;
using System.Text.Json.Serialization;
using Hertzole.GameJolt.Serialization.System;
#else
using Newtonsoft.Json;
using Hertzole.GameJolt.Serialization.Newtonsoft;
#endif

namespace Hertzole.GameJolt
{
	internal readonly struct FriendId : IEquatable<FriendId>
	{
		[JsonProperty("friend_id")]
		[JsonConverter(typeof(GameJoltIntConverter))]
		public readonly int id;

		[JsonConstructor]
		public FriendId(int id)
		{
			this.id = id;
		}

		public bool Equals(FriendId other)
		{
			return id == other.id;
		}

		public override bool Equals(object obj)
		{
			return obj is FriendId other && Equals(other);
		}

		public override int GetHashCode()
		{
			return id;
		}

		public static bool operator ==(FriendId left, FriendId right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(FriendId left, FriendId right)
		{
			return !left.Equals(right);
		}

		public override string ToString()
		{
			return $"{nameof(FriendId)} ({nameof(id)}: {id})";
		}
	}
}