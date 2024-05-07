using System;
using System.Threading.Tasks;
using Hertzole.GameJolt;
using NSubstitute;
using NUnit.Framework;

namespace GameJolt.NET.Tests
{
	partial class DataStoreTest
	{
		[Test]
		public async Task GetValueUser_Authenticated_String_Success()
		{
			await AuthenticateAsync();

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string json = serializer.SerializeResponse(new GetDataResponse(true, null, "value"));

				return FromResult(json);
			});

			GameJoltResult<string> result = await GameJoltAPI.DataStore.GetValueAsStringAsCurrentUserAsync("key");

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			Assert.That(result.Value, Is.EqualTo("value"));
		}

		[Test]
		public async Task GetValueUser_Authenticated_String_Error_Fail()
		{
			await AuthenticateAsync();
			await AssertErrorAsync<GetDataResponse, string, GameJoltInvalidDataStoreKeyException>(CreateResponse, GetResult,
				GameJoltInvalidDataStoreKeyException.NO_KEY_MESSAGE);

			return;

			GetDataResponse CreateResponse()
			{
				return new GetDataResponse(false, GameJoltInvalidDataStoreKeyException.NO_KEY_MESSAGE, null);
			}

			Task<GameJoltResult<string>> GetResult()
			{
				return GameJoltAPI.DataStore.GetValueAsStringAsCurrentUserAsync("key");
			}
		}

		[Test]
		public async Task GetValueUser_Authenticated_Int_Success()
		{
			await AuthenticateAsync();

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string json = serializer.SerializeResponse(new GetDataResponse(true, null, "1"));

				return FromResult(json);
			});

			GameJoltResult<int> result = await GameJoltAPI.DataStore.GetValueAsIntAsCurrentUserAsync("key");

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			Assert.That(result.Value, Is.EqualTo(1));
		}

		[Test]
		public async Task GetValueUser_Authenticated_Int_InvalidValue_Fail()
		{
			await AuthenticateAsync();

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string json = serializer.SerializeResponse(new GetDataResponse(true, null, "invalid value"));

				return FromResult(json);
			});

			GameJoltResult<int> result = await GameJoltAPI.DataStore.GetValueAsIntAsCurrentUserAsync("key");

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception, Is.TypeOf<GameJoltInvalidDataStoreValueException>());
		}

		[Test]
		public async Task GetValueUser_Authenticated_Int_Error_Fail()
		{
			await AuthenticateAsync();
			await AssertErrorAsync<GetDataResponse, int, GameJoltInvalidDataStoreKeyException>(CreateResponse, GetResult,
				GameJoltInvalidDataStoreKeyException.NO_KEY_MESSAGE);

			return;

			GetDataResponse CreateResponse()
			{
				return new GetDataResponse(false, GameJoltInvalidDataStoreKeyException.NO_KEY_MESSAGE, null);
			}

			Task<GameJoltResult<int>> GetResult()
			{
				return GameJoltAPI.DataStore.GetValueAsIntAsCurrentUserAsync("key");
			}
		}

		[Test]
		public async Task GetValueUser_Authenticated_Bytes_Success()
		{
			await AuthenticateAsync();

			byte[] bytes = DummyData.Bytes();

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string json = serializer.SerializeResponse(new GetDataResponse(true, null, Convert.ToBase64String(bytes)));

				return FromResult(json);
			});

			GameJoltResult<byte[]> result = await GameJoltAPI.DataStore.GetValueAsBytesAsCurrentUserAsync("key");

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			Assert.That(result.Value, Is.EqualTo(bytes));
		}

		[Test]
		public async Task GetValueUser_Authenticated_Bytes_InvalidValue_Fail()
		{
			await AuthenticateAsync();

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string json = serializer.SerializeResponse(new GetDataResponse(true, null, "VeryInvalidValue12345"));

				return FromResult(json);
			});

			GameJoltResult<byte[]> result = await GameJoltAPI.DataStore.GetValueAsBytesAsCurrentUserAsync("key");

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception, Is.TypeOf<FormatException>());
		}

		[Test]
		public async Task GetValueUser_Authenticated_Bytes_EmptyValue_Success()
		{
			await AuthenticateAsync();

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string json = serializer.SerializeResponse(new GetDataResponse(true, null, string.Empty));

				return FromResult(json);
			});

			GameJoltResult<byte[]> result = await GameJoltAPI.DataStore.GetValueAsBytesAsCurrentUserAsync("key");

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			Assert.That(result.Value, Is.Empty);
		}

		[Test]
		public async Task GetValueUser_Authenticated_Bytes_Error_Fail()
		{
			await AuthenticateAsync();
			await AssertErrorAsync<GetDataResponse, byte[], GameJoltInvalidDataStoreKeyException>(CreateResponse, GetResult,
				GameJoltInvalidDataStoreKeyException.NO_KEY_MESSAGE);

			return;

			GetDataResponse CreateResponse()
			{
				return new GetDataResponse(false, GameJoltInvalidDataStoreKeyException.NO_KEY_MESSAGE, null);
			}

			Task<GameJoltResult<byte[]>> GetResult()
			{
				return GameJoltAPI.DataStore.GetValueAsBytesAsCurrentUserAsync("key");
			}
		}

		[Test]
		public async Task GetValueUser_Authenticated_Bool_Success()
		{
			await AuthenticateAsync();

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string json = serializer.SerializeResponse(new GetDataResponse(true, null, "true"));

				return FromResult(json);
			});

			GameJoltResult<bool> result = await GameJoltAPI.DataStore.GetValueAsBoolAsCurrentUserAsync("key");

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			Assert.That(result.Value, Is.True);
		}

		[Test]
		public async Task GetValueUser_Authenticated_Bool_InvalidValue_Fail()
		{
			await AuthenticateAsync();

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string json = serializer.SerializeResponse(new GetDataResponse(true, null, "invalid value"));

				return FromResult(json);
			});

			GameJoltResult<bool> result = await GameJoltAPI.DataStore.GetValueAsBoolAsCurrentUserAsync("key");

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception, Is.TypeOf<GameJoltInvalidDataStoreValueException>());
		}

		[Test]
		public async Task GetValueUser_Authenticated_Bool_Error_Fail()
		{
			await AuthenticateAsync();
			await AssertErrorAsync<GetDataResponse, bool, GameJoltInvalidDataStoreKeyException>(CreateResponse, GetResult,
				GameJoltInvalidDataStoreKeyException.NO_KEY_MESSAGE);

			return;

			GetDataResponse CreateResponse()
			{
				return new GetDataResponse(false, GameJoltInvalidDataStoreKeyException.NO_KEY_MESSAGE, null);
			}

			Task<GameJoltResult<bool>> GetResult()
			{
				return GameJoltAPI.DataStore.GetValueAsBoolAsCurrentUserAsync("key");
			}
		}

		[Test]
		public async Task GetValueUser_NotAuthenticated_String_Fail()
		{
			GameJoltResult<string> result = await GameJoltAPI.DataStore.GetValueAsStringAsCurrentUserAsync("key");

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception, Is.TypeOf<GameJoltAuthorizedException>());
		}

		[Test]
		public async Task GetValueUser_NotAuthenticated_Int_Fail()
		{
			GameJoltResult<int> result = await GameJoltAPI.DataStore.GetValueAsIntAsCurrentUserAsync("key");

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception, Is.TypeOf<GameJoltAuthorizedException>());
		}

		[Test]
		public async Task GetValueUser_NotAuthenticated_Bytes_Fail()
		{
			GameJoltResult<byte[]> result = await GameJoltAPI.DataStore.GetValueAsBytesAsCurrentUserAsync("key");

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception, Is.TypeOf<GameJoltAuthorizedException>());
		}

		[Test]
		public async Task GetValueUser_NotAuthenticated_Bool_Fail()
		{
			GameJoltResult<bool> result = await GameJoltAPI.DataStore.GetValueAsBoolAsCurrentUserAsync("key");

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception, Is.TypeOf<GameJoltAuthorizedException>());
		}
		
		[Test]
		public async Task GetValueAsCurrentUserAsync_String_ValidUrl()
		{
			await AuthenticateAsync();

			await TestUrlAsync(() => GameJoltAPI.DataStore.GetValueAsStringAsCurrentUserAsync("Key"),
				url =>
				{
					Assert.That(url,
						Does.StartWith($"{GameJoltUrlBuilder.BASE_URL}{GameJoltDataStore.FETCH_ENDPOINT}?key=Key&username={Username}&user_token={Token}"));
				});
		}

		[Test]
		public async Task GetValueAsCurrentUserAsync_Int_ValidUrl()
		{
			await AuthenticateAsync();

			await TestUrlAsync(() => GameJoltAPI.DataStore.GetValueAsIntAsCurrentUserAsync("Key"),
				url =>
				{
					Assert.That(url,
						Does.StartWith($"{GameJoltUrlBuilder.BASE_URL}{GameJoltDataStore.FETCH_ENDPOINT}?key=Key&username={Username}&user_token={Token}"));
				});
		}

		[Test]
		public async Task GetValueAsCurrentUserAsync_Bytes_ValidUrl()
		{
			await AuthenticateAsync();

			await TestUrlAsync(() => GameJoltAPI.DataStore.GetValueAsBytesAsCurrentUserAsync("Key"),
				url =>
				{
					Assert.That(url,
						Does.StartWith($"{GameJoltUrlBuilder.BASE_URL}{GameJoltDataStore.FETCH_ENDPOINT}?key=Key&username={Username}&user_token={Token}"));
				});
		}

		[Test]
		public async Task GetValueAsCurrentUserAsync_Bool_ValidUrl()
		{
			await AuthenticateAsync();

			await TestUrlAsync(() => GameJoltAPI.DataStore.GetValueAsBoolAsCurrentUserAsync("Key"),
				url =>
				{
					Assert.That(url,
						Does.StartWith($"{GameJoltUrlBuilder.BASE_URL}{GameJoltDataStore.FETCH_ENDPOINT}?key=Key&username={Username}&user_token={Token}"));
				});
		}
	}
}