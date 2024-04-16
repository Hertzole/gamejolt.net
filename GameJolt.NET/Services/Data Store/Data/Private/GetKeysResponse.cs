#nullable enable

#if NET6_0_OR_GREATER
using JsonName = System.Text.Json.Serialization.JsonPropertyNameAttribute;
using JsonConverter = System.Text.Json.Serialization.JsonConverterAttribute;
using JsonConstructor = System.Text.Json.Serialization.JsonConstructorAttribute;
#else
using JsonName = Newtonsoft.Json.JsonPropertyAttribute;
using JsonConverter = Newtonsoft.Json.JsonConverterAttribute;
using JsonConstructor = Newtonsoft.Json.JsonConstructorAttribute;
#endif
using System;

namespace Hertzole.GameJolt
{
	internal readonly struct GetKeysResponse : IResponse, IEquatable<GetKeysResponse>
	{
		[JsonName("keys")]
		public readonly DataKey[] keys;
		[JsonName("success")]
		[JsonConverter(typeof(GameJoltBooleanConverter))]
		public bool Success { get; }
		[JsonName("message")]
		public string? Message { get; }

		[JsonConstructor]
		public GetKeysResponse(bool success, string? message, DataKey[] keys)
		{
			this.keys = keys;
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