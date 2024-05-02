﻿#nullable enable

using System;
using Hertzole.GameJolt.Serialization.Shared;
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

namespace Hertzole.GameJolt
{
	internal readonly struct User : IEquatable<User>
	{
		[JsonName("id")]
		[JsonConverter(typeof(GameJoltIntConverter))]
		public readonly int id;
		[JsonName("type")]
		[JsonConverter(typeof(GameJoltUserTypeConverter))]
		public readonly UserType type;
		[JsonName("username")]
		public readonly string username;
		[JsonName("avatar_url")]
		public readonly string avatarUrl;
		[JsonName("signed_up")]
		public readonly string signedUp;
		[JsonName("signed_up_timestamp")]
		[JsonConverter(typeof(GameJoltLongConverter))]
		public readonly long signedUpTimestamp;
		[JsonName("last_logged_in")]
		public readonly string lastLoggedIn;
		[JsonName("last_logged_in_timestamp")]
		[JsonConverter(typeof(GameJoltLongConverter))]
		public readonly long lastLoggedInTimestamp;
		[JsonName("status")]
		[JsonConverter(typeof(GameJoltStatusConverter))]
		public readonly UserStatus status;
		[JsonName("developer_name")]
		public readonly string displayName;
		[JsonName("developer_website")]
		public readonly string userWebsite;
		[JsonName("developer_description")]
		public readonly string userDescription;

		[JsonConstructor]
		public User(int id,
			UserType type,
			string? username,
			string? avatarUrl,
			string? signedUp,
			long signedUpTimestamp,
			string? lastLoggedIn,
			long lastLoggedInTimestamp,
			UserStatus status,
			string? displayName,
			string? userWebsite,
			string? userDescription)
		{
			this.id = id;
			this.type = type;
			this.username = username ?? string.Empty;
			this.avatarUrl = avatarUrl ?? string.Empty;
			this.signedUp = signedUp ?? string.Empty;
			this.signedUpTimestamp = signedUpTimestamp;
			this.lastLoggedIn = lastLoggedIn ?? string.Empty;
			this.lastLoggedInTimestamp = lastLoggedInTimestamp;
			this.status = status;
			this.displayName = displayName ?? string.Empty;
			this.userWebsite = userWebsite ?? string.Empty;
			this.userDescription = userDescription ?? string.Empty;
		}

		public bool Equals(User other)
		{
			return id == other.id && type == other.type && signedUpTimestamp == other.signedUpTimestamp &&
			       lastLoggedInTimestamp == other.lastLoggedInTimestamp && status == other.status &&
			       EqualityHelper.StringEquals(username, other.username) &&
			       EqualityHelper.StringEquals(avatarUrl, other.avatarUrl) &&
			       EqualityHelper.StringEquals(signedUp, other.signedUp) &&
			       EqualityHelper.StringEquals(lastLoggedIn, other.lastLoggedIn) &&
			       EqualityHelper.StringEquals(displayName, other.displayName) &&
			       EqualityHelper.StringEquals(userWebsite, other.userWebsite) &&
			       EqualityHelper.StringEquals(userDescription, other.userDescription);
		}

		public override bool Equals(object? obj)
		{
			return obj is User other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = id;
				hashCode = (hashCode * 397) ^ (int) type;
				hashCode = (hashCode * 397) ^ signedUpTimestamp.GetHashCode();
				hashCode = (hashCode * 397) ^ lastLoggedInTimestamp.GetHashCode();
				hashCode = (hashCode * 397) ^ (int) status;
				hashCode = (hashCode * 397) ^ username.GetHashCode();
				hashCode = (hashCode * 397) ^ avatarUrl.GetHashCode();
				hashCode = (hashCode * 397) ^ signedUp.GetHashCode();
				hashCode = (hashCode * 397) ^ lastLoggedIn.GetHashCode();
				hashCode = (hashCode * 397) ^ displayName.GetHashCode();
				hashCode = (hashCode * 397) ^ (userWebsite != null ? userWebsite.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ userDescription.GetHashCode();
				return hashCode;
			}
		}

		public static bool operator ==(User left, User right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(User left, User right)
		{
			return !left.Equals(right);
		}

		public GameJoltUser ToPublicUser()
		{
			DateTime signedUpDate = DateTimeHelper.FromUnixTimestamp(signedUpTimestamp);
			DateTime lastLoggedInDate = DateTimeHelper.FromUnixTimestamp(lastLoggedInTimestamp);
			bool onlineNow = lastLoggedIn.Equals("online now", StringComparison.OrdinalIgnoreCase);

			return new GameJoltUser(id, type, username, avatarUrl, status, displayName, userWebsite, userDescription, signedUpDate, lastLoggedInDate,
				onlineNow);
		}

		public override string ToString()
		{
			return
				$"{nameof(User)} ({nameof(id)}: {id}, {nameof(type)}: {type}, {nameof(username)}: {username}, {nameof(avatarUrl)}: {avatarUrl}, {nameof(signedUp)}: {signedUp}, {nameof(signedUpTimestamp)}: {signedUpTimestamp}, {nameof(lastLoggedIn)}: {lastLoggedIn}, {nameof(lastLoggedInTimestamp)}: {lastLoggedInTimestamp}, {nameof(status)}: {status}, {nameof(displayName)}: {displayName}, {nameof(userWebsite)}: {userWebsite}, {nameof(userDescription)}: {userDescription})";
		}
	}
}