using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests.Exceptions
{
	[Parallelizable(ParallelScope.All)]
	public sealed class Global : BaseExceptionTest
	{
		[Test]
		public void UnknownError()
		{
			AssertError<GameJoltException>("Unknown fatal error occurred.");
		}

		[Test]
		public void NoSignature()
		{
			AssertError<GameJoltException>("You must enter a signature with your request.");
		}
		
		[Test]
		public void InvalidSignature()
		{
			AssertError<GameJoltException>("The signature you entered for the request is invalid.");
		}

		[Test]
		public void InvalidGameId()
		{
			AssertError<GameJoltInvalidGameException>("The game ID you passed in does not point to a valid game.");
		}
	}
}