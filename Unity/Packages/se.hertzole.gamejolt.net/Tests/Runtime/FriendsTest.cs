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
				return FromResult(serializer.SerializeResponse(new FetchFriendsResponse(true, null, new[]
				{
					new FriendId(0),
					new FriendId(1)
				})));
			});

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

			GameJoltAPI.webClient.GetStringAsync("", default)
			           .ReturnsForAnyArgs(_ => FromResult(serializer.SerializeResponse(new FetchFriendsResponse(true, null, null))));

			GameJoltResult<int[]> result = await GameJoltAPI.Friends.GetFriendsAsync();

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			Assert.That(result.Value, Is.Not.Null);
			Assert.That(result.Value, Is.Empty);
		}

		[Test]
		public async Task Fetch_NotAuthenticated_Fail()
		{
			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(_ =>
			{
				return FromResult(serializer.SerializeResponse(new FetchFriendsResponse(false, null, new[]
				{
					new FriendId(0),
					new FriendId(1)
				})));
			});

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

			await AssertErrorAsync<FetchFriendsResponse, int[], GameJoltException>(CreateResponse, CallMethod, GameJoltException.UnknownFatalError.Message);

			return;

			FetchFriendsResponse CreateResponse()
			{
				return new FetchFriendsResponse(false, GameJoltException.UnknownFatalError.Message, null);
			}

			Task<GameJoltResult<int[]>> CallMethod()
			{
				return GameJoltAPI.Friends.GetFriendsAsync();
			}
		}
	}
}