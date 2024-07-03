#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

#if NET6_0_OR_GREATER
using JsonException = System.Text.Json.JsonException;
#else
using JsonException = Newtonsoft.Json.JsonSerializationException;
#endif
using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests
{
	public class SerializerBoolOrDateTests
	{
		private static object[] testCases =
		{
			new object[] { "true", true },
			new object[] { "false", false },
			new object[] { "1", true },
			new object[] { "0", false },
			new object[] { "\"true\"", true },
			new object[] { "\"false\"", false },
			new object[] { "\"True\"", true },
			new object[] { "\"False\"", false },
			new object[] { "\"TRUE\"", true },
			new object[] { "\"FALSE\"", false },
			new object[] { "\"1\"", true },
			new object[] { "\"0\"", false },
			new object[] { "\"9 months ago\"", true },
			new object[] { 0, false },
			new object[] { 1, true }
		};

		[Test]
		[TestCaseSource(nameof(testCases))]
		public void CanDeserialize(object input, bool expected)
		{
			FetchTrophiesResponse response =
				GameJoltAPI.serializer.DeserializeResponse<FetchTrophiesResponse>("{\"response\": {\"trophies\": [{\"achieved\":" + input + "}]}}");

			Assert.That(response.trophies[0].achieved, Is.EqualTo(expected));
		}

		[Test]
		public void EmptyString_ThrowsException()
		{
			Assert.Throws<JsonException>(() =>
				GameJoltAPI.serializer.DeserializeResponse<FetchTrophiesResponse>("{\"response\": {\"trophies\": [{\"achieved\":\"\"}]}}"));
		}

		[Test]
		public void InvalidToken_ThrowsException()
		{
			Assert.Throws<JsonException>(() =>
				GameJoltAPI.serializer.DeserializeResponse<FetchTrophiesResponse>("{\"response\": {\"trophies\": [{\"achieved\":null}]}}"));
		}

		[Test]
		public void InvalidInteger_ThrowsException()
		{
			Assert.Throws<JsonException>(() =>
				GameJoltAPI.serializer.DeserializeResponse<FetchTrophiesResponse>("{\"response\": {\"trophies\": [{\"achieved\":2}]}}"));
		}
	}
}
#endif // DISABLE_GAMEJOLT