#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

#if !NET6_0_OR_GREATER
using System;
using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests
{
	public class ResponseSerializerTests
	{
		[Test]
		public void InvalidResponseType_ThrowsException()
		{
			Assert.Throws<NotSupportedException>(() => GameJoltAPI.serializer.DeserializeResponse<Response>("{ \"success\": true, \"message\": \"Success!\", \"notsupported\": 1 }"));
		}
	}
}
#endif
#endif // DISABLE_GAMEJOLT