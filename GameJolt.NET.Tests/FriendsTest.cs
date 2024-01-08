using System.Threading.Tasks;
using Hertzole.GameJolt;
using NSubstitute;
using NUnit.Framework;

namespace GameJolt.NET.Tests
{
	public class FriendsTest : BaseTest
	{
		[Test]
		public async Task Fetch_Authenticated_Success()
		{
			await AuthenticateAsync();

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(_ =>
			{
				return FromResult(serializer.Serialize(new FetchFriendsResponse(true, null, new[]
				{
					new FriendId(0),
					new FriendId(1)
				})));
			});

			GameJoltResult<int[]> result = await GameJoltAPI.Friends.FetchAsync();

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			Assert.That(result.Value, Is.Not.Null);
			Assert.That(result.Value, Has.Length.EqualTo(2));
			Assert.That(result.Value[0], Is.EqualTo(0));
			Assert.That(result.Value[1], Is.EqualTo(1));
		}

		[Test]
		public async Task Fetch_NotAuthenticated_Fail()
		{
			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(_ =>
			{
				return FromResult(serializer.Serialize(new FetchFriendsResponse(false, null, new[]
				{
					new FriendId(0),
					new FriendId(1)
				})));
			});

			GameJoltResult<int[]> result = await GameJoltAPI.Friends.FetchAsync();

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception, Is.TypeOf<GameJoltAuthorizedException>());
			Assert.That(result.Value, Is.Null);
		}
	}
}