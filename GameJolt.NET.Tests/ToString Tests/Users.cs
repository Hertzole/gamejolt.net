#nullable enable

using System;
using GameJolt.NET.Tests.Enums;
using GameJolt.NET.Tests.Extensions;
using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests.ToString
{
	public sealed class Users : BaseToStringTest
	{
		[Test]
		public void User([Values] bool nullUserWebsite)
		{
			int id = faker.Random.Int();
			UserType type = faker.PickRandom<UserType>();
			string username = faker.Internet.UserName();
			string avatarUrl = faker.Internet.Avatar();
			string signedUp = faker.Random.String();
			long signedUpTimestamp = faker.Random.Long();
			string lastLoggedIn = faker.Random.String();
			long lastLoggedInTimestamp = faker.Random.Long();
			UserStatus status = faker.PickRandom<UserStatus>();
			string displayName = faker.Company.CompanyName();
			string? userWebsite = nullUserWebsite ? null : faker.Internet.Url();
			string userDescription = faker.Lorem.Sentence();

			User user = new User(id, type, username, avatarUrl, signedUp, signedUpTimestamp, lastLoggedIn, lastLoggedInTimestamp, status, displayName,
				userWebsite, userDescription);

			Assert.That(user.ToString(),
				Is.EqualTo(
					$"{nameof(Hertzole.GameJolt.User)} (id: {id}, type: {type}, username: {username}, avatarUrl: {avatarUrl}, signedUp: {signedUp}, signedUpTimestamp: {signedUpTimestamp}, lastLoggedIn: {lastLoggedIn}, lastLoggedInTimestamp: {lastLoggedInTimestamp}, status: {status}, displayName: {displayName}, userWebsite: {userWebsite}, userDescription: {userDescription})"));
		}

		[Test]
		public void UsersFetchResponse([Values] ArrayInitialization arrayInitialization, [Values] bool nullMessage)
		{
			User[]? users = CreateArray(arrayInitialization,
				f => new User(f.Random.Int(), f.PickRandom<UserType>(), f.Internet.UserName(), f.Internet.Avatar(), f.Random.String(), f.Random.Long(),
					f.Random.String(), f.Random.Long(), f.PickRandom<UserStatus>(), f.Company.CompanyName(), f.Internet.Url(), f.Lorem.Sentence()));

			bool success = faker.Random.Bool();
			string? message = nullMessage ? null : faker.Lorem.Sentence();

			UsersFetchResponse response = new UsersFetchResponse(success, message, users);

			Assert.That(response.ToString(),
				Is.EqualTo($"{nameof(Hertzole.GameJolt.UsersFetchResponse)} (Success: {success}, Message: {message}, Users: {users.GetExpectedString()})"));
		}

		[Test]
		public void GameJoltUser([Values] bool nullUserWebsite)
		{
			int id = faker.Random.Int();
			UserType type = faker.PickRandom<UserType>();
			string username = faker.Internet.UserName();
			string avatarUrl = faker.Internet.Avatar();
			UserStatus status = faker.PickRandom<UserStatus>();
			string displayName = faker.Company.CompanyName();
			string? userWebsite = nullUserWebsite ? null : faker.Internet.Url();
			string userDescription = faker.Lorem.Sentence();
			DateTime signedUp = faker.Date.Past();
			DateTime lastLoggedIn = faker.Date.Past();
			bool onlineNow = faker.Random.Bool();

			GameJoltUser user = new GameJoltUser(id, type, username, avatarUrl, status, displayName, userWebsite, userDescription, signedUp, lastLoggedIn,
				onlineNow);

			Assert.That(user.ToString(),
				Is.EqualTo(
					$"{nameof(Hertzole.GameJolt.GameJoltUser)} (Id: {id}, Type: {type}, Username: {username}, AvatarUrl: {avatarUrl}, SignedUp: {signedUp}, LastLoggedIn: {lastLoggedIn}, OnlineNow: {onlineNow}, Status: {status}, DisplayName: {displayName}{(nullUserWebsite ? "" : $", UserWebsite: {userWebsite}")}, UserDescription: {userDescription})"));
		}
	}
}