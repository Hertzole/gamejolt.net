#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests.Equality
{
	public sealed class Shared : EqualityTest
	{
		[Test]
		public void Response()
		{
			TestEquality((a, b) => a == b, (a, b) => a != b,
				new Response(true, "message"),
				new Response(false, "message"),
				new Response(true, "message2"));
			
			AssertNotEqualObject<Response>();
		}
	}
}
#endif // DISABLE_GAMEJOLT