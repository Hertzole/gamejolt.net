using System;
#if NET6_0_OR_GREATER
using JsonName = System.Text.Json.Serialization.JsonPropertyNameAttribute;
using JsonConverter = System.Text.Json.Serialization.JsonConverterAttribute;
using JsonConstructor = System.Text.Json.Serialization.JsonConstructorAttribute;

#else
using JsonName = Newtonsoft.Json.JsonPropertyAttribute;
using JsonConverter = Newtonsoft.Json.JsonConverterAttribute;
using JsonConstructor = Newtonsoft.Json.JsonConstructorAttribute;
#endif

namespace Hertzole.GameJolt
{
	internal readonly struct FriendId : IEquatable<FriendId>
	{
		[JsonName("friend_id")]
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