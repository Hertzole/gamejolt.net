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
	internal readonly struct User
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
		public readonly string? userWebsite;
		[JsonName("developer_description")]
		public readonly string userDescription;

		[JsonConstructor]
		public User(int id,
			UserType type,
			string username,
			string avatarUrl,
			string signedUp,
			long signedUpTimestamp,
			string lastLoggedIn,
			long lastLoggedInTimestamp,
			UserStatus status,
			string displayName,
			string? userWebsite,
			string userDescription)
		{
			this.id = id;
			this.type = type;
			this.username = username;
			this.avatarUrl = avatarUrl;
			this.signedUp = signedUp;
			this.signedUpTimestamp = signedUpTimestamp;
			this.lastLoggedIn = lastLoggedIn;
			this.lastLoggedInTimestamp = lastLoggedInTimestamp;
			this.status = status;
			this.displayName = displayName;
			this.userWebsite = userWebsite;
			this.userDescription = userDescription;
		}

		public GameJoltUser ToPublicUser()
		{
			DateTime signedUpDate = DateTimeHelper.FromUnixTimestamp(signedUpTimestamp);
			DateTime lastLoggedInDate = DateTimeHelper.FromUnixTimestamp(lastLoggedInTimestamp);
			bool onlineNow = lastLoggedIn.Equals("online now", StringComparison.OrdinalIgnoreCase);

			return new GameJoltUser(id, type, username, avatarUrl, status, displayName, userWebsite, userDescription, signedUpDate, lastLoggedInDate,
				onlineNow);
		}
	}
}