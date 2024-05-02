#nullable enable

using System;
#if NET6_0_OR_GREATER
using JsonName = System.Text.Json.Serialization.JsonPropertyNameAttribute;
using JsonConverter = System.Text.Json.Serialization.JsonConverterAttribute;
using JsonConstructor = System.Text.Json.Serialization.JsonConstructorAttribute;
using JsonIgnore = System.Text.Json.Serialization.JsonIgnoreAttribute;
using Hertzole.GameJolt.Serialization.System;
#else
using JsonName = Newtonsoft.Json.JsonPropertyAttribute;
using JsonConverter = Newtonsoft.Json.JsonConverterAttribute;
using JsonConstructor = Newtonsoft.Json.JsonConstructorAttribute;
using JsonIgnore = Newtonsoft.Json.JsonIgnoreAttribute;
using Hertzole.GameJolt.Serialization.Newtonsoft;
#endif

namespace Hertzole.GameJolt
{
	internal readonly struct GetScoreRankResponse : IResponse, IEquatable<GetScoreRankResponse>
	{
		[JsonName("success")]
		[JsonConverter(typeof(GameJoltBooleanConverter))]
		public readonly bool success;
		[JsonName("message")]
		public readonly string? message;
		[JsonName("rank")]
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