using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests.Exceptions
{
	public sealed class Scores : BaseExceptionTest
	{
		[Test]
		public void InvalidTable()
		{
			AssertError<GameJoltInvalidTableException>(GameJoltInvalidTableException.MESSAGE);
		}

		[Test]
		public void UsernameAndGuestMutual()
		{
			AssertError<GameJoltException>("'username' and 'guest' are mutually exclusive");
		}

		[Test]
		public void BetterThanWorseThanMutual()
		{
			AssertError<GameJoltException>("'better-than' and 'worse-than' are mutually exclusive");
		}

		[Test]
		public void NoGuests()
		{
			AssertError<GameJoltAuthorizedException>("Guests are not allowed to enter scores for this game.");
		}

		[Test]
		public void NoGuestOrUser()
		{
			AssertError<GameJoltException>("You must pass in a user/guest for this score.");
		}

		[Test]
		public void NoScore()
		{
			AssertError<GameJoltException>("You must enter a score.");
		}
		
		[Test]
		public void NoSort()
		{
			AssertError<GameJoltException>("You must enter a sort value for this score, and it must be numeric.");
		}

		[Test]
		public void UnknownError()
		{
			AssertError<GameJoltException>("Unknown error has occured");
		}

		[Test]
		public void CouldNotGetRank()
		{
			AssertError<GameJoltException>("Could not get a rank for the parameters you entered.");
		}
	}
}