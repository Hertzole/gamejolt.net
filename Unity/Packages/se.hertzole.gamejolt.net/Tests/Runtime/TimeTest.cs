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
			           .ReturnsForAnyArgs(_ => FromResult(serializer.SerializeResponse(new FetchTimeResponse(true, null, time))));

			GameJoltResult<DateTime> result = await GameJoltAPI.Time.GetTimeAsync();

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			// Create a new DateTime, or else it will fail because of the milliseconds.
			Assert.That(result.Value, Is.EqualTo(new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, time.Second, DateTimeKind.Utc)));
		}

		[Test]
		public async Task Fetch_Error()
		{
			GameJoltAPI.webClient.GetStringAsync("", default)
			           .ReturnsForAnyArgs(_ => FromResult(serializer.SerializeResponse(new FetchTimeResponse(false, "Internal error", DateTime.Now))));

			GameJoltResult<DateTime> result = await GameJoltAPI.Time.GetTimeAsync();

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception.Message, Is.EqualTo("Internal error"));
			Assert.That(result.Exception, Is.TypeOf<GameJoltException>());
		}

		[Test]
		public async Task Fetch_ValidUrl()
		{
			await TestUrlAsync(() => GameJoltAPI.Time.GetTimeAsync(),
				url => { Assert.That(url, Does.StartWith(GameJoltUrlBuilder.BASE_URL + GameJoltTime.ENDPOINT)); });
		}

		[Test]
		public void CorrectTimeZone()
		{
			TimeZoneInfo timeZone = GameJoltAPI.Time.TimeZone;

			Assert.That(timeZone, Is.Not.Null);
			Assert.That(timeZone.Id, Is.EqualTo("America/New_York"));
			Assert.That(timeZone.BaseUtcOffset, Is.EqualTo(TimeSpan.FromHours(-5)));
			Assert.That(timeZone.DisplayName, Is.EqualTo("Eastern Standard Time"));
		}
	}
}