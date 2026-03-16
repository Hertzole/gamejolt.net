#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GameJolt.NET.Tests.Attributes;
using Hertzole.GameJolt;
using NSubstitute;
using NUnit.Framework;

namespace GameJolt.NET.Tests
{
	partial class DataStoreTest
	{
		[Test]
		[NeedsAuthentication]
		public async Task GetValueUser_Authenticated_String_Success()
		{
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
		[NeedsAuthentication]
		public async Task GetValueUser_Authenticated_String_Error_Fail()
		{
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
		[NeedsAuthentication]
		public async Task GetValueUser_Authenticated_Int_Success()
		{
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
		[NeedsAuthentication]
		public async Task GetValueUser_Authenticated_Int_InvalidValue_Fail()
		{
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
		[NeedsAuthentication]
		public async Task GetValueUser_Authenticated_Int_Error_Fail()
		{
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
		[NeedsAuthentication]
		public async Task GetValueUser_Authenticated_Bytes_Success()
		{
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
		[NeedsAuthentication]
		public async Task GetValueUser_Authenticated_Bytes_InvalidValue_Fail()
		{
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
		[NeedsAuthentication]
		public async Task GetValueUser_Authenticated_Bytes_EmptyValue_Success()
		{
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
		[NeedsAuthentication]
		public async Task GetValueUser_Authenticated_Bytes_Error_Fail()
		{
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
		[NeedsAuthentication]
		public async Task GetValueUser_Authenticated_Bool_Success()
		{
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
		[NeedsAuthentication]
		public async Task GetValueUser_Authenticated_Bool_InvalidValue_Fail()
		{
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
		[NeedsAuthentication]
		public async Task GetValueUser_Authenticated_Bool_Error_Fail()
		{
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
		[NeedsAuthentication]
		public async Task GetValueAsCurrentUserAsync_String_ValidUrl()
		{
			await TestUrlAsync(() => GameJoltAPI.DataStore.GetValueAsStringAsCurrentUserAsync("Key"),
				url =>
				{
					Assert.That(url,
						Does.StartWith($"{GameJoltUrlBuilder.BASE_URL}{GameJoltDataStore.FETCH_ENDPOINT}?key=Key&username={Username}&user_token={Token}"));
				});
		}

		[Test]
		[NeedsAuthentication]
		public async Task GetValueAsCurrentUserAsync_Int_ValidUrl()
		{
			await TestUrlAsync(() => GameJoltAPI.DataStore.GetValueAsIntAsCurrentUserAsync("Key"),
				url =>
				{
					Assert.That(url,
						Does.StartWith($"{GameJoltUrlBuilder.BASE_URL}{GameJoltDataStore.FETCH_ENDPOINT}?key=Key&username={Username}&user_token={Token}"));
				});
		}

		[Test]
		[NeedsAuthentication]
		public async Task GetValueAsCurrentUserAsync_Bytes_ValidUrl()
		{
			await TestUrlAsync(() => GameJoltAPI.DataStore.GetValueAsBytesAsCurrentUserAsync("Key"),
				url =>
				{
					Assert.That(url,
						Does.StartWith($"{GameJoltUrlBuilder.BASE_URL}{GameJoltDataStore.FETCH_ENDPOINT}?key=Key&username={Username}&user_token={Token}"));
				});
		}

		[Test]
		[NeedsAuthentication]
		public async Task GetValueAsCurrentUserAsync_Bool_ValidUrl()
		{
			await TestUrlAsync(() => GameJoltAPI.DataStore.GetValueAsBoolAsCurrentUserAsync("Key"),
				url =>
				{
					Assert.That(url,
						Does.StartWith($"{GameJoltUrlBuilder.BASE_URL}{GameJoltDataStore.FETCH_ENDPOINT}?key=Key&username={Username}&user_token={Token}"));
				});
		}

		[Test]
		[NeedsAuthentication]
		public async Task GetValueAsCurrentUserAsync_Buffer_Bytes_Success()
		{
			// Arrange
			byte[] bytes = DummyData.Bytes();
			List<byte> buffer = new List<byte>();
			GameJoltAPI.webClient.GetStringAsync("", CancellationToken.None).ReturnsForAnyArgs(info =>
			{
				string json = serializer.SerializeResponse(new GetDataResponse(true, null, Convert.ToBase64String(bytes)));

				return FromResult(json);
			});

			// Act
			GameJoltResult result = await GameJoltAPI.DataStore.GetValueAsBytesAsCurrentUserAsync("key", buffer);

			// Assert
			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			Assert.That(buffer.Count, Is.EqualTo(bytes.Length));
			for (int i = 0; i < bytes.Length; i++)
			{
				Assert.That(buffer[i], Is.EqualTo(bytes[i]));
			}
		}

		[Test]
		[NeedsAuthentication]
		public async Task GetValueAsCurrentUserAsync_Buffer_Bytes_InvalidValue_Fail()
		{
			// Arrange
			List<byte> buffer = new List<byte>();
			GameJoltAPI.webClient.GetStringAsync("", CancellationToken.None).ReturnsForAnyArgs(info =>
			{
				string json = serializer.SerializeResponse(new GetDataResponse(true, null, "VeryInvalidValue12345"));

				return FromResult(json);
			});

			// Act
			GameJoltResult result = await GameJoltAPI.DataStore.GetValueAsBytesAsCurrentUserAsync("key", buffer);

			// Assert
			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception, Is.TypeOf<FormatException>());
		}

		[Test]
		[NeedsAuthentication]
		public async Task GetValueAsCurrentUserAsync_Buffer_Bytes_EmptyValue_Success()
		{
			// Arrange
			List<byte> buffer = new List<byte>();
			GameJoltAPI.webClient.GetStringAsync("", CancellationToken.None).ReturnsForAnyArgs(info =>
			{
				string json = serializer.SerializeResponse(new GetDataResponse(true, null, string.Empty));

				return FromResult(json);
			});

			// Act
			GameJoltResult result = await GameJoltAPI.DataStore.GetValueAsBytesAsCurrentUserAsync("key", buffer);

			// Assert
			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			Assert.That(buffer, Is.Empty);
		}

		[Test]
		[NeedsAuthentication]
		public async Task GetValueAsCurrentUserAsync_Buffer_Bytes_Error_Fail()
		{
			// Arrange
			List<byte> buffer = new List<byte>();
			GameJoltAPI.webClient.GetStringAsync("", CancellationToken.None).ReturnsForAnyArgs(info =>
			{
				string json = serializer.SerializeResponse(new GetDataResponse(false, GameJoltInvalidDataStoreKeyException.NO_KEY_MESSAGE, null));

				return FromResult(json);
			});

			// Act
			GameJoltResult result = await GameJoltAPI.DataStore.GetValueAsBytesAsCurrentUserAsync("key", buffer);

			// Assert
			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception, Is.TypeOf<GameJoltInvalidDataStoreKeyException>());
		}

		[Test]
		public async Task GetValueAsCurrentUserAsync_Buffer_Bytes_NotAuthenticated_Fail()
		{
			// Arrange
			List<byte> buffer = new List<byte>();

			// Act
			GameJoltResult result = await GameJoltAPI.DataStore.GetValueAsBytesAsCurrentUserAsync("key", buffer);

			// Assert
			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception, Is.TypeOf<GameJoltAuthorizedException>());
		}
	}
}
#endif // DISABLE_GAMEJOLT