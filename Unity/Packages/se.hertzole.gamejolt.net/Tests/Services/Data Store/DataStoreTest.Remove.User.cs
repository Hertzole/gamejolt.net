using System.Threading.Tasks;
using Hertzole.GameJolt;
using NSubstitute;
using NUnit.Framework;

namespace GameJolt.NET.Tests
{
	partial class DataStoreTest
	{
		[Test]
		public async Task RemoveUser_Authenticated_Success()
		{
			await AuthenticateAsync();

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string json = serializer.SerializeResponse(new Response(true, null));

				return FromResult(json);
			});

			GameJoltResult result = await GameJoltAPI.DataStore.RemoveAsCurrentUserAsync("key");

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
		}

		[Test]
		public async Task RemoveUser_NotAuthenticated_Fail()
		{
			GameJoltResult result = await GameJoltAPI.DataStore.RemoveAsCurrentUserAsync("key");

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception, Is.TypeOf<GameJoltAuthorizedException>());
		}

		[Test]
		public async Task RemoveUser_Error_Fail()
		{
			await AuthenticateAsync();
			await AssertErrorAsync<Response, GameJoltInvalidDataStoreKeyException>(CreateResponse, GetResult,
				GameJoltInvalidDataStoreKeyException.NO_KEY_MESSAGE);

			return;

			Response CreateResponse()
			{
				return new Response(false, GameJoltInvalidDataStoreKeyException.NO_KEY_MESSAGE);
			}

			Task<GameJoltResult> GetResult()
			{
				return GameJoltAPI.DataStore.RemoveAsCurrentUserAsync("key");
			}
		}
		
		[Test]
		public async Task RemoveAsCurrentUser_ValidUrl()
		{
			await AuthenticateAsync();

			await TestUrlAsync(() => GameJoltAPI.DataStore.RemoveAsCurrentUserAsync("Key"), url =>
			{
				Assert.That(url, Does.StartWith(
					$"{GameJoltUrlBuilder.BASE_URL}{GameJoltDataStore.REMOVE_ENDPOINT}?key=Key&username={Username}&user_token={Token}"));
			});
		}
	}
}