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
	internal readonly struct GetScoresResponse : IResponse, IEquatable<GetScoresResponse>
	{
		[JsonProperty("success")]
		[JsonConverter(typeof(GameJoltBooleanConverter))]
		public readonly bool success;
		[JsonProperty("message")]
		public readonly string? message;
		[JsonProperty("scores")]
		public readonly ScoreInternal[] scores;

		[JsonIgnore]
		public bool Success
		{
			get { return success; }
		}
		[JsonIgnore]
		public string? Message
		{
			get { return message; }
		}

		[JsonConstructor]
		public GetScoresResponse(bool success, string? message, ScoreInternal[]? scores)
		{
			this.success = success;
			this.message = message;
			this.scores = scores ?? Array.Empty<ScoreInternal>();
		}

		public bool Equals(GetScoresResponse other)
		{
			return EqualityHelper.ResponseEquals(this, other) && EqualityHelper.ArrayEquals(scores, other.scores);
		}

		public override bool Equals(object? obj)
		{
			return obj is GetScoresResponse other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = EqualityHelper.ResponseHashCode(0, this);
				hashCode = (hashCode * 397) ^ scores.GetHashCode();
				return hashCode;
			}
		}

		public static bool operator ==(GetScoresResponse left, GetScoresResponse right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(GetScoresResponse left, GetScoresResponse right)
		{
			return !left.Equals(right);
		}

		public override string ToString()
		{
			return
				$"{nameof(GetScoresResponse)} ({nameof(Success)}: {Success}, {nameof(Message)}: {Message}, {nameof(scores)}: {scores.ToCommaSeparatedString()})";
		}
	}
}