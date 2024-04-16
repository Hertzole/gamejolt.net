using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests.ToString
{
	public sealed class Sessions : BaseToStringTest
	{
		[Test]
		public void SessionResponse()
		{
			bool success = faker.Random.Bool();
			string message = faker.Lorem.Sentence();

			SessionResponse response = new SessionResponse(success, message);

			Assert.That(response.ToString(), Is.EqualTo($"{nameof(SessionResponse)} (Success: {success}, Message: {message})"));
		}
	}
}