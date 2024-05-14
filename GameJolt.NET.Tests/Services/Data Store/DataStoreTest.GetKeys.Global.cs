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
		public async Task GetKeysGlobal_Success(string pattern)
		{
			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (string.IsNullOrEmpty(pattern))
				{
					Assert.That(arg, Does.StartWith(GameJoltUrlBuilder.BASE_URL + GameJoltDataStore.FETCH_KEYS_ENDPOINT));
				}
				else
				{
					Assert.That(arg, Does.StartWith(GameJoltUrlBuilder.BASE_URL + GameJoltDataStore.FETCH_KEYS_ENDPOINT + $"?pattern={pattern}"));
				}

				string json = serializer.SerializeResponse(new GetKeysResponse(true, null, new[]
				{
					new DataKey("key1"),
					new DataKey("key2")
				}));

				return FromResult(json);
			});

			GameJoltResult<string[]> result = await GameJoltAPI.DataStore.GetKeysAsync(pattern);

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			Assert.That(result.Value, Is.Not.Null);
			Assert.That(result.Value!.Length, Is.EqualTo(2));
			Assert.That(result.Value[0], Is.EqualTo("key1"));
			Assert.That(result.Value[1], Is.EqualTo("key2"));
		}

		[Test]
		public async Task GetKeysGlobal_Success_NoKeys()
		{
			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();
				Assert.That(arg, Does.StartWith(GameJoltUrlBuilder.BASE_URL + GameJoltDataStore.FETCH_KEYS_ENDPOINT));

				string json = serializer.SerializeResponse(new GetKeysResponse(true, null, null));

				return FromResult(json);
			});

			GameJoltResult<string[]> result = await GameJoltAPI.DataStore.GetKeysAsync();

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			Assert.That(result.Value, Is.Not.Null);
			Assert.That(result.Value, Is.Empty);
		}

		[Test]
		public async Task GetKeysGlobal_Error_Fail()
		{
			await AssertErrorAsync<GetKeysResponse, string[], GameJoltException>(CreateResponse, GetResult);
			return;

			GetKeysResponse CreateResponse()
			{
				return new GetKeysResponse(false, GameJoltException.UNKNOWN_FATAL_ERROR, null);
			}

			Task<GameJoltResult<string[]>> GetResult()
			{
				return GameJoltAPI.DataStore.GetKeysAsync();
			}
		}
		
		[Test]
		[TestCase("")]
		[TestCase("*")]
		public async Task GetKeysAsync_ValidUrl(string pattern)
		{
			await TestUrlAsync(() => GameJoltAPI.DataStore.GetKeysAsync(pattern),
				url =>
				{
					if (string.IsNullOrEmpty(pattern))
					{
						Assert.That(url, Does.StartWith($"{GameJoltUrlBuilder.BASE_URL}{GameJoltDataStore.FETCH_KEYS_ENDPOINT}"));
					}
					else
					{
						Assert.That(url, Does.StartWith($"{GameJoltUrlBuilder.BASE_URL}{GameJoltDataStore.FETCH_KEYS_ENDPOINT}?pattern={pattern}"));
					}
				});
		}
	}
}