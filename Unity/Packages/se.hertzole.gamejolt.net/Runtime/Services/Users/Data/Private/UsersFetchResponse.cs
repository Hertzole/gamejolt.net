﻿#nullable enable

using System;
#if NET6_0_OR_GREATER
using JsonName = System.Text.Json.Serialization.JsonPropertyNameAttribute;
using System.Text.Json.Serialization;
using Hertzole.GameJolt.Serialization.System;
#else
using JsonName = Newtonsoft.Json.JsonPropertyAttribute;
using Newtonsoft.Json;
using Hertzole.GameJolt.Serialization.Newtonsoft;
#endif

namespace Hertzole.GameJolt
{
	internal readonly struct UsersFetchResponse : IResponse, IEquatable<UsersFetchResponse>
	{
		[JsonName("success")]
		[JsonConverter(typeof(GameJoltBooleanConverter))]
		public bool Success { get; }
		[JsonName("message")]
		public string? Message { get; }

		[JsonName("users")]
		public User[] Users { get; }

		[JsonConstructor]
		public UsersFetchResponse(bool success, string? message, User[]? users)
		{
			Success = success;
			Message = message;
			Users = users ?? Array.Empty<User>();
		}

		public UsersFetchResponse(bool success, string? message, User user)
		{
			Success = success;
			Message = message;
			Users = new[] { user };
		}

		public bool Equals(UsersFetchResponse other)
		{
			return EqualityHelper.ResponseEquals(this, other) && EqualityHelper.ArrayEquals(Users, other.Users);
		}

		public override bool Equals(object? obj)
		{
			return obj is UsersFetchResponse other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = EqualityHelper.ResponseHashCode(0, this);
				hashCode = (hashCode * 397) ^ Users.GetHashCode();
				return hashCode;
			}
		}

		public static bool operator ==(UsersFetchResponse left, UsersFetchResponse right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(UsersFetchResponse left, UsersFetchResponse right)
		{
			return !left.Equals(right);
		}

		public override string ToString()
		{
			return $"{nameof(UsersFetchResponse)} (Success: {Success}, Message: {Message}, Users: {Users.ToCommaSeparatedString()})";
		}
	}
}