using System;
using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests.Equality
{
	public sealed class Users : EqualityTest
	{
		[Test]
		public void User()
		{
			TestEquality((a, b) => a == b, (a, b) => a != b,
				new User(0, UserType.User, "username", "avatar", "signedUp", 0, "lastLoggedIn", 0, UserStatus.Active, "displayName", "website", "description"),
				new User(1, UserType.User, "username", "avatar", "signedUp", 0, "lastLoggedIn", 0, UserStatus.Active, "displayName", "website", "description"),
				new User(0, UserType.Developer, "username", "avatar", "signedUp", 0, "lastLoggedIn", 0, UserStatus.Active, "displayName", "website",
					"description"),
				new User(0, UserType.User, "username2", "avatar", "signedUp", 0, "lastLoggedIn", 0, UserStatus.Active, "displayName", "website", "description"),
				new User(0, UserType.User, "username", "avatar2", "signedUp", 0, "lastLoggedIn", 0, UserStatus.Active, "displayName", "website", "description"),
				new User(0, UserType.User, "username", "avatar", "signedUp2", 0, "lastLoggedIn", 0, UserStatus.Active, "displayName", "website", "description"),
				new User(0, UserType.User, "username", "avatar", "signedUp", 1, "lastLoggedIn", 0, UserStatus.Active, "displayName", "website", "description"),
				new User(0, UserType.User, "username", "avatar", "signedUp", 0, "lastLoggedIn2", 0, UserStatus.Active, "displayName", "website", "description"),
				new User(0, UserType.User, "username", "avatar", "signedUp", 0, "lastLoggedIn", 1, UserStatus.Active, "displayName", "website", "description"),
				new User(0, UserType.User, "username", "avatar", "signedUp", 0, "lastLoggedIn", 0, UserStatus.Banned, "displayName", "website", "description"),
				new User(0, UserType.User, "username", "avatar", "signedUp", 0, "lastLoggedIn", 0, UserStatus.Active, "displayName2", "website", "description"),
				new User(0, UserType.User, "username", "avatar", "signedUp", 0, "lastLoggedIn", 0, UserStatus.Active, "displayName", "website2", "description"),
				new User(0, UserType.User, "username", "avatar", "signedUp", 0, "lastLoggedIn", 0, UserStatus.Active, "displayName", null, "description"),
				new User(0, UserType.User, "username", "avatar", "signedUp", 0, "lastLoggedIn", 0, UserStatus.Active, "displayName", "website",
					"description2"));
		}

		[Test]
		public void UsersFetchResponse()
		{
			TestEquality((a, b) => a == b, (a, b) => a != b,
				new UsersFetchResponse(true, "message", Array.Empty<User>()),
				new UsersFetchResponse(false, "message", Array.Empty<User>()),
				new UsersFetchResponse(true, "message2", Array.Empty<User>()),
				new UsersFetchResponse(true, "message", new[] { DummyData.User() }));
		}

		[Test]
		public void GameJoltUser()
		{
			TestEquality((a, b) => a == b, (a, b) => a != b,
				new GameJoltUser(0, UserType.User, "username", "avatar", UserStatus.Active, "displayName", "website", "description", DateTime.MinValue,
					DateTime.MaxValue, true),
				new GameJoltUser(1, UserType.User, "username", "avatar", UserStatus.Active, "displayName", "website", "description", DateTime.MinValue,
					DateTime.MaxValue, true),
				new GameJoltUser(0, UserType.Developer, "username", "avatar", UserStatus.Active, "displayName", "website", "description", DateTime.MinValue,
					DateTime.MaxValue, true),
				new GameJoltUser(0, UserType.User, "username2", "avatar", UserStatus.Active, "displayName", "website", "description", DateTime.MinValue,
					DateTime.MaxValue, true),
				new GameJoltUser(0, UserType.User, "username", "avatar2", UserStatus.Active, "displayName", "website", "description", DateTime.MinValue,
					DateTime.MaxValue, true),
				new GameJoltUser(0, UserType.User, "username", "avatar", UserStatus.Banned, "displayName", "website", "description", DateTime.MinValue,
					DateTime.MaxValue, true),
				new GameJoltUser(0, UserType.User, "username", "avatar", UserStatus.Active, "displayName2", "website", "description", DateTime.MinValue,
					DateTime.MaxValue, true),
				new GameJoltUser(0, UserType.User, "username", "avatar", UserStatus.Active, "displayName", "website2", "description", DateTime.MinValue,
					DateTime.MaxValue, true),
				new GameJoltUser(0, UserType.User, "username", "avatar", UserStatus.Active, "displayName", null, "description", DateTime.MinValue,
					DateTime.MaxValue, true),
				new GameJoltUser(0, UserType.User, "username", "avatar", UserStatus.Active, "displayName", "website", "description2", DateTime.MinValue,
					DateTime.MaxValue, true),
				new GameJoltUser(0, UserType.User, "username", "avatar", UserStatus.Active, "displayName", "website", "description",
					DateTime.MinValue.AddSeconds(1), DateTime.MaxValue, true),
				new GameJoltUser(0, UserType.User, "username", "avatar", UserStatus.Active, "displayName", "website", "description", DateTime.MinValue,
					DateTime.MaxValue.AddSeconds(-1), true),
				new GameJoltUser(0, UserType.User, "username", "avatar", UserStatus.Active, "displayName", "website", "description", DateTime.MinValue,
					DateTime.MaxValue, false));
		}
	}
}