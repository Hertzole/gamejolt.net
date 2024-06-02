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
	internal readonly struct ScoreInternal : IEquatable<ScoreInternal>
	{
		[JsonProperty("sort")]
		[JsonConverter(typeof(GameJoltIntConverter))]
		public readonly int sort;
		[JsonProperty("score")]
		public readonly string score;
		[JsonProperty("extra_data")]
		public readonly string extraData;
		[JsonProperty("user")]
		public readonly string username;
		[JsonProperty("user_id")]
		[JsonConverter(typeof(GameJoltIntConverter))]
		public readonly int userId;
		[JsonProperty("guest")]
		public readonly string guestName;
		[JsonProperty("stored_timestamp")]
		[JsonConverter(typeof(GameJoltLongConverter))]
		public readonly long storedTimestamp;
		[JsonProperty("stored")]
		public readonly string stored;

		[JsonConstructor]
		public ScoreInternal(int sort, string? score, string? extraData, string? username, int userId, string? guestName, string? stored, long storedTimestamp)
		{
			this.sort = sort;
			this.score = score ?? string.Empty;
			this.extraData = extraData ?? string.Empty;
			this.username = username ?? string.Empty;
			this.userId = userId;
			this.guestName = guestName ?? string.Empty;
			this.stored = stored ?? string.Empty;
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

		public override bool Equals(object? obj)
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
				hashCode = (hashCode * 397) ^ (!string.IsNullOrEmpty(score) ? score.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (!string.IsNullOrEmpty(extraData) ? extraData.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (!string.IsNullOrEmpty(username) ? username.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (!string.IsNullOrEmpty(guestName) ? guestName.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (!string.IsNullOrEmpty(stored) ? stored.GetHashCode() : 0);
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
			return new GameJoltScore(sort, score, extraData, username, userId < 0 ? null : userId, guestName, DateTimeHelper.FromUnixTimestamp(storedTimestamp));
		}

		public override string ToString()
		{
			return
				$"{nameof(ScoreInternal)} ({nameof(sort)}: {sort}, {nameof(score)}: {score}, {nameof(extraData)}: {extraData}, {nameof(username)}: {username}, {nameof(userId)}: {userId}, {nameof(guestName)}: {guestName}, {nameof(storedTimestamp)}: {storedTimestamp}, {nameof(stored)}: {stored})";
		}
	}
}