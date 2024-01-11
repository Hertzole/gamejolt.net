using System;
using System.Threading.Tasks;
using Hertzole.GameJolt;
using NSubstitute;
using NUnit.Framework;

namespace GameJolt.NET.Tests
{
	public class TimeTest : BaseTest
	{
		[Test]
		public async Task Fetch_Success()
		{
			DateTime time = DummyData.faker.Date.Recent().ToUniversalTime();

			GameJoltAPI.webClient.GetStringAsync("", default)
			           .ReturnsForAnyArgs(_ => FromResult(serializer.Serialize(new FetchTimeResponse(true, null, time))));

			GameJoltResult<DateTime> result = await GameJoltAPI.Time.GetTimeAsync();

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			// Create a new DateTime, or else it will fail because of the milliseconds.
			Assert.That(result.Value, Is.EqualTo(new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, time.Second, DateTimeKind.Utc)));
		}

		[Test]
		public async Task Fetch_ValidUrl()
		{
			await TestUrlAsync(() => GameJoltAPI.Time.GetTimeAsync(), url => 
			{
				Assert.That(url, Does.StartWith(GameJoltUrlBuilder.BASE_URL + GameJoltTime.ENDPOINT));
			});
		}
	}
}