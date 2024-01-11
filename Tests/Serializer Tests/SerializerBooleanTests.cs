#if NET6_0_OR_GREATER
using JsonException = System.Text.Json.JsonException;
#else
using JsonException = Newtonsoft.Json.JsonSerializationException;
#endif
using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests
{
	[TestFixture(typeof(GetDataResponse))]
	[TestFixture(typeof(GetKeysResponse))]
	[TestFixture(typeof(StoreDataResponse))]
	[TestFixture(typeof(UpdateDataResponse))]
	[TestFixture(typeof(FetchFriendsResponse))]
	[TestFixture(typeof(GetScoreRankResponse))]
	[TestFixture(typeof(GetScoresResponse))]
	[TestFixture(typeof(GetTablesResponse))]
	[TestFixture(typeof(SubmitScoreResponse))]
	[TestFixture(typeof(SessionResponse))]
	[TestFixture(typeof(FetchTimeResponse))]
	[TestFixture(typeof(FetchTrophiesResponse))]
	[TestFixture(typeof(TrophyResponse))]
	[TestFixture(typeof(AuthResponse))]
	[TestFixture(typeof(UsersFetchResponse))]
	internal class SerializerBooleanTests<T> where T : IResponse
	{
		private static object[] testCases =
		{
			new object[] { "true", true },
			new object[] { "false", false },
			new object[] { "0", false },
			new object[] { "1", true },
			new object[] { "\"true\"", true },
			new object[] { "\"false\"", false },
			new object[] { "\"0\"", false },
			new object[] { "\"1\"", true },
			new object[] { "\"True\"", true },
			new object[] { "\"False\"", false },
			new object[] { "\"TRUE\"", true },
			new object[] { "\"FALSE\"", false },
			new object[] { "\"tRuE\"", true },
			new object[] { "\"fAlSe\"", false },
			new object[] { "\"yes\"", true },
			new object[] { "\"no\"", false },
			new object[] { "\"YES\"", true },
			new object[] { "\"NO\"", false },
			new object[] { "\"Yes\"", true },
			new object[] { "\"No\"", false },
			new object[] { "\"yEs\"", true },
			new object[] { "\"nO\"", false }
		};

		[Test]
		[TestCaseSource(nameof(testCases))]
		public void CanDeserialize(string boolean, bool expected)
		{
			T response = GameJoltAPI.serializer.Deserialize<T>("{\"response\": {\"success\": " + boolean + "}}");

			Assert.That(response.Success, Is.EqualTo(expected));
		}

		[Test]
		public void EmptyString_ThrowsException()
		{
			Assert.Throws<JsonException>(() => GameJoltAPI.serializer.Deserialize<T>("{\"response\": {\"success\": \"\"}}"));
		}

		[Test]
		public void InvalidString_ThrowsException()
		{
			Assert.Throws<JsonException>(() => GameJoltAPI.serializer.Deserialize<T>("{\"response\": {\"success\": \"test\"}}"));
		}

		[Test]
		public void InvalidToken_ThrowsException()
		{
			Assert.Throws<JsonException>(() => GameJoltAPI.serializer.Deserialize<T>("{\"response\": {\"success\": []}}"));
		}
	}
}