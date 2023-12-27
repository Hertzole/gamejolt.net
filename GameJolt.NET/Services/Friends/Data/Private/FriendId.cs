#if NET6_0_OR_GREATER
using JsonName = System.Text.Json.Serialization.JsonPropertyNameAttribute;
using JsonConverter = System.Text.Json.Serialization.JsonConverterAttribute;
using JsonConstructor = System.Text.Json.Serialization.JsonConstructorAttribute;
#endif

namespace Hertzole.GameJolt
{
	internal readonly struct FriendId
	{
		[JsonName("friend_id")]
		[JsonConverter(typeof(GameJoltIntConverter))]
		public readonly int id;

		[JsonConstructor]
		public FriendId(int id)
		{
			this.id = id;
		}
	}
}