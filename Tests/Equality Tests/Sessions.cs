using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests.Equality
{
	public sealed class Sessions : EqualityTest
	{
		[Test]
		public void SessionResponse()
		{
			TestEquality((a, b) => a == b, (a, b) => a != b,
				new SessionResponse(true, "message"),
				new SessionResponse(false, "message"),
				new SessionResponse(true, "message2"));
		}
	}
}