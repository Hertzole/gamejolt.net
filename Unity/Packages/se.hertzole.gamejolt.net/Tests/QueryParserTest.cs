using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests
{
	public class QueryParserTest
	{
		[Test]
		public void NoQuery_ReturnsFalse()
		{
			string url = "https://gamejolt.com/games/MyGame/123456";
			bool result = QueryParser.TryGetToken(url, "token", out _);

			Assert.That(result, Is.False);
		}

		[Test]
		[TestCase("https://gamejolt.com/games/MyGame/123456?test=123", "token")]
		[TestCase("https://gamejolt.com/games/MyGame/123456?test=123", "token=")]
		[TestCase("https://gamejolt.com/games/MyGame/123456?test=123&test2=456", "token")]
		[TestCase("https://gamejolt.com/games/MyGame/123456?test=123&test2=456", "token=")]
		public void Query_NoToken_ReturnsFalse(string url, string token)
		{
			bool result = QueryParser.TryGetToken(url, token, out _);

			Assert.That(result, Is.False);
		}

		[Test]
		[TestCase("https://gamejolt.com/games/MyGame/123456?token=123", "token")]
		[TestCase("https://gamejolt.com/games/MyGame/123456?token=123", "token=")]
		[TestCase("https://gamejolt.com/games/MyGame/123456?token=123&token2=456", "token")]
		[TestCase("https://gamejolt.com/games/MyGame/123456?token=123&token2=456", "token=")]
		[TestCase("https://gamejolt.com/games/MyGame/123456?token2=456&token=123&", "token")]
		[TestCase("https://gamejolt.com/games/MyGame/123456?token2=456&token=123&", "token=")]
		public void Query_HasToken_ReturnsTrue(string url, string token)
		{
			bool result = QueryParser.TryGetToken(url, token, out _);

			Assert.That(result, Is.True);
		}

		[Test]
		[TestCase("https://gamejolt.com/games/MyGame/123456?token=123", "token", "123")]
		[TestCase("https://gamejolt.com/games/MyGame/123456?token=123", "token=", "123")]
		[TestCase("https://gamejolt.com/games/MyGame/123456?token=123&token2=456", "token", "123")]
		[TestCase("https://gamejolt.com/games/MyGame/123456?token=123&token2=456", "token=", "123")]
		[TestCase("https://gamejolt.com/games/MyGame/123456?token2=456&token=123", "token", "123")]
		[TestCase("https://gamejolt.com/games/MyGame/123456?token2=456&token=123", "token=", "123")]
		public void Query_HasToken_ReturnsTokenValue(string url, string token, string expected)
		{
			bool result = QueryParser.TryGetToken(url, "token", out string? resultToken);

			Assert.That(result, Is.True);
			Assert.That(resultToken, Is.EqualTo(expected));
		}
	}
}