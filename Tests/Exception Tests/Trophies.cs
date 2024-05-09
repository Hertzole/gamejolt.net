using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests.Exceptions
{
	public sealed class Trophies : BaseExceptionTest
	{
		[Test]
		public void DoesNotBelongToGame()
		{
			AssertError<GameJoltInvalidTrophyException>(GameJoltInvalidTrophyException.DOES_NOT_BELONG_MESSAGE);
		}
		
		[Test]
		public void IncorrectID()
		{
			AssertError<GameJoltInvalidTrophyException>(GameJoltInvalidTrophyException.INCORRECT_ID_MESSAGE);
		}

		[Test]
		public void AlreadyUnlocked()
		{
			AssertError<GameJoltTrophyException>(GameJoltTrophyException.ALREADY_UNLOCKED_MESSAGE);
		}
		
		[Test]
		public void DoesNotHave()
		{
			AssertError<GameJoltTrophyException>(GameJoltTrophyException.DOES_NOT_HAVE_MESSAGE);
		}
	}
}