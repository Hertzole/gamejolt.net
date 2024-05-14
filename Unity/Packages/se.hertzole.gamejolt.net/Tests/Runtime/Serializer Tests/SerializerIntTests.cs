#if NET6_0_OR_GREATER
using JsonException = System.Text.Json.JsonException;
#else
using JsonException = Newtonsoft.Json.JsonSerializationException;
#endif
using System;
using Bogus;
using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests
{
	[TestFixture(typeof(FriendId))]
	[TestFixture(typeof(GetScoreRankResponse))]
	[TestFixture(typeof(ScoreInternal))]
	[TestFixture(typeof(TableInternal))]
	[TestFixture(typeof(FetchTimeResponse))]
	[TestFixture(typeof(TrophyInternal))]
	[TestFixture(typeof(User))]
	public class SerializerIntTests<T>
	{
		private static readonly Randomizer randomizer = new Randomizer();

		private static object[] testCases =
		{
			GetRandomInt(true, true),
			GetRandomInt(true, false),
			GetRandomInt(false, true),
			GetRandomInt(false, false),
			new object[] { "1.1", 1 },
			new object[] { "\"1.1\"", 1 },
		};

		[Test]
		[TestCaseSource(nameof(testCases))]
		public void CanDeserialize(string value, int expected)
		{
			T response = GameJoltAPI.serializer.DeserializeResponse<T>("{\"response\":" + GetTypeJson(value) + "}");
			AssertType(response, expected);
		}

		[Test]
		public void InvalidString_ThrowsException()
		{
			Assert.Throws<JsonException>(() => GameJoltAPI.serializer.DeserializeResponse<T>("{\"response\":" + GetTypeJson("\"invalid\"") + "}"));
		}

		[Test]
		public void InvalidToken_ThrowsException()
		{
			Assert.Throws<JsonException>(() => GameJoltAPI.serializer.DeserializeResponse<T>("{\"response\":" + GetTypeJson("true") + "}"));
		}
		
		[Test]
		public void EmptyString_ThrowsException()
		{
			Assert.Throws<JsonException>(() => GameJoltAPI.serializer.DeserializeResponse<T>("{\"response\":" + GetTypeJson("\"\"") + "}"));
		}
		
		public void NullString_ThrowsException()
		{
			Assert.Throws<JsonException>(() => GameJoltAPI.serializer.DeserializeResponse<T>("{\"response\":" + GetTypeJson("") + "}"));
		}

		private static string GetTypeJson(string value)
		{
			switch (typeof(T).Name)
			{
				case "FriendId":
					return "{\"friend_id\": " + value + "}";
				case "GetScoreRankResponse":
					return "{\"rank\": " + value + "}";
				case "ScoreInternal":
					return "{\"sort\": " + value + ", \"user_id\": " + value + "}";
				case "TableInternal":
					return "{\"id\": " + value + "}";
				case "FetchTimeResponse":
					return "{\"year\": " + value + ", \"month\": " + value + ", \"day\": " + value + ", \"hour\": " + value + ", \"minute\": " + value +
					       ", \"second\": " + value + "}";
				case "TrophyInternal":
					return "{\"id\": " + value + "}";
				case "User":
					return "{\"id\": " + value + "}";
				default:
					throw new NotImplementedException($"Type {typeof(T).Name} is not implemented.");
			}
		}

		private static void AssertType(T response, int expected)
		{
			if (response is FriendId friendId)
			{
				Assert.That(friendId.id, Is.EqualTo(expected));
				return;
			}

			if (response is GetScoreRankResponse getScoreRankResponse)
			{
				Assert.That(getScoreRankResponse.rank, Is.EqualTo(expected));
				return;
			}

			if (response is ScoreInternal scoreInternal)
			{
				Assert.That(scoreInternal.sort, Is.EqualTo(expected));
				Assert.That(scoreInternal.userId, Is.EqualTo(expected));
				return;
			}

			if (response is TableInternal tableInternal)
			{
				Assert.That(tableInternal.id, Is.EqualTo(expected));
				return;
			}

			if (response is FetchTimeResponse fetchTimeResponse)
			{
				Assert.That(fetchTimeResponse.year, Is.EqualTo(expected));
				Assert.That(fetchTimeResponse.month, Is.EqualTo(expected));
				Assert.That(fetchTimeResponse.day, Is.EqualTo(expected));
				Assert.That(fetchTimeResponse.hour, Is.EqualTo(expected));
				Assert.That(fetchTimeResponse.minute, Is.EqualTo(expected));
				Assert.That(fetchTimeResponse.second, Is.EqualTo(expected));
				return;
			}

			if (response is TrophyInternal trophyInternal)
			{
				Assert.That(trophyInternal.id, Is.EqualTo(expected));
				return;
			}

			if (response is User user)
			{
				Assert.That(user.id, Is.EqualTo(expected));
				return;
			}

			throw new NotImplementedException($"Type {typeof(T).Name} is not implemented.");
		}

		private static object[] GetRandomInt(bool asString, bool positive)
		{
			int value = randomizer.Int(1);
			if (!positive)
			{
				value *= -1;
			}

			if (asString)
			{
				return new object[] { $"\"{value}\"", value };
			}

			return new object[] { value.ToString(), value };
		}
	}
}