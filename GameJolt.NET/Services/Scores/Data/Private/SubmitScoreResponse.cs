#nullable enable

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
	internal readonly struct SubmitScoreResponse : IResponse, IEquatable<SubmitScoreResponse>
	{
		[JsonName("success")]
		[JsonConverter(typeof(GameJoltBooleanConverter))]
		public bool Success { get; }
		[JsonName("message")]
		public string? Message { get; }

		[JsonConstructor]
		public SubmitScoreResponse(bool success, string? message)
		{
			Success = success;
			Message = message;
		}

		public bool Equals(SubmitScoreResponse other)
		{
			return Success == other.Success && EqualityHelper.StringEquals(Message, other.Message);
		}

		public override bool Equals(object? obj)
		{
			return obj is SubmitScoreResponse other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (Success.GetHashCode() * 397) ^ (Message != null ? Message.GetHashCode() : 0);
			}
		}

		public static bool operator ==(SubmitScoreResponse left, SubmitScoreResponse right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(SubmitScoreResponse left, SubmitScoreResponse right)
		{
			return !left.Equals(right);
		}
	}
}