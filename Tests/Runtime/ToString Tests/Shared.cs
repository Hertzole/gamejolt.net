#nullable enable

using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests.ToString
{
	public sealed class Shared : BaseToStringTest
	{
		[Test]
		public void Response([Values] bool nullMessage, [Values] bool success)
		{
			string? message = nullMessage ? null : faker.Lorem.Sentence();

			Response response = new Response(success, message);

			Assert.That(response.ToString(), Is.EqualTo($"{nameof(Hertzole.GameJolt.Response)} (Success: {success}, Message: {message})"));
		}
	}
}