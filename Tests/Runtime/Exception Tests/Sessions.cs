#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests.Exceptions
{
	public sealed class Sessions : BaseExceptionTest
	{
		[Test]
		public void NoOpenSession()
		{
			AssertError<GameJoltSessionException>("Could not find an open session. You must open a new one.");
		}
	}
}
#endif // DISABLE_GAMEJOLT