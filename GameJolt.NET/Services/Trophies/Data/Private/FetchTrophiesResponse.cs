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
	internal readonly struct FetchTrophiesResponse : IResponse, IEquatable<FetchTrophiesResponse>
	{
		[JsonProperty("trophies")]
		public readonly TrophyInternal[] trophies;

		[JsonProperty("success")]
		[JsonConverter(typeof(GameJoltBooleanConverter))]
		public bool Success { get; }
		[JsonProperty("message")]
		public string? Message { get; }

		[JsonConstructor]
		public FetchTrophiesResponse(bool success, string? message, TrophyInternal[]? trophies)
		{
			this.trophies = trophies ?? Array.Empty<TrophyInternal>();
			Success = success;
			Message = message;
		}

		public bool Equals(FetchTrophiesResponse other)
		{
			return EqualityHelper.ResponseEquals(this, other) && EqualityHelper.ArrayEquals(trophies, other.trophies);
		}

		public override bool Equals(object? obj)
		{
			return obj is FetchTrophiesResponse other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = EqualityHelper.ResponseHashCode(0, this);
				hashCode = (hashCode * 397) ^ trophies.GetHashCode();
				return hashCode;
			}
		}

		public static bool operator ==(FetchTrophiesResponse left, FetchTrophiesResponse right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(FetchTrophiesResponse left, FetchTrophiesResponse right)
		{
			return !left.Equals(right);
		}

		public override string ToString()
		{
			return
				$"{nameof(FetchTrophiesResponse)} ({nameof(Success)}: {Success}, {nameof(Message)}: {Message}, {nameof(trophies)}: {trophies.ToCommaSeparatedString()})";
		}
	}
}
#endif // DISABLE_GAMEJOLT