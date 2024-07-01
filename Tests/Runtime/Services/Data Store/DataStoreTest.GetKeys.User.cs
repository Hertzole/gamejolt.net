#nullable enable

using System.Threading.Tasks;
using Hertzole.GameJolt;
using NSubstitute;
using NUnit.Framework;

namespace GameJolt.NET.Tests
{
	partial class DataStoreTest
	{
		[Test]
		[TestCase("")]
		[TestCase("*")]
		public async Task GetKeysUser_Authenticated_Success(string pattern)
		{
			await AuthenticateAsync();

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (string.IsNullOrEmpty(pattern))
				{
					Assert.That(arg, Does.StartWith(GameJoltUrlBuilder.BASE_URL + GameJoltDataStore.FETCH_KEYS_ENDPOINT + "?username="));
				}
				else
				{
					Assert.That(arg, Does.StartWith(GameJoltUrlBuilder.BASE_URL + GameJoltDataStore.FETCH_KEYS_ENDPOINT + $"?pattern={pattern}&username="));
				}

				string json = serializer.SerializeResponse(new GetKeysResponse(true, null, new[]
				{
					new DataKey("key1"),
					new DataKey("key2")
				}));

				return FromResult(json);
			});

			GameJoltResult<string[]> result = await GameJoltAPI.DataStore.GetKeysAsCurrentUserAsync(pattern);

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			Assert.That(result.Value, Is.Not.Null);
			Assert.That(result.Value, Has.Length.EqualTo(2));
			Assert.That(result.Value![0], Is.EqualTo("key1"));
			Assert.That(result.Value[1], Is.EqualTo("key2"));
		}

		[Test]
		public async Task GetKeysUser_Authenticated_Success_NoKeys()
		{
			await AuthenticateAsync();

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();
				Assert.That(arg, Does.StartWith(GameJoltUrlBuilder.BASE_URL + GameJoltDataStore.FETCH_KEYS_ENDPOINT + "?username="));

				string json = serializer.SerializeResponse(new GetKeysResponse(true, null, null));

				return FromResult(json);
			});

			GameJoltResult<string[]> result = await GameJoltAPI.DataStore.GetKeysAsCurrentUserAsync();

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			Assert.That(result.Value, Is.Not.Null);
			Assert.That(result.Value, Is.Empty);
		}

		[Test]
		public async Task GetKeysUser_Authenticated_Error_Fail()
		{
			await AuthenticateAsync();
			await AssertErrorAsync<GetKeysResponse, string[], GameJoltException>(CreateResponse, GetResult);
			return;

			GetKeysResponse CreateResponse()
			{
				return new GetKeysResponse(false, GameJoltException.UNKNOWN_FATAL_ERROR, null);
			}

			Task<GameJoltResult<string[]>> GetResult()
			{
				return GameJoltAPI.DataStore.GetKeysAsCurrentUserAsync();
			}
		}

		[Test]
		public async Task GetKeysUser_NotAuthenticated_Fail()
		{
			GameJoltResult<string[]> result = await GameJoltAPI.DataStore.GetKeysAsCurrentUserAsync("");

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception, Is.TypeOf<GameJoltAuthorizedException>());
		}
		
		[Test]
		[TestCase("")]
		[TestCase("*")]
		public async Task GetKeysAsCurrentUserAsync_ValidUrl(string pattern)
		{
			await AuthenticateAsync();

			await TestUrlAsync(() => GameJoltAPI.DataStore.GetKeysAsCurrentUserAsync(pattern),
				url =>
				{
					if (string.IsNullOrEmpty(pattern))
					{
						Assert.That(url,
							Does.StartWith($"{GameJoltUrlBuilder.BASE_URL}{GameJoltDataStore.FETCH_KEYS_ENDPOINT}?username={Username}&user_token={Token}"));
					}
					else
					{
						Assert.That(url,
							Does.StartWith(
								$"{GameJoltUrlBuilder.BASE_URL}{GameJoltDataStore.FETCH_KEYS_ENDPOINT}?pattern={pattern}&username={Username}&user_token={Token}"));
					}
				});
		}
	}
}