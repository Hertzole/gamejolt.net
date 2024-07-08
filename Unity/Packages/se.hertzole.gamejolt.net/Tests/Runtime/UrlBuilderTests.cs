#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

using System.Security.Cryptography;
using System.Text;
using Bogus;
using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests
{
	public sealed class UrlBuilderTests
	{
		private readonly Faker faker = new Faker();
		
		[Test]
		public void EndsWithSlash_WithSlash_ReturnsTrue()
		{
			Assert.That(GameJoltUrlBuilder.EndsWithSlash(new StringBuilder("test/")), Is.True);
		}
		
		[Test]
		public void EndsWithSlash_WithoutSlash_ReturnsFalse()
		{
			Assert.That(GameJoltUrlBuilder.EndsWithSlash(new StringBuilder("test")), Is.False);
		}
		
		[Test]
		public void EndsWithSlash_Empty_ReturnsFalse()
		{
			Assert.That(GameJoltUrlBuilder.EndsWithSlash(new StringBuilder()), Is.False);
		}
		
		[Test]
		public void BuildUrl_ReturnsCorrectUrl([Values] bool endsWithSlash)
		{
			string endpoint = faker.Lorem.Word();

			if (endsWithSlash)
			{
				endpoint += '/';
			}
			
			int gameId = faker.Random.Int();
			string privateKey = faker.Random.String2(10, 20);
			
			GameJoltAPI.Initialize(gameId, privateKey);
			
			StringBuilder builder = new StringBuilder(endpoint);
			string url = GameJoltUrlBuilder.BuildUrl(builder);
			char endChar = endsWithSlash ? '?' : '&';
			
			string baseString = $"https://api.gamejolt.com/api/game/v1_2/{endpoint}{endChar}game_id={gameId}&format=json";
			string withPrivateKey = baseString + privateKey;
			
			string hash = GetHashString(withPrivateKey);
			
			string expected = baseString + "&signature=" + hash;
			
			Assert.That(url, Is.EqualTo(expected));
			
			GameJoltAPI.Shutdown();
		}
		
		private static byte[] GetHash(string input)
		{
#if NET7_0_OR_GREATER
			return MD5.HashData(Encoding.UTF8.GetBytes(input));
#else
			using (MD5 md5 = MD5.Create())
			{
				return md5.ComputeHash(Encoding.UTF8.GetBytes(input));
			}
#endif
		}
		
		private static string GetHashString(string input)
		{
			byte[] hash = GetHash(input);
			StringBuilder builder = new StringBuilder();
			
			foreach (byte b in hash)
			{
				builder.Append(b.ToString("x2"));
			}
			
			return builder.ToString();
		}
	}
}
#endif // DISABLE_GAMEJOLT