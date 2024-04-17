﻿#nullable enable

#if NET6_0_OR_GREATER
using JsonName = System.Text.Json.Serialization.JsonPropertyNameAttribute;
using System.Text.Json.Serialization;
using Hertzole.GameJolt.Serialization.System;
#else
using JsonName = Newtonsoft.Json.JsonPropertyAttribute;
using Newtonsoft.Json;
using Hertzole.GameJolt.Serialization.Newtonsoft;
#endif
using System;

namespace Hertzole.GameJolt
{
	internal readonly struct Response : IResponse, IEquatable<Response>
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
		public Response(bool success, string? message)
		{
			Success = success;
			Message = message;
		}

		public bool Equals(Response other)
		{
			return EqualityHelper.ResponseEquals(this, other);
		}

		public override bool Equals(object? obj)
		{
			return obj is Response other && Equals(other);
		}

		public override int GetHashCode()
		{
			return EqualityHelper.ResponseHashCode(0, this);
		}

		public static bool operator ==(Response left, Response right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Response left, Response right)
		{
			return !left.Equals(right);
		}

		public override string ToString()
		{
			return $"{nameof(Response)} ({nameof(Success)}: {Success}, {nameof(Message)}: {Message})";
		}
	}
}