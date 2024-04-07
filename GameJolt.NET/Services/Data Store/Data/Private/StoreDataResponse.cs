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
	internal readonly struct StoreDataResponse : IResponse, IEquatable<StoreDataResponse>
	{
		[JsonName("success")]
		[JsonConverter(typeof(GameJoltBooleanConverter))]
		public bool Success { get; }
		[JsonName("message")]
		public string? Message { get; }

		[JsonConstructor]
		public StoreDataResponse(bool success, string? message)
		{
			Success = success;
			Message = message;
		}

		public bool Equals(StoreDataResponse other)
		{
			return EqualityHelper.ResponseEquals(this, other);
		}

		public override bool Equals(object? obj)
		{
			return obj is StoreDataResponse other && Equals(other);
		}

		public override int GetHashCode()
		{
			return EqualityHelper.ResponseHashCode(0, this);
		}

		public static bool operator ==(StoreDataResponse left, StoreDataResponse right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(StoreDataResponse left, StoreDataResponse right)
		{
			return !left.Equals(right);
		}
	}
}