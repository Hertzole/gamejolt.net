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
	[TestFixture(typeof(ScoreInternal))]
	[TestFixture(typeof(FetchTimeResponse))]
	[TestFixture(typeof(User))]
	public class SerializerLongTests<T>
	{
		private static readonly Randomizer randomizer = new Randomizer();

		private static object[] testCases =
		{
			GetRandomLong(true, true),
			GetRandomLong(true, false),
			GetRandomLong(false, true),
			GetRandomLong(false, false),
			new object[] { "1.1", 1 },
			new object[] { "\"1.1\"", 1 },
		};

		[Test]
		[TestCaseSource(nameof(testCases))]
		public void CanDeserialize(string value, long expected)
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

		private static string GetTypeJson(string value)
		{
			switch (typeof(T).Name)
			{
				case "ScoreInternal":
					return "{\"stored_timestamp\": " + value + "}";
				case "FetchTimeResponse":
					return "{\"timestamp\": " + value + "}";
				case "User":
					return "{\"signed_up_timestamp\": " + value + ", \"last_logged_in_timestamp\": " + value + "}";
				default:
					throw new NotImplementedException($"Type {typeof(T).Name} is not implemented.");
			}
		}

		private static void AssertType(T response, long expected)
		{
			if (response is ScoreInternal scoreInternal)
			{
				Assert.That(scoreInternal.storedTimestamp, Is.EqualTo(expected));
				return;
			}

			if (response is FetchTimeResponse fetchTimeResponse)
			{
				Assert.That(fetchTimeResponse.timestamp, Is.EqualTo(expected));
				return;
			}

			if (response is User user)
			{
				Assert.That(user.signedUpTimestamp, Is.EqualTo(expected));
				Assert.That(user.lastLoggedInTimestamp, Is.EqualTo(expected));
				return;
			}

			throw new NotImplementedException($"Type {typeof(T).Name} is not implemented.");
		}

		private static object[] GetRandomLong(bool asString, bool positive)
		{
			long value = randomizer.Long(1);
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