#nullable enable

using System.Threading.Tasks;
using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests
{
	public class WebTest
	{
		[Test]
		public async Task SendRequest()
		{
			string? str = await GameJoltAPI.webClient.GetStringAsync("https://api.gamejolt.com/api/game/v1_2/time", default);
			
			Assert.That(str, Is.Not.Null);
		}
	}
}