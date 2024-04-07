#nullable enable

using System;
#if NET6_0_OR_GREATER
using JsonName = System.Text.Json.Serialization.JsonPropertyNameAttribute;
using JsonConverter = System.Text.Json.Serialization.JsonConverterAttribute;
using JsonConstructor = System.Text.Json.Serialization.JsonConstructorAttribute;
using JsonIgnore = System.Text.Json.Serialization.JsonIgnoreAttribute;
#else
using JsonName = Newtonsoft.Json.JsonPropertyAttribute;
using JsonConverter = Newtonsoft.Json.JsonConverterAttribute;
using JsonConstructor = Newtonsoft.Json.JsonConstructorAttribute;
using JsonIgnore = Newtonsoft.Json.JsonIgnoreAttribute;
#endif

namespace Hertzole.GameJolt
{
	internal readonly struct GetScoresResponse : IResponse, IEquatable<GetScoresResponse>
	{
		[JsonName("success")]
		[JsonConverter(typeof(GameJoltBooleanConverter))]
		public readonly bool success;
		[JsonName("message")]
		public readonly string? message;
		[JsonName("scores")]
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
		public GetScoresResponse(bool success, string? message, ScoreInternal[] scores)
		{
			this.success = success;
			this.message = message;
			this.scores = scores;
		}

		public bool Equals(GetScoresResponse other)
		{
			return success == other.success && EqualityHelper.StringEquals(message, other.message) && EqualityHelper.ArrayEquals(scores, other.scores);
		}

		public override bool Equals(object? obj)
		{
			return obj is GetScoresResponse other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = success.GetHashCode();
				hashCode = (hashCode * 397) ^ (message != null ? message.GetHashCode() : 0);
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
	}
}