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
			DateTime time = faker.Date.Recent().ToUniversalTime();

			GameJoltAPI.webClient.GetStringAsync("", default)
			           .ReturnsForAnyArgs(_ => FromResult(serializer.Serialize(new FetchTimeResponse(true, null, time))));

			GameJoltResult<DateTime> result = await GameJoltAPI.Time.GetTimeAsync();

			Assert.IsFalse(result.HasError);
			Assert.IsNull(result.Exception);
			// Create a new DateTime, or else it will fail because of the milliseconds.
			Assert.That(result.Value, Is.EqualTo(new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, time.Second, DateTimeKind.Utc)));
		}
	}
}