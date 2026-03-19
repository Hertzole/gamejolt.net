#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Hertzole.GameJolt;
using NSubstitute;
using NUnit.Framework;

namespace GameJolt.NET.Tests
{
	partial class DataStoreTest
	{
		[Test]
		public async Task GetValueGlobal_String_Success()
		{
			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string json = serializer.SerializeResponse(new GetDataResponse(true, null, "value"));

				return FromResult(json);
			});

			GameJoltResult<string> result = await GameJoltAPI.DataStore.GetValueAsStringAsync("key");

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			Assert.That(result.Value, Is.EqualTo("value"));
		}

		[Test]
		public async Task GetValueGlobal_String_Error_Fail()
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
				return GameJoltAPI.DataStore.GetValueAsStringAsync("key");
			}
		}

		[Test]
		public async Task GetValueGlobal_Int_Success()
		{
			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string json = serializer.SerializeResponse(new GetDataResponse(true, null, "1"));

				return FromResult(json);
			});

			GameJoltResult<int> result = await GameJoltAPI.DataStore.GetValueAsIntAsync("key");

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			Assert.That(result.Value, Is.EqualTo(1));
		}

		[Test]
		public async Task GetValueGlobal_Int_InvalidValue_Fail()
		{
			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string json = serializer.SerializeResponse(new GetDataResponse(true, null, "invalid value"));

				return FromResult(json);
			});

			GameJoltResult<int> result = await GameJoltAPI.DataStore.GetValueAsIntAsync("key");

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception, Is.TypeOf<GameJoltInvalidDataStoreValueException>());
		}

		[Test]
		public async Task GetValueGlobal_Int_Error_Fail()
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
				return GameJoltAPI.DataStore.GetValueAsIntAsync("key");
			}
		}

		[Test]
		public async Task GetValueGlobal_Bytes_Success()
		{
			byte[] bytes = DummyData.Bytes();

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string json = serializer.SerializeResponse(new GetDataResponse(true, null, Convert.ToBase64String(bytes)));

				return FromResult(json);
			});

			GameJoltResult<byte[]> result = await GameJoltAPI.DataStore.GetValueAsBytesAsync("key");

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			Assert.That(result.Value, Is.EqualTo(bytes));
		}

		[Test]
		public async Task GetValueGlobal_Bytes_InvalidValue_Fail()
		{
			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string json = serializer.SerializeResponse(new GetDataResponse(true, null, "VeryInvalidValue12345"));

				return FromResult(json);
			});

			GameJoltResult<byte[]> result = await GameJoltAPI.DataStore.GetValueAsBytesAsync("key");

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception, Is.TypeOf<FormatException>());
		}

		[Test]
		public async Task GetValueGlobal_Bytes_EmptyValue_Success()
		{
			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string json = serializer.SerializeResponse(new GetDataResponse(true, null, string.Empty));

				return FromResult(json);
			});

			GameJoltResult<byte[]> result = await GameJoltAPI.DataStore.GetValueAsBytesAsync("key");

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			Assert.That(result.Value, Is.Empty);
		}

		[Test]
		public async Task GetValueGlobal_Bytes_Error_Fail()
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
				return GameJoltAPI.DataStore.GetValueAsBytesAsync("key");
			}
		}

		[Test]
		public async Task GetValueGlobal_Bool_Success()
		{
			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string json = serializer.SerializeResponse(new GetDataResponse(true, null, "true"));

				return FromResult(json);
			});

			GameJoltResult<bool> result = await GameJoltAPI.DataStore.GetValueAsBoolAsync("key");

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			Assert.That(result.Value, Is.True);
		}

		[Test]
		public async Task GetValueGlobal_Bool_InvalidValue_Fail()
		{
			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string json = serializer.SerializeResponse(new GetDataResponse(true, null, "invalid value"));

				return FromResult(json);
			});

			GameJoltResult<bool> result = await GameJoltAPI.DataStore.GetValueAsBoolAsync("key");

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception, Is.TypeOf<GameJoltInvalidDataStoreValueException>());
		}

		[Test]
		public async Task GetValueGlobal_Bool_Error_Fail()
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
				return GameJoltAPI.DataStore.GetValueAsBoolAsync("key");
			}
		}

		[Test]
		public async Task GetValueAsync_String_ValidUrl()
		{
			await TestUrlAsync(() => GameJoltAPI.DataStore.GetValueAsStringAsync("Key"),
				url => { Assert.That(url, Does.StartWith($"{GameJoltUrlBuilder.BASE_URL}{GameJoltDataStore.FETCH_ENDPOINT}?key=Key")); });
		}

		[Test]
		public async Task GetValueAsync_Int_ValidUrl()
		{
			await TestUrlAsync(() => GameJoltAPI.DataStore.GetValueAsIntAsync("Key"),
				url => { Assert.That(url, Does.StartWith($"{GameJoltUrlBuilder.BASE_URL}{GameJoltDataStore.FETCH_ENDPOINT}?key=Key")); });
		}

		[Test]
		public async Task GetValueAsync_Bytes_ValidUrl()
		{
			await TestUrlAsync(() => GameJoltAPI.DataStore.GetValueAsBytesAsync("Key"),
				url => { Assert.That(url, Does.StartWith($"{GameJoltUrlBuilder.BASE_URL}{GameJoltDataStore.FETCH_ENDPOINT}?key=Key")); });
		}

		[Test]
		public async Task GetValueAsync_Bool_ValidUrl()
		{
			await TestUrlAsync(() => GameJoltAPI.DataStore.GetValueAsBoolAsync("Key"),
				url => { Assert.That(url, Does.StartWith($"{GameJoltUrlBuilder.BASE_URL}{GameJoltDataStore.FETCH_ENDPOINT}?key=Key")); });
		}

		[Test]
		public async Task GetValueGlobal_Buffer_Bytes_Success()
		{
			// Arrange
			byte[] bytes = DummyData.Bytes(10);
			List<byte> buffer = new List<byte>(DummyData.Bytes()); // Add dummy data to make sure buffer is cleared.
			GameJoltAPI.webClient.GetStringAsync("", CancellationToken.None).ReturnsForAnyArgs(info =>
			{
				string json = serializer.SerializeResponse(new GetDataResponse(true, null, Convert.ToBase64String(bytes)));

				return FromResult(json);
			});

			// Act
			GameJoltResult result = await GameJoltAPI.DataStore.GetValueAsBytesAsync("key", buffer);

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
		public async Task GetValueGlobal_Buffer_Bytes_InvalidValue_Fail()
		{
			// Arrange
			List<byte> buffer = new List<byte>();
			GameJoltAPI.webClient.GetStringAsync("", CancellationToken.None).ReturnsForAnyArgs(info =>
			{
				string json = serializer.SerializeResponse(new GetDataResponse(true, null, "VeryInvalidValue12345"));

				return FromResult(json);
			});

			// Act
			GameJoltResult result = await GameJoltAPI.DataStore.GetValueAsBytesAsync("key", buffer);

			// Assert
			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception, Is.TypeOf<FormatException>());
		}

		[Test]
		public async Task GetValueGlobal_Buffer_Bytes_EmptyValue_Success()
		{
			// Arrange
			List<byte> buffer = new List<byte>(DummyData.Bytes()); // Add dummy data to make sure buffer is cleared.
			GameJoltAPI.webClient.GetStringAsync("", CancellationToken.None).ReturnsForAnyArgs(info =>
			{
				string json = serializer.SerializeResponse(new GetDataResponse(true, null, string.Empty));

				return FromResult(json);
			});

			// Act
			GameJoltResult result = await GameJoltAPI.DataStore.GetValueAsBytesAsync("key", buffer);

			// Assert
			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			Assert.That(buffer, Is.Empty);
		}

		[Test]
		public async Task GetValueGlobal_Buffer_Bytes_Error_Fail()
		{
			// Arrange
			List<byte> buffer = new List<byte>();
			GameJoltAPI.webClient.GetStringAsync("", CancellationToken.None).ReturnsForAnyArgs(info =>
			{
				string json = serializer.SerializeResponse(new GetDataResponse(false, GameJoltInvalidDataStoreKeyException.NO_KEY_MESSAGE, null));

				return FromResult(json);
			});

			// Act
			GameJoltResult result = await GameJoltAPI.DataStore.GetValueAsBytesAsync("key", buffer);

			// Assert
			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception, Is.TypeOf<GameJoltInvalidDataStoreKeyException>());
		}
	}
}
#endif // DISABLE_GAMEJOLT