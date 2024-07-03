#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests.ToString
{
	public sealed class Time : BaseToStringTest
	{
		[Test]
		public void FetchTimeResponse()
		{
			long timestamp = faker.Random.Long();
			string timezone = faker.Random.Utf16String();
			int year = faker.Random.Int();
			int month = faker.Random.Int();
			int day = faker.Random.Int();
			int hour = faker.Random.Int();
			int minute = faker.Random.Int();
			int second = faker.Random.Int();
			bool success = faker.Random.Bool();
			string message = faker.Lorem.Sentence();

			FetchTimeResponse response = new FetchTimeResponse(timestamp, timezone, year, month, day, hour, minute, second, success, message);

			Assert.That(response.ToString(),
				Is.EqualTo(
					$"{nameof(FetchTimeResponse)} (Success: {success}, Message: {message}, timestamp: {timestamp}, timezone: {timezone}, year: {year}, month: {month}, day: {day}, hour: {hour}, minute: {minute}, second: {second})"));
		}
	}
}
#endif // DISABLE_GAMEJOLT