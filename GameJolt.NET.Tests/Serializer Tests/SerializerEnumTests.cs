using Hertzole.GameJolt;
using NUnit.Framework;
#if NET6_0_OR_GREATER
using JsonException = System.Text.Json.JsonException;
#else
using JsonException = Newtonsoft.Json.JsonSerializationException;
#endif

namespace GameJolt.NET.Tests
{
	public class SerializerEnumTests
	{
		private static object[] userStatusTestCases =
		{
			new object[] { "\"active\"", UserStatus.Active },
			new object[] { "\"banned\"", UserStatus.Banned },
			new object[] { "\"ACTIVE\"", UserStatus.Active },
			new object[] { "\"BANNED\"", UserStatus.Banned },
			new object[] { "\"Active\"", UserStatus.Active },
			new object[] { "\"Banned\"", UserStatus.Banned },
			new object[] { "\"aCtIvE\"", UserStatus.Active },
			new object[] { "\"bAnNeD\"", UserStatus.Banned },
			new object[] { "\"0\"", UserStatus.Active },
			new object[] { "\"10\"", UserStatus.Banned },
			new object[] { 0.ToString(), UserStatus.Active },
			new object[] { 10.ToString(), UserStatus.Banned }
		};

		private static object[] trophyDifficultyTestCases =
		{
			new object[] { "\"bronze\"", TrophyDifficulty.Bronze },
			new object[] { "\"silver\"", TrophyDifficulty.Silver },
			new object[] { "\"gold\"", TrophyDifficulty.Gold },
			new object[] { "\"platinum\"", TrophyDifficulty.Platinum },
			new object[] { "\"BRONZE\"", TrophyDifficulty.Bronze },
			new object[] { "\"SILVER\"", TrophyDifficulty.Silver },
			new object[] { "\"GOLD\"", TrophyDifficulty.Gold },
			new object[] { "\"PLATINUM\"", TrophyDifficulty.Platinum },
			new object[] { "\"Bronze\"", TrophyDifficulty.Bronze },
			new object[] { "\"Silver\"", TrophyDifficulty.Silver },
			new object[] { "\"Gold\"", TrophyDifficulty.Gold },
			new object[] { "\"Platinum\"", TrophyDifficulty.Platinum },
			new object[] { "\"bRoNzE\"", TrophyDifficulty.Bronze },
			new object[] { "\"sIlVeR\"", TrophyDifficulty.Silver },
			new object[] { "\"gOlD\"", TrophyDifficulty.Gold },
			new object[] { "\"pLaTiNuM\"", TrophyDifficulty.Platinum },
			new object[] { "\"0\"", TrophyDifficulty.Bronze },
			new object[] { "\"1\"", TrophyDifficulty.Silver },
			new object[] { "\"2\"", TrophyDifficulty.Gold },
			new object[] { "\"3\"", TrophyDifficulty.Platinum },
			new object[] { 0.ToString(), TrophyDifficulty.Bronze },
			new object[] { 1.ToString(), TrophyDifficulty.Silver },
			new object[] { 2.ToString(), TrophyDifficulty.Gold },
			new object[] { 3.ToString(), TrophyDifficulty.Platinum }
		};

		private static object[] userTypeTestCases =
		{
			new object[] { "\"user\"", UserType.User },
			new object[] { "\"developer\"", UserType.Developer },
			new object[] { "\"moderator\"", UserType.Moderator },
			new object[] { "\"administrator\"", UserType.Administrator },
			new object[] { "\"USER\"", UserType.User },
			new object[] { "\"DEVELOPER\"", UserType.Developer },
			new object[] { "\"MODERATOR\"", UserType.Moderator },
			new object[] { "\"ADMINISTRATOR\"", UserType.Administrator },
			new object[] { "\"User\"", UserType.User },
			new object[] { "\"Developer\"", UserType.Developer },
			new object[] { "\"Moderator\"", UserType.Moderator },
			new object[] { "\"Administrator\"", UserType.Administrator },
			new object[] { "\"uSeR\"", UserType.User },
			new object[] { "\"dEvElOpEr\"", UserType.Developer },
			new object[] { "\"mOdErAtOr\"", UserType.Moderator },
			new object[] { "\"aDmInIsTrAtOr\"", UserType.Administrator },
			new object[] { "\"0\"", UserType.User },
			new object[] { "\"1\"", UserType.Developer },
			new object[] { "\"2\"", UserType.Moderator },
			new object[] { "\"3\"", UserType.Administrator },
			new object[] { 0.ToString(), UserType.User },
			new object[] { 1.ToString(), UserType.Developer },
			new object[] { 2.ToString(), UserType.Moderator },
			new object[] { 3.ToString(), UserType.Administrator }
		};

		[Test]
		[TestCaseSource(nameof(userStatusTestCases))]
		public void CanDeserializeUserStatus(string value, UserStatus expected)
		{
			User response = GameJoltAPI.serializer.Deserialize<User>("{\"response\":{\"status\":" + value + "}}");

			Assert.That(response.status, Is.EqualTo(expected));
		}

		[Test]
		public void UserStatus_InvalidString_ThrowsException()
		{
			Assert.Throws<JsonException>(() => GameJoltAPI.serializer.Deserialize<User>("{\"response\":{\"status\":\"invalid\"}}"));
		}

		[Test]
		public void UserStatus_InvalidToken_ThrowsException()
		{
			Assert.Throws<JsonException>(() => GameJoltAPI.serializer.Deserialize<User>("{\"response\":{\"status\":true}}"));
		}

		[Test]
		public void UserStatus_InvalidNumber_ThrowsException()
		{
			Assert.Throws<JsonException>(() => GameJoltAPI.serializer.Deserialize<User>("{\"response\":{\"status\":11}}"));
		}
		
		[Test]
		public void UserStatus_EmptyString_ThrowsException()
		{
			Assert.Throws<JsonException>(() => GameJoltAPI.serializer.Deserialize<User>("{\"response\":{\"status\":\"\"}}"));
		}

		[Test]
		[TestCaseSource(nameof(trophyDifficultyTestCases))]
		public void CanDeserializeTrophyDifficulty(string value, TrophyDifficulty expected)
		{
			FetchTrophiesResponse response =
				GameJoltAPI.serializer.Deserialize<FetchTrophiesResponse>("{\"response\":{\"trophies\":[{\"difficulty\":" + value + "}]}}");

			Assert.That(response.trophies[0].difficulty, Is.EqualTo(expected));
		}

		[Test]
		public void TrophyDifficulty_InvalidString_ThrowsException()
		{
			Assert.Throws<JsonException>(() =>
				GameJoltAPI.serializer.Deserialize<FetchTrophiesResponse>("{\"response\":{\"trophies\":[{\"difficulty\":\"invalid\"}]}}"));
		}

		[Test]
		public void TrophyDifficulty_InvalidToken_ThrowsException()
		{
			Assert.Throws<JsonException>(() =>
				GameJoltAPI.serializer.Deserialize<FetchTrophiesResponse>("{\"response\":{\"trophies\":[{\"difficulty\":true}]}}"));
		}

		[Test]
		public void TrophyDifficulty_InvalidNumber_ThrowsException()
		{
			Assert.Throws<JsonException>(() => GameJoltAPI.serializer.Deserialize<FetchTrophiesResponse>("{\"response\":{\"trophies\":[{\"difficulty\":4}]}}"));
		}
		
		[Test]
		public void TrophyDifficulty_EmptyString_ThrowsException()
		{
			Assert.Throws<JsonException>(() => GameJoltAPI.serializer.Deserialize<FetchTrophiesResponse>("{\"response\":{\"trophies\":[{\"difficulty\":\"\"}]}}"));
		}

		[Test]
		[TestCaseSource(nameof(userTypeTestCases))]
		public void CanDeserializeUserType(string value, UserType expected)
		{
			UsersFetchResponse response = GameJoltAPI.serializer.Deserialize<UsersFetchResponse>("{\"response\":{\"users\":[{\"type\":" + value + "}]}}");

			Assert.That(response.Users[0].type, Is.EqualTo(expected));
		}

		[Test]
		public void UserType_InvalidString_ThrowsException()
		{
			Assert.Throws<JsonException>(() => GameJoltAPI.serializer.Deserialize<UsersFetchResponse>("{\"response\":{\"users\":[{\"type\":\"invalid\"}]}}"));
		}

		[Test]
		public void UserType_InvalidToken_ThrowsException()
		{
			Assert.Throws<JsonException>(() => GameJoltAPI.serializer.Deserialize<UsersFetchResponse>("{\"response\":{\"users\":[{\"type\":true}]}}"));
		}

		[Test]
		public void UserType_InvalidNumber_ThrowsException()
		{
			Assert.Throws<JsonException>(() => GameJoltAPI.serializer.Deserialize<UsersFetchResponse>("{\"response\":{\"users\":[{\"type\":4}]}}"));
		}

		[Test]
		public void UserType_EmptyString_ThrowsException()
		{
			Assert.Throws<JsonException>(() => GameJoltAPI.serializer.Deserialize<UsersFetchResponse>("{\"response\":{\"users\":[{\"type\":\"\"}]}}"));
		}
	}
}