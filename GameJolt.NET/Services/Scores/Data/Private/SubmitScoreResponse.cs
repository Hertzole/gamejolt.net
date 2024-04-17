#nullable enable

using System;
#if NET6_0_OR_GREATER
using JsonName = System.Text.Json.Serialization.JsonPropertyNameAttribute;
using JsonConverter = System.Text.Json.Serialization.JsonConverterAttribute;
using JsonConstructor = System.Text.Json.Serialization.JsonConstructorAttribute;
using Hertzole.GameJolt.Serialization.System;
#else
using JsonName = Newtonsoft.Json.JsonPropertyAttribute;
using JsonConverter = Newtonsoft.Json.JsonConverterAttribute;
using JsonConstructor = Newtonsoft.Json.JsonConstructorAttribute;
using Hertzole.GameJolt.Serialization.Newtonsoft;
#endif

namespace Hertzole.GameJolt
{
	[Obsolete("Use Response instead.", true)]
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
			return EqualityHelper.ResponseEquals(this, other);
		}

		public override bool Equals(object? obj)
		{
			return obj is SubmitScoreResponse other && Equals(other);
		}

		public override int GetHashCode()
		{
			return EqualityHelper.ResponseHashCode(0, this);
		}

		public static bool operator ==(SubmitScoreResponse left, SubmitScoreResponse right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(SubmitScoreResponse left, SubmitScoreResponse right)
		{
			return !left.Equals(right);
		}

		public override string ToString()
		{
			return $"{nameof(SubmitScoreResponse)} ({nameof(Success)}: {Success}, {nameof(Message)}: {Message})";
		}
	}
}