#nullable enable

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
	internal readonly struct GetScoreRankResponse : IResponse, IEquatable<GetScoreRankResponse>
	{
		[JsonProperty("success")]
		[JsonConverter(typeof(GameJoltBooleanConverter))]
		public readonly bool success;
		[JsonProperty("message")]
		public readonly string? message;
		[JsonProperty("rank")]
		[JsonConverter(typeof(GameJoltIntConverter))]
		public readonly int rank;

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
		public GetScoreRankResponse(bool success, string? message, int rank)
		{
			this.success = success;
			this.message = message;
			this.rank = rank;
		}

		public bool Equals(GetScoreRankResponse other)
		{
			return EqualityHelper.ResponseEquals(this, other) && rank == other.rank;
		}

		public override bool Equals(object? obj)
		{
			return obj is GetScoreRankResponse other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = EqualityHelper.ResponseHashCode(0, this);
				hashCode = (hashCode * 397) ^ rank;
				return hashCode;
			}
		}

		public static bool operator ==(GetScoreRankResponse left, GetScoreRankResponse right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(GetScoreRankResponse left, GetScoreRankResponse right)
		{
			return !left.Equals(right);
		}

		public override string ToString()
		{
			return $"{nameof(GetScoreRankResponse)} ({nameof(Success)}: {Success}, {nameof(Message)}: {Message}, {nameof(rank)}: {rank})";
		}
	}
}