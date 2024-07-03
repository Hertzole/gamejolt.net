#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests.Exceptions
{
	public sealed class Users : BaseExceptionTest
	{
		[Test]
		public void NoUsernameUserId()
		{
			AssertError<GameJoltException>("You must enter in a user ID or username.");
		}

		[Test]
		public void NoSuchUser()
		{
			AssertError<GameJoltInvalidUserException>(GameJoltInvalidUserException.MESSAGE);
		}
	}
}
#endif // DISABLE_GAMEJOLT