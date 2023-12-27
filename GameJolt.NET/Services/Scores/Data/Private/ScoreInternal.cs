#if NET6_0_OR_GREATER
using JsonName = System.Text.Json.Serialization.JsonPropertyNameAttribute;
using JsonConstructor = System.Text.Json.Serialization.JsonConstructorAttribute;
using JsonConverter = System.Text.Json.Serialization.JsonConverterAttribute;
#endif

namespace Hertzole.GameJolt
{
	internal readonly struct ScoreInternal
	{
		[JsonName("sort")]
		[JsonConverter(typeof(GameJoltIntConverter))]
		public readonly int sort;
		[JsonName("score")]
		public readonly string score;
		[JsonName("extra_data")]
		public readonly string extraData;
		[JsonName("user")]
		public readonly string username;
		[JsonName("user_id")]
		[JsonConverter(typeof(GameJoltIntConverter))]
		public readonly int userId;
		[JsonName("guest")]
		public readonly string guestName;
		[JsonName("stored_timestamp")]
		[JsonConverter(typeof(GameJoltLongConverter))]
		public readonly long storedTimestamp;
		[JsonName("stored")]
		public readonly string stored;

		[JsonConstructor]
		public ScoreInternal(int sort, string score, string extraData, string username, int userId, string guestName, string stored, long storedTimestamp)
		{
			this.sort = sort;
			this.score = score;
			this.extraData = extraData;
			this.username = username;
			this.userId = userId;
			this.guestName = guestName;
			this.stored = stored;
			this.storedTimestamp = storedTimestamp;
		}

		public GameJoltScore ToPublicScore()
		{
			return new GameJoltScore(sort, score, extraData, username, userId, guestName, DateTimeHelper.FromUnixTimestamp(storedTimestamp));
		}
	}
}