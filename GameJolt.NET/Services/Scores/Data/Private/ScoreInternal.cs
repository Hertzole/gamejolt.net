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
	internal readonly struct ScoreInternal : IEquatable<ScoreInternal>
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

		public bool Equals(ScoreInternal other)
		{
			return sort == other.sort && userId == other.userId && storedTimestamp == other.storedTimestamp &&
			       EqualityHelper.StringEquals(score, other.score) &&
			       EqualityHelper.StringEquals(extraData, other.extraData) &&
			       EqualityHelper.StringEquals(username, other.username) &&
			       EqualityHelper.StringEquals(guestName, other.guestName) &&
			       EqualityHelper.StringEquals(stored, other.stored);
		}

		public override bool Equals(object obj)
		{
			return obj is ScoreInternal other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = sort;
				hashCode = (hashCode * 397) ^ userId;
				hashCode = (hashCode * 397) ^ storedTimestamp.GetHashCode();
				hashCode = (hashCode * 397) ^ (score != null ? score.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (extraData != null ? extraData.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (username != null ? username.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (guestName != null ? guestName.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (stored != null ? stored.GetHashCode() : 0);
				return hashCode;
			}
		}

		public static bool operator ==(ScoreInternal left, ScoreInternal right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(ScoreInternal left, ScoreInternal right)
		{
			return !left.Equals(right);
		}

		public GameJoltScore ToPublicScore()
		{
			return new GameJoltScore(sort, score, extraData, username, userId, guestName, DateTimeHelper.FromUnixTimestamp(storedTimestamp));
		}
		
		public override string ToString()
		{
			return $"{nameof(ScoreInternal)} ({nameof(sort)}: {sort}, {nameof(score)}: {score}, {nameof(extraData)}: {extraData}, {nameof(username)}: {username}, {nameof(userId)}: {userId}, {nameof(guestName)}: {guestName}, {nameof(storedTimestamp)}: {storedTimestamp}, {nameof(stored)}: {stored})";
		}
	}
}