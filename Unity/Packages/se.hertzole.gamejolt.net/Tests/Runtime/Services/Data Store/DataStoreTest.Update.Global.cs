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
		public async Task UpdateGlobal_String_Success([Values] StringOperation operation)
		{
			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string result;
				switch (operation)
				{
					case StringOperation.Append:
						result = "1value";
						break;
					case StringOperation.Prepend:
						result = "value1";
						break;
					default:
						throw new ArgumentOutOfRangeException(nameof(operation), operation, null);
				}

				string json = serializer.SerializeResponse(new UpdateDataResponse(true, null, result));

				return FromResult(json);
			});

			GameJoltResult<string> result = await GameJoltAPI.DataStore.UpdateAsync("key", "1", operation);

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);

			switch (operation)
			{
				case StringOperation.Append:
					Assert.That(result.Value, Is.EqualTo("1value"));
					break;
				case StringOperation.Prepend:
					Assert.That(result.Value, Is.EqualTo("value1"));
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(operation), operation, null);
			}
		}

		[Test]
		public async Task UpdateGlobal_String_Error_Fail()
		{
			await AssertErrorAsync<UpdateDataResponse, string, GameJoltInvalidDataStoreKeyException>(CreateResponse, GetResult,
				GameJoltInvalidDataStoreKeyException.NO_KEY_MESSAGE);

			return;

			UpdateDataResponse CreateResponse()
			{
				return new UpdateDataResponse(false, GameJoltInvalidDataStoreKeyException.NO_KEY_MESSAGE, null);
			}

			Task<GameJoltResult<string>> GetResult()
			{
				return GameJoltAPI.DataStore.UpdateAsync("key", "value", StringOperation.Append);
			}
		}

		[Test]
		public async Task UpdateGlobal_Int_Success([Values] NumericOperation operation)
		{
			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string result;
				switch (operation)
				{
					case NumericOperation.Add:
						result = "2";
						break;
					case NumericOperation.Subtract:
						result = "0";
						break;
					case NumericOperation.Multiply:
						result = "1";
						break;
					case NumericOperation.Divide:
						result = "1";
						break;
					case NumericOperation.Append:
						result = "11";
						break;
					case NumericOperation.Prepend:
						result = "11";
						break;
					default:
						result = "0";
						break;
				}

				string json = serializer.SerializeResponse(new UpdateDataResponse(true, null, result));

				return FromResult(json);
			});

			GameJoltResult<int> result = await GameJoltAPI.DataStore.UpdateAsync("key", 1, operation);

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);

			switch (operation)
			{
				case NumericOperation.Add:
					Assert.That(result.Value, Is.EqualTo(2));
					break;
				case NumericOperation.Multiply:
					Assert.That(result.Value, Is.EqualTo(1));
					break;
				case NumericOperation.Divide:
					Assert.That(result.Value, Is.EqualTo(1));
					break;
				case NumericOperation.Append:
					Assert.That(result.Value, Is.EqualTo(11));
					break;
				case NumericOperation.Prepend:
					Assert.That(result.Value, Is.EqualTo(11));
					break;
				default:
					Assert.That(result.Value, Is.EqualTo(0));
					break;
			}
		}

		[Test]
		public async Task UpdateGlobal_Int_Error_Fail()
		{
			await AssertErrorAsync<UpdateDataResponse, int, GameJoltInvalidDataStoreKeyException>(CreateResponse, GetResult,
				GameJoltInvalidDataStoreKeyException.NO_KEY_MESSAGE);

			return;

			UpdateDataResponse CreateResponse()
			{
				return new UpdateDataResponse(false, GameJoltInvalidDataStoreKeyException.NO_KEY_MESSAGE, null);
			}

			Task<GameJoltResult<int>> GetResult()
			{
				return GameJoltAPI.DataStore.UpdateAsync("key", 0, NumericOperation.Add);
			}
		}
		
		[Test]
		public async Task UpdateAsync_String_ValidUrl([Values] StringOperation operation)
		{
			await TestUrlAsync(() => GameJoltAPI.DataStore.UpdateAsync("Key", "Value", operation), url =>
			{
				Assert.That(url, Does.StartWith(
					$"{GameJoltUrlBuilder.BASE_URL}{GameJoltDataStore.UPDATE_ENDPOINT}?key=Key&operation={GameJoltDataStore.GetStringOperation(operation)}&value=Value"));
			});
		}

		[Test]
		public async Task UpdateAsync_Int_ValidUrl([Values] NumericOperation operation)
		{
			await TestUrlAsync(() => GameJoltAPI.DataStore.UpdateAsync("Key", 1, operation), url =>
			{
				Assert.That(url, Does.StartWith(
					$"{GameJoltUrlBuilder.BASE_URL}{GameJoltDataStore.UPDATE_ENDPOINT}?key=Key&operation={GameJoltDataStore.GetNumberOperation(operation)}&value=1"));
			});
		}
	}
}