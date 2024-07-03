#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests.Equality
{
	public class GameJoltResponse : EqualityTest
	{
		[Test]
		public void Test()
		{
			TestEquality((a, b) => a == b, (a, b) => a != b,
				new GameJoltResponse<DataKey>(new DataKey("key")),
				new GameJoltResponse<DataKey>(new DataKey("key2")));
		}
	}
}
#endif // DISABLE_GAMEJOLT