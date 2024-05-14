#nullable enable

using System.Text;
using GameJolt.NET.Tests.Enums;
using GameJolt.NET.Tests.Extensions;
using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests.Serialization.Converters
{
	public sealed class UserConverter : BaseSerializationTest
	{
		[Test]
		public void WriteJson()
		{
			int id = faker.Random.Int();
			UserType type = faker.PickRandom<UserType>();
			string username = faker.Internet.UserName();
			string avatarUrl = faker.Internet.Url();
			string signedUp = faker.Date.Past().ToString("yyyy-MM-dd HH:mm:ss");
			long signedUpTimestamp = faker.Random.Long();
			string lastLoggedIn = faker.Date.Past().ToString("yyyy-MM-dd HH:mm:ss");
			long lastLoggedInTimestamp = faker.Random.Long();
			UserStatus status = faker.PickRandom<UserStatus>();
			string displayName = faker.Company.CompanyName();
			string userWebsite = faker.Internet.Url();
			string userDescription = faker.Lorem.Sentence();

			User user = new User(id, type, username, avatarUrl, signedUp, signedUpTimestamp, lastLoggedIn, lastLoggedInTimestamp, status, displayName,
				userWebsite, userDescription);

			string json = Serialize(user);

			string expected = WriteUserJson(id, type, username, avatarUrl, signedUp, signedUpTimestamp, lastLoggedIn, lastLoggedInTimestamp, status,
				displayName, userWebsite, userDescription);

			Assert.That(json, Is.EqualTo(expected));
		}

		[Test]
		public void ReadJson([Values] bool randomCapitalize,
			[Values] StringInitialization usernameInitialization,
			[Values] StringInitialization avatarUrlInitialization,
			[Values] StringInitialization signedUpInitialization,
			[Values] StringInitialization lastLoggedInInitialization,
			[Values] StringInitialization displayNameInitialization,
			[Values] StringInitialization userWebsiteInitialization,
			[Values] StringInitialization userDescriptionInitialization,
			[Values] bool writeNull)
		{
			int id = faker.Random.Int();
			UserType type = faker.PickRandom<UserType>();
			string? username = usernameInitialization.GetData();
			string? avatarUrl = avatarUrlInitialization.GetData();
			string? signedUp = signedUpInitialization.GetData();
			long signedUpTimestamp = faker.Random.Long();
			string? lastLoggedIn = lastLoggedInInitialization.GetData();
			long lastLoggedInTimestamp = faker.Random.Long();
			UserStatus status = faker.PickRandom<UserStatus>();
			string? displayName = displayNameInitialization.GetData();
			string? userWebsite = userWebsiteInitialization.GetData();
			string? userDescription = userDescriptionInitialization.GetData();

			string json = WriteUserJson(id, type, username, avatarUrl, signedUp, signedUpTimestamp, lastLoggedIn, lastLoggedInTimestamp, status, displayName,
				userWebsite, userDescription, writeNull, randomCapitalize);

			User user = Deserialize<User>(json);

			// Serializers returns empty string if null.
			username ??= string.Empty;
			avatarUrl ??= string.Empty;
			signedUp ??= string.Empty;
			lastLoggedIn ??= string.Empty;
			displayName ??= string.Empty;
			userWebsite ??= string.Empty;
			userDescription ??= string.Empty;

			Assert.That(user.id, Is.EqualTo(id));
			Assert.That(user.type, Is.EqualTo(type));
			Assert.That(user.username, Is.EqualTo(username));
			Assert.That(user.avatarUrl, Is.EqualTo(avatarUrl));
			Assert.That(user.signedUp, Is.EqualTo(signedUp));
			Assert.That(user.signedUpTimestamp, Is.EqualTo(signedUpTimestamp));
			Assert.That(user.lastLoggedIn, Is.EqualTo(lastLoggedIn));
			Assert.That(user.lastLoggedInTimestamp, Is.EqualTo(lastLoggedInTimestamp));
			Assert.That(user.status, Is.EqualTo(status));
			Assert.That(user.displayName, Is.EqualTo(displayName));
			Assert.That(user.userWebsite, Is.EqualTo(userWebsite));
			Assert.That(user.userDescription, Is.EqualTo(userDescription));
		}

		[Test]
		public void ReadJson_TooManyFields([Values] bool randomCapitalize,
			[Values] StringInitialization usernameInitialization,
			[Values] StringInitialization avatarUrlInitialization,
			[Values] StringInitialization signedUpInitialization,
			[Values] StringInitialization lastLoggedInInitialization,
			[Values] StringInitialization displayNameInitialization,
			[Values] StringInitialization userWebsiteInitialization,
			[Values] StringInitialization userDescriptionInitialization,
			[Values] bool writeNull,
			[Values] bool beforeData)
		{
			int id = faker.Random.Int();
			UserType type = faker.PickRandom<UserType>();
			string? username = usernameInitialization.GetData();
			string? avatarUrl = avatarUrlInitialization.GetData();
			string? signedUp = signedUpInitialization.GetData();
			long signedUpTimestamp = faker.Random.Long();
			string? lastLoggedIn = lastLoggedInInitialization.GetData();
			long lastLoggedInTimestamp = faker.Random.Long();
			UserStatus status = faker.PickRandom<UserStatus>();
			string? displayName = displayNameInitialization.GetData();
			string? userWebsite = userWebsiteInitialization.GetData();
			string? userDescription = userDescriptionInitialization.GetData();

			string json = WriteUserJson(id, type, username, avatarUrl, signedUp, signedUpTimestamp, lastLoggedIn, lastLoggedInTimestamp, status, displayName,
				userWebsite, userDescription, writeNull, randomCapitalize, true, beforeData);

			User user = Deserialize<User>(json);

			// Serializers returns empty string if null.
			username ??= string.Empty;
			avatarUrl ??= string.Empty;
			signedUp ??= string.Empty;
			lastLoggedIn ??= string.Empty;
			displayName ??= string.Empty;
			userWebsite ??= string.Empty;
			userDescription ??= string.Empty;

			Assert.That(user.id, Is.EqualTo(id));
			Assert.That(user.type, Is.EqualTo(type));
			Assert.That(user.username, Is.EqualTo(username));
			Assert.That(user.avatarUrl, Is.EqualTo(avatarUrl));
			Assert.That(user.signedUp, Is.EqualTo(signedUp));
			Assert.That(user.signedUpTimestamp, Is.EqualTo(signedUpTimestamp));
			Assert.That(user.lastLoggedIn, Is.EqualTo(lastLoggedIn));
			Assert.That(user.lastLoggedInTimestamp, Is.EqualTo(lastLoggedInTimestamp));
			Assert.That(user.status, Is.EqualTo(status));
			Assert.That(user.displayName, Is.EqualTo(displayName));
			Assert.That(user.userWebsite, Is.EqualTo(userWebsite));
			Assert.That(user.userDescription, Is.EqualTo(userDescription));
		}

		private static string WriteUserJson(int id,
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
			string? userDescription,
			bool writeNull = false,
			bool randomCapitalize = false,
			bool hasExtraData = false,
			bool beforeData = false)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append('{');

			if (hasExtraData && beforeData)
			{
				sb.AppendJsonPropertyName("extra_field", randomCapitalize);
				sb.AppendStringValue("haha");
				sb.Append(',');
			}

			sb.AppendJsonPropertyName("id", randomCapitalize);
			sb.Append(id);
			sb.Append(',');
			sb.AppendJsonPropertyName("type", randomCapitalize);
			sb.AppendStringValue(type.ToString());
			sb.Append(',');
			sb.AppendJsonPropertyName("username", randomCapitalize);
			sb.AppendStringValue(username, writeNull);
			sb.Append(',');
			sb.AppendJsonPropertyName("avatar_url", randomCapitalize);
			sb.AppendStringValue(avatarUrl, writeNull);
			sb.Append(',');
			sb.AppendJsonPropertyName("signed_up", randomCapitalize);
			sb.AppendStringValue(signedUp, writeNull);
			sb.Append(',');
			sb.AppendJsonPropertyName("signed_up_timestamp", randomCapitalize);
			sb.Append(signedUpTimestamp);
			sb.Append(',');
			sb.AppendJsonPropertyName("last_logged_in", randomCapitalize);
			sb.AppendStringValue(lastLoggedIn, writeNull);
			sb.Append(',');
			sb.AppendJsonPropertyName("last_logged_in_timestamp", randomCapitalize);
			sb.Append(lastLoggedInTimestamp);
			sb.Append(',');
			sb.AppendJsonPropertyName("status", randomCapitalize);
			sb.AppendStringValue(status.ToString());
			sb.Append(',');
			sb.AppendJsonPropertyName("developer_name", randomCapitalize);
			sb.AppendStringValue(displayName, writeNull);
			sb.Append(',');
			sb.AppendJsonPropertyName("developer_website", randomCapitalize);
			sb.AppendStringValue(userWebsite, writeNull);
			sb.Append(',');
			sb.AppendJsonPropertyName("developer_description", randomCapitalize);
			sb.AppendStringValue(userDescription, writeNull);

			if (hasExtraData && !beforeData)
			{
				sb.Append(',');
				sb.AppendJsonPropertyName("extra_field", randomCapitalize);
				sb.AppendStringValue("haha");
			}

			sb.Append('}');

			return sb.ToString();
		}
	}
}