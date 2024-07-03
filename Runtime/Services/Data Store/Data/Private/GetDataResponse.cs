#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

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
	internal readonly struct GetDataResponse : IResponse, IEquatable<GetDataResponse>
	{
		[JsonProperty("data")]
		public readonly string data;
		[JsonProperty("success")]
		[JsonConverter(typeof(GameJoltBooleanConverter))]
		public bool Success { get; }
		[JsonProperty("message")]
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
				hashCode = (hashCode * 397) ^ (!string.IsNullOrEmpty(data) ? data.GetHashCode() : 0);
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
#endif // DISABLE_GAMEJOLT