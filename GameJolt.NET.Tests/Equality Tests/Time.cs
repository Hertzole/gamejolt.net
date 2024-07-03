#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests.Equality
{
	public class Time : EqualityTest
	{
		[Test]
		public void FetchTimeResponse()
		{
			TestEquality((a, b) => a == b, (a, b) => a != b,
				new FetchTimeResponse(0, "timezone", 1, 2, 3, 4, 5, 6, true, "message"),
				new FetchTimeResponse(1, "timezone", 1, 2, 3, 4, 5, 6, true, "message"),
				new FetchTimeResponse(0, "timezone2", 1, 2, 3, 4, 5, 6, true, "message"),
				new FetchTimeResponse(0, "timezone", 2, 2, 3, 4, 5, 6, true, "message"),
				new FetchTimeResponse(0, "timezone", 1, 3, 3, 4, 5, 6, true, "message"),
				new FetchTimeResponse(0, "timezone", 1, 2, 4, 4, 5, 6, true, "message"),
				new FetchTimeResponse(0, "timezone", 1, 2, 3, 5, 5, 6, true, "message"),
				new FetchTimeResponse(0, "timezone", 1, 2, 3, 4, 6, 6, true, "message"),
				new FetchTimeResponse(0, "timezone", 1, 2, 3, 4, 5, 7, true, "message"),
				new FetchTimeResponse(0, "timezone", 1, 2, 3, 4, 5, 6, false, "message"),
				new FetchTimeResponse(0, "timezone", 1, 2, 3, 4, 5, 6, true, "message2"));
			
			AssertNotEqualObject<FetchTimeResponse>();
		}
	}
}
#endif // DISABLE_GAMEJOLT