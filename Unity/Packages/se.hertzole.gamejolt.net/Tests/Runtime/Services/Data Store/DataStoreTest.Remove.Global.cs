using System.Threading.Tasks;
using Hertzole.GameJolt;
using NSubstitute;
using NUnit.Framework;

namespace GameJolt.NET.Tests
{
	partial class DataStoreTest
	{
		[Test]
		public async Task RemoveGlobal_Success()
		{
			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string json = serializer.SerializeResponse(new Response(true, null));

				return FromResult(json);
			});

			GameJoltResult result = await GameJoltAPI.DataStore.RemoveAsync("key");

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
		}

		[Test]
		public async Task RemoveGlobal_Error_Fail()
		{
			await AssertErrorAsync<Response, GameJoltInvalidDataStoreKeyException>(CreateResponse, GetResult,
				GameJoltInvalidDataStoreKeyException.NO_KEY_MESSAGE);

			return;

			Response CreateResponse()
			{
				return new Response(false, GameJoltInvalidDataStoreKeyException.NO_KEY_MESSAGE);
			}

			Task<GameJoltResult> GetResult()
			{
				return GameJoltAPI.DataStore.RemoveAsync("key");
			}
		}
		
		[Test]
		public async Task Remove_ValidUrl()
		{
			await TestUrlAsync(() => GameJoltAPI.DataStore.RemoveAsync("Key"),
				url => { Assert.That(url, Does.StartWith($"{GameJoltUrlBuilder.BASE_URL}{GameJoltDataStore.REMOVE_ENDPOINT}?key=Key")); });
		}
	}
}