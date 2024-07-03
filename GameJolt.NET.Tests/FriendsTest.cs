#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

using System.Threading.Tasks;
using Hertzole.GameJolt;
using NSubstitute;
using NUnit.Framework;

namespace GameJolt.NET.Tests
{
	public sealed class FriendsTest : BaseTest
	{
		private static readonly FriendId[] friends =
		{
			new FriendId(0),
			new FriendId(1)
		};

		[Test]
		public async Task Fetch_Authenticated_Success()
		{
			await AuthenticateAsync();

			FetchFriendsResponse expectedResponse = new FetchFriendsResponse(true, null, friends);
			string expectedJson = serializer.SerializeResponse(expectedResponse);

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(expectedJson);

			GameJoltResult<int[]> result = await GameJoltAPI.Friends.GetFriendsAsync();

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			Assert.That(result.Value, Is.Not.Null);
			Assert.That(result.Value, Has.Length.EqualTo(2));
			Assert.That(result.Value[0], Is.EqualTo(0));
			Assert.That(result.Value[1], Is.EqualTo(1));
		}

		[Test]
		public async Task Fetch_Authenticated_Success_NoFriends()
		{
			await AuthenticateAsync();

			FetchFriendsResponse expectedResponse = new FetchFriendsResponse(true, null, null);
			string expectedJson = serializer.SerializeResponse(expectedResponse);

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(expectedJson);

			GameJoltResult<int[]> result = await GameJoltAPI.Friends.GetFriendsAsync();

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			Assert.That(result.Value, Is.Not.Null);
			Assert.That(result.Value, Is.Empty);
		}

		[Test]
		public async Task Fetch_NotAuthenticated_Fail()
		{
			FetchFriendsResponse expectedResponse = new FetchFriendsResponse(false, null, friends);
			string expectedJson = serializer.SerializeResponse(expectedResponse);

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(expectedJson);

			GameJoltResult<int[]> result = await GameJoltAPI.Friends.GetFriendsAsync();

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception, Is.TypeOf<GameJoltAuthorizedException>());
			Assert.That(result.Value, Is.Null);
		}

		[Test]
		public async Task Fetch_ValidUrl()
		{
			await AuthenticateAsync();

			await TestUrlAsync(() => GameJoltAPI.Friends.GetFriendsAsync(),
				url =>
				{
					Assert.That(url, Does.StartWith(GameJoltUrlBuilder.BASE_URL + GameJoltFriends.ENDPOINT + $"?username={Username}&user_token={Token}"));
				});
		}

		[Test]
		public async Task Fetch_Error()
		{
			await AuthenticateAsync();

			await AssertErrorAsync<FetchFriendsResponse, int[], GameJoltException>(CreateResponse, CallMethod, GameJoltException.UNKNOWN_FATAL_ERROR);

			return;

			FetchFriendsResponse CreateResponse()
			{
				return new FetchFriendsResponse(false, GameJoltException.UNKNOWN_FATAL_ERROR, null);
			}

			Task<GameJoltResult<int[]>> CallMethod()
			{
				return GameJoltAPI.Friends.GetFriendsAsync();
			}
		}
	}
}
#endif // DISABLE_GAMEJOLT