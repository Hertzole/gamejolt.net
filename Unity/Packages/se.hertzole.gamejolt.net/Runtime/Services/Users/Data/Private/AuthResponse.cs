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
	internal readonly struct AuthResponse : IResponse, IEquatable<AuthResponse>
	{
		/// <summary>
		///     Whether the request succeeded or failed.
		/// </summary>
		[JsonName("success")]
		[JsonConverter(typeof(GameJoltBooleanConverter))]
		public bool Success { get; }

		/// <summary>
		///     If the request was not successful, this will contain the error message.
		/// </summary>
		[JsonName("message")]
		public string? Message { get; }

		[JsonConstructor]
		public AuthResponse(bool success, string? message)
		{
			Success = success;
			Message = message;
		}

		public bool Equals(AuthResponse other)
		{
			return EqualityHelper.ResponseEquals(this, other);
		}

		public override bool Equals(object? obj)
		{
			return obj is AuthResponse other && Equals(other);
		}

		public override int GetHashCode()
		{
			return EqualityHelper.ResponseHashCode(0, this);
		}

		public static bool operator ==(AuthResponse left, AuthResponse right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(AuthResponse left, AuthResponse right)
		{
			return !left.Equals(right);
		}

		public override string ToString()
		{
			return $"{nameof(AuthResponse)} ({nameof(Success)}: {Success}, {nameof(Message)}: {Message})";
		}
	}
}