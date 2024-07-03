#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

using System;
using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests.Equality
{
	public sealed class Friends : EqualityTest
	{
		[Test]
		public void FetchFriendsResponse()
		{
			TestEquality((a, b) => a == b, (a, b) => a != b,
				new FetchFriendsResponse(true, "message", Array.Empty<FriendId>()),
				new FetchFriendsResponse(false, "message", Array.Empty<FriendId>()),
				new FetchFriendsResponse(true, "message2", Array.Empty<FriendId>()),
				new FetchFriendsResponse(true, "message", new[] { new FriendId(0) }),
				new FetchFriendsResponse(true, "message", new[] { new FriendId(0), new FriendId(1) }));
			
			AssertNotEqualObject<FetchFriendsResponse>();
		}

		[Test]
		public void FriendId()
		{
			TestEquality((a, b) => a == b, (a, b) => a != b,
				new FriendId(0),
				new FriendId(1));
			
			AssertNotEqualObject<FriendId>();
		}
	}
}
#endif // DISABLE_GAMEJOLT