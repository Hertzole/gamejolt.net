using Hertzole.GameJolt;
using NUnit.Framework;
#if NET6_0_OR_GREATER
using System.Text.Json;

#else
using Newtonsoft.Json;
#endif

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
			new object[] { "\"9 months ago\"", true }
		};

		[Test]
		[TestCaseSource(nameof(testCases))]
		public void CanDeserialize(object input, bool expected)
		{
			FetchTrophiesResponse response =
				GameJoltAPI.serializer.Deserialize<FetchTrophiesResponse>("{\"response\": {\"trophies\": [{\"achieved\":" + input + "}]}}");

			Assert.That(response.trophies[0].achieved, Is.EqualTo(expected));
		}

		[Test]
		public void EmptyString_ThrowsException()
		{
			Assert.Throws<JsonException>(() =>
				GameJoltAPI.serializer.Deserialize<FetchTrophiesResponse>("{\"response\": {\"trophies\": [{\"achieved\":\"\"}]}}"));
		}
		
		[Test]
		public void InvalidToken_ThrowsException()
		{
			Assert.Throws<JsonException>(() =>
				GameJoltAPI.serializer.Deserialize<FetchTrophiesResponse>("{\"response\": {\"trophies\": [{\"achieved\":null}]}}"));
		}
	}
}