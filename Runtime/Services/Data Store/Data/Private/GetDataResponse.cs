#nullable enable

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
using System;

namespace Hertzole.GameJolt
{
	internal readonly struct GetDataResponse : IResponse, IEquatable<GetDataResponse>
	{
		[JsonName("data")]
		public readonly string data;
		[JsonName("success")]
		[JsonConverter(typeof(GameJoltBooleanConverter))]
		public bool Success { get; }
		[JsonName("message")]
		public string? Message { get; }

		[JsonConstructor]
		public GetDataResponse(bool success, string? message, string? data)
		{
			this.data = data ?? string.Empty;
			Success = success;
			Message = message;
		}

		public bool Equals(GetDataResponse other)
		{
			return EqualityHelper.ResponseEquals(this, other) && EqualityHelper.StringEquals(data, other.data);
		}

		public override bool Equals(object? obj)
		{
			return obj is GetDataResponse other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = EqualityHelper.ResponseHashCode(0, this);
				hashCode = (hashCode * 397) ^ (data != null ? data.GetHashCode() : 0);
				return hashCode;
			}
		}

		public static bool operator ==(GetDataResponse left, GetDataResponse right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(GetDataResponse left, GetDataResponse right)
		{
			return !left.Equals(right);
		}

		public override string ToString()
		{
			return $"{nameof(GetDataResponse)} ({nameof(Success)}: {Success}, {nameof(Message)}: {Message}, {nameof(data)}: {data})";
		}
	}
}