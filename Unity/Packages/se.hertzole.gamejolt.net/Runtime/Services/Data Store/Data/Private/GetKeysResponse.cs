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
	internal readonly struct GetKeysResponse : IResponse, IEquatable<GetKeysResponse>
	{
		[JsonProperty("keys")]
		public readonly DataKey[] keys;
		[JsonProperty("success")]
		[JsonConverter(typeof(GameJoltBooleanConverter))]
		public bool Success { get; }
		[JsonProperty("message")]
		public string? Message { get; }

		[JsonConstructor]
		public GetKeysResponse(bool success, string? message, DataKey[]? keys)
		{
			this.keys = keys ?? Array.Empty<DataKey>();
			Success = success;
			Message = message;
		}

		public bool Equals(GetKeysResponse other)
		{
			return EqualityHelper.ResponseEquals(this, other) && EqualityHelper.ArrayEquals(keys, other.keys);
		}

		public override bool Equals(object? obj)
		{
			return obj is GetKeysResponse other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = EqualityHelper.ResponseHashCode(0, this);
				hashCode = (hashCode * 397) ^ keys.GetHashCode();
				return hashCode;
			}
		}

		public static bool operator ==(GetKeysResponse left, GetKeysResponse right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(GetKeysResponse left, GetKeysResponse right)
		{
			return !left.Equals(right);
		}

		public override string ToString()
		{
			return $"{nameof(GetKeysResponse)} ({nameof(Success)}: {Success}, {nameof(Message)}: {Message}, {nameof(keys)}: {keys.ToCommaSeparatedString()})";
		}
	}
}