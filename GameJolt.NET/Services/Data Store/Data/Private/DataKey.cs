#nullable enable

using System;
#if NET6_0_OR_GREATER || FORCE_SYSTEM_JSON
using JsonProperty = System.Text.Json.Serialization.JsonPropertyNameAttribute;
using System.Text.Json.Serialization;
#else
using Newtonsoft.Json;
#endif

namespace Hertzole.GameJolt
{
	internal readonly struct DataKey : IEquatable<DataKey>
	{
		[JsonProperty("key")]
		public readonly string key;

		[JsonConstructor]
		public DataKey(string? key)
		{
			this.key = key ?? string.Empty;
		}

		public bool Equals(DataKey other)
		{
			return EqualityHelper.StringEquals(key, other.key);
		}

		public override bool Equals(object? obj)
		{
			return obj is DataKey other && Equals(other);
		}

		public override int GetHashCode()
		{
			return !string.IsNullOrEmpty(key) ? key.GetHashCode() : 0;
		}

		public static bool operator ==(DataKey left, DataKey right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(DataKey left, DataKey right)
		{
			return !left.Equals(right);
		}

		public override string ToString()
		{
			return $"{nameof(DataKey)} ({nameof(key)}: {key})";
		}
	}
}