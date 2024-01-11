using System;
using System.Threading.Tasks;
using Hertzole.GameJolt;
using NSubstitute;
using NUnit.Framework;

namespace GameJolt.NET.Tests
{
	public class DataStoreTest : BaseTest
	{
		[Test]
		public async Task SetGlobal_String_Success()
		{
			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string json = serializer.Serialize(new StoreDataResponse(true, null));

				return FromResult(json);
			});

			GameJoltResult result = await GameJoltAPI.DataStore.SetAsync("key", "value");

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
		}

		[Test]
		public async Task SetGlobal_Int_Success()
		{
			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string json = serializer.Serialize(new StoreDataResponse(true, null));

				return FromResult(json);
			});

			GameJoltResult result = await GameJoltAPI.DataStore.SetAsync("key", 1);

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
		}

		[Test]
		public async Task SetGlobal_Bytes_Success()
		{
			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string json = serializer.Serialize(new StoreDataResponse(true, null));

				return FromResult(json);
			});

			GameJoltResult result = await GameJoltAPI.DataStore.SetAsync("key", DummyData.Bytes());

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
		}

		[Test]
		public async Task SetGlobal_Bool_Success()
		{
			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string json = serializer.Serialize(new StoreDataResponse(true, null));

				return FromResult(json);
			});

			GameJoltResult result = await GameJoltAPI.DataStore.SetAsync("key", true);

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
		}

		[Test]
		public async Task SetUser_Authenticated_String_Success()
		{
			await AuthenticateAsync();

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string json = serializer.Serialize(new StoreDataResponse(true, null));

				return FromResult(json);
			});

			GameJoltResult result = await GameJoltAPI.DataStore.SetAsCurrentUserAsync("key", "value");

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
		}

		[Test]
		public async Task SetUser_Authenticated_Int_Success()
		{
			await AuthenticateAsync();

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string json = serializer.Serialize(new StoreDataResponse(true, null));

				return FromResult(json);
			});

			GameJoltResult result = await GameJoltAPI.DataStore.SetAsCurrentUserAsync("key", 1);

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
		}

		[Test]
		public async Task SetUser_Authenticated_Bytes_Success()
		{
			await AuthenticateAsync();

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string json = serializer.Serialize(new StoreDataResponse(true, null));

				return FromResult(json);
			});

			GameJoltResult result = await GameJoltAPI.DataStore.SetAsCurrentUserAsync("key", DummyData.Bytes());

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
		}
		
		[Test]
		public async Task SetUser_Authenticated_Bool_Success()
		{
			await AuthenticateAsync();

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string json = serializer.Serialize(new StoreDataResponse(true, null));

				return FromResult(json);
			});

			GameJoltResult result = await GameJoltAPI.DataStore.SetAsCurrentUserAsync("key", true);

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
		}

		[Test]
		public async Task SetUser_NotAuthenticated_String_Fail()
		{
			GameJoltResult result = await GameJoltAPI.DataStore.SetAsCurrentUserAsync("key", "value");

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception, Is.TypeOf<GameJoltAuthorizedException>());
		}

		[Test]
		public async Task SetUser_NotAuthenticated_Int_Fail()
		{
			GameJoltResult result = await GameJoltAPI.DataStore.SetAsCurrentUserAsync("key", 1);

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception, Is.TypeOf<GameJoltAuthorizedException>());
		}

		[Test]
		public async Task SetUser_NotAuthenticated_Bytes_Fail()
		{
			GameJoltResult result = await GameJoltAPI.DataStore.SetAsCurrentUserAsync("key", DummyData.Bytes());

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception, Is.TypeOf<GameJoltAuthorizedException>());
		}
		
		[Test]
		public async Task SetUser_NotAuthenticated_Bool_Fail()
		{
			GameJoltResult result = await GameJoltAPI.DataStore.SetAsCurrentUserAsync("key", true);

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception, Is.TypeOf<GameJoltAuthorizedException>());
		}

		[Test]
		public async Task RemoveGlobal_Success()
		{
			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string json = serializer.Serialize(new StoreDataResponse(true, null));

				return FromResult(json);
			});

			GameJoltResult result = await GameJoltAPI.DataStore.RemoveAsync("key");

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
		}

		[Test]
		public async Task RemoveUser_Authenticated_Success()
		{
			await AuthenticateAsync();

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string json = serializer.Serialize(new StoreDataResponse(true, null));

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
		[TestCase(StringOperation.Prepend)]
		[TestCase(StringOperation.Append)]
		public async Task UpdateGlobal_String_Success(StringOperation operation)
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

				string json = serializer.Serialize(new UpdateDataResponse(true, null, result));

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
		[TestCase(NumericOperation.Add)]
		[TestCase(NumericOperation.Subtract)]
		[TestCase(NumericOperation.Multiply)]
		[TestCase(NumericOperation.Divide)]
		[TestCase(NumericOperation.Append)]
		[TestCase(NumericOperation.Prepend)]
		public async Task UpdateGlobal_Int_Success(NumericOperation operation)
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

				string json = serializer.Serialize(new UpdateDataResponse(true, null, result));

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
		[TestCase(StringOperation.Prepend)]
		[TestCase(StringOperation.Append)]
		public async Task UpdateUser_Authenticated_String_Success(StringOperation operation)
		{
			await AuthenticateAsync();

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

				string json = serializer.Serialize(new UpdateDataResponse(true, null, result));

				return FromResult(json);
			});

			GameJoltResult<string> result = await GameJoltAPI.DataStore.UpdateAsCurrentUserAsync("key", "1", operation);

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
		[TestCase(NumericOperation.Add)]
		[TestCase(NumericOperation.Subtract)]
		[TestCase(NumericOperation.Multiply)]
		[TestCase(NumericOperation.Divide)]
		[TestCase(NumericOperation.Append)]
		[TestCase(NumericOperation.Prepend)]
		public async Task UpdateUser_Authenticated_Int_Success(NumericOperation operation)
		{
			await AuthenticateAsync();

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

				string json = serializer.Serialize(new UpdateDataResponse(true, null, result));

				return FromResult(json);
			});

			GameJoltResult<int> result = await GameJoltAPI.DataStore.UpdateAsCurrentUserAsync("key", 1, operation);

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
		[TestCase(StringOperation.Prepend)]
		[TestCase(StringOperation.Append)]
		public async Task UpdateUser_NotAuthenticated_String_Fail(StringOperation operation)
		{
			GameJoltResult<string> result = await GameJoltAPI.DataStore.UpdateAsCurrentUserAsync("key", "1", operation);

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception, Is.TypeOf<GameJoltAuthorizedException>());
		}

		[Test]
		[TestCase(NumericOperation.Add)]
		[TestCase(NumericOperation.Subtract)]
		[TestCase(NumericOperation.Multiply)]
		[TestCase(NumericOperation.Divide)]
		[TestCase(NumericOperation.Append)]
		[TestCase(NumericOperation.Prepend)]
		public async Task UpdateUser_NotAuthenticated_Int_Fail(NumericOperation operation)
		{
			GameJoltResult<int> result = await GameJoltAPI.DataStore.UpdateAsCurrentUserAsync("key", 1, operation);

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception, Is.TypeOf<GameJoltAuthorizedException>());
		}

		[Test]
		public async Task GetValueGlobal_String_Success()
		{
			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string json = serializer.Serialize(new GetDataResponse(true, null, "value"));

				return FromResult(json);
			});

			GameJoltResult<string> result = await GameJoltAPI.DataStore.GetValueAsStringAsync("key");

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			Assert.That(result.Value, Is.EqualTo("value"));
		}

		[Test]
		public async Task GetValueGlobal_Int_Success()
		{
			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string json = serializer.Serialize(new GetDataResponse(true, null, "1"));

				return FromResult(json);
			});

			GameJoltResult<int> result = await GameJoltAPI.DataStore.GetValueAsIntAsync("key");

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			Assert.That(result.Value, Is.EqualTo(1));
		}

		[Test]
		public async Task GetValueGlobal_Bytes_Success()
		{
			byte[] bytes = DummyData.Bytes();

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string json = serializer.Serialize(new GetDataResponse(true, null, Convert.ToBase64String(bytes)));

				return FromResult(json);
			});

			GameJoltResult<byte[]> result = await GameJoltAPI.DataStore.GetValueAsBytesAsync("key");

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			Assert.That(result.Value, Is.EqualTo(bytes));
		}
		
		[Test]
		public async Task GetValueGlobal_Bool_Success()
		{
			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string json = serializer.Serialize(new GetDataResponse(true, null, "true"));

				return FromResult(json);
			});

			GameJoltResult<bool> result = await GameJoltAPI.DataStore.GetValueAsBoolAsync("key");

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			Assert.That(result.Value, Is.True);
		}

		[Test]
		public async Task GetValueUser_Authenticated_String_Success()
		{
			await AuthenticateAsync();

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string json = serializer.Serialize(new GetDataResponse(true, null, "value"));

				return FromResult(json);
			});

			GameJoltResult<string> result = await GameJoltAPI.DataStore.GetValueAsStringAsCurrentUserAsync("key");

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			Assert.That(result.Value, Is.EqualTo("value"));
		}

		[Test]
		public async Task GetValueUser_Authenticated_Int_Success()
		{
			await AuthenticateAsync();

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string json = serializer.Serialize(new GetDataResponse(true, null, "1"));

				return FromResult(json);
			});

			GameJoltResult<int> result = await GameJoltAPI.DataStore.GetValueAsIntAsCurrentUserAsync("key");

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			Assert.That(result.Value, Is.EqualTo(1));
		}

		[Test]
		public async Task GetValueUser_Authenticated_Bytes_Success()
		{
			await AuthenticateAsync();

			byte[] bytes = DummyData.Bytes();

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string json = serializer.Serialize(new GetDataResponse(true, null, Convert.ToBase64String(bytes)));

				return FromResult(json);
			});

			GameJoltResult<byte[]> result = await GameJoltAPI.DataStore.GetValueAsBytesAsCurrentUserAsync("key");

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			Assert.That(result.Value, Is.EqualTo(bytes));
		}
		
		[Test]
		public async Task GetValueUser_Authenticated_Bool_Success()
		{
			await AuthenticateAsync();

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string json = serializer.Serialize(new GetDataResponse(true, null, "true"));

				return FromResult(json);
			});

			GameJoltResult<bool> result = await GameJoltAPI.DataStore.GetValueAsBoolAsCurrentUserAsync("key");

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			Assert.That(result.Value, Is.True);
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

				string json = serializer.Serialize(new GetKeysResponse(true, null, new[]
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

				string json = serializer.Serialize(new GetKeysResponse(true, null, new[]
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
			Assert.That(result.Value!.Length, Is.EqualTo(2));
			Assert.That(result.Value[0], Is.EqualTo("key1"));
			Assert.That(result.Value[1], Is.EqualTo("key2"));
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
		public async Task Set_String_ValidUrl()
		{
			await TestUrlAsync(() => GameJoltAPI.DataStore.SetAsync("Key", "Value"),
				url => { Assert.That(url, Does.StartWith(GameJoltUrlBuilder.BASE_URL + GameJoltDataStore.SET_ENDPOINT + "?key=Key&data=Value")); });
		}

		[Test]
		public async Task Set_Int_ValidUrl()
		{
			await TestUrlAsync(() => GameJoltAPI.DataStore.SetAsync("Key", 1),
				url => { Assert.That(url, Does.StartWith(GameJoltUrlBuilder.BASE_URL + GameJoltDataStore.SET_ENDPOINT + "?key=Key&data=1")); });
		}

		[Test]
		public async Task Set_Bytes_ValidUrl()
		{
			string bytes = Convert.ToBase64String(DummyData.Bytes());

			await TestUrlAsync(() => GameJoltAPI.DataStore.SetAsync("Key", bytes),
				url => { Assert.That(url, Does.StartWith($"{GameJoltUrlBuilder.BASE_URL}{GameJoltDataStore.SET_ENDPOINT}?key=Key&data={bytes}")); });
		}
		
		[Test]
		[TestCase(true)]
		[TestCase(false)]
		public async Task Set_Bool_ValidUrl(bool value)
		{
			await TestUrlAsync(() => GameJoltAPI.DataStore.SetAsync("Key", value),
				url => { Assert.That(url, Does.StartWith($"{GameJoltUrlBuilder.BASE_URL}{GameJoltDataStore.SET_ENDPOINT}?key=Key&data={value.ToString().ToLowerInvariant()}")); });
		}

		[Test]
		public async Task SetAsCurrentUser_String_ValidUrl()
		{
			await AuthenticateAsync();

			await TestUrlAsync(() => GameJoltAPI.DataStore.SetAsCurrentUserAsync("Key", "Value"), url =>
			{
				Assert.That(url, Does.StartWith(
					$"{GameJoltUrlBuilder.BASE_URL}{GameJoltDataStore.SET_ENDPOINT}?key=Key&data=Value&username={Username}&user_token={Token}"));
			});
		}

		[Test]
		public async Task SetAsCurrentUser_Int_ValidUrl()
		{
			await AuthenticateAsync();

			await TestUrlAsync(() => GameJoltAPI.DataStore.SetAsCurrentUserAsync("Key", 1), url =>
			{
				Assert.That(url, Does.StartWith(
					$"{GameJoltUrlBuilder.BASE_URL}{GameJoltDataStore.SET_ENDPOINT}?key=Key&data=1&username={Username}&user_token={Token}"));
			});
		}

		[Test]
		public async Task SetAsCurrentUser_Bytes_ValidUrl()
		{
			await AuthenticateAsync();

			string bytes = Convert.ToBase64String(DummyData.Bytes());

			await TestUrlAsync(() => GameJoltAPI.DataStore.SetAsCurrentUserAsync("Key", bytes), url =>
			{
				Assert.That(url, Does.StartWith(
					$"{GameJoltUrlBuilder.BASE_URL}{GameJoltDataStore.SET_ENDPOINT}?key=Key&data={bytes}&username={Username}&user_token={Token}"));
			});
		}
		
		[Test]
		[TestCase(true)]
		[TestCase(false)]
		public async Task SetAsCurrentUser_Bool_ValidUrl(bool value)
		{
			await AuthenticateAsync();

			await TestUrlAsync(() => GameJoltAPI.DataStore.SetAsCurrentUserAsync("Key", value), url =>
			{
				Assert.That(url, Does.StartWith(
					$"{GameJoltUrlBuilder.BASE_URL}{GameJoltDataStore.SET_ENDPOINT}?key=Key&data={value.ToString().ToLowerInvariant()}&username={Username}&user_token={Token}"));
			});
		}

		[Test]
		public async Task Remove_ValidUrl()
		{
			await TestUrlAsync(() => GameJoltAPI.DataStore.RemoveAsync("Key"),
				url => { Assert.That(url, Does.StartWith($"{GameJoltUrlBuilder.BASE_URL}{GameJoltDataStore.REMOVE_ENDPOINT}?key=Key")); });
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

		[Test]
		[TestCase(StringOperation.Prepend)]
		[TestCase(StringOperation.Append)]
		public async Task UpdateAsync_String_ValidUrl(StringOperation operation)
		{
			await TestUrlAsync(() => GameJoltAPI.DataStore.UpdateAsync("Key", "Value", operation), url =>
			{
				Assert.That(url, Does.StartWith(
					$"{GameJoltUrlBuilder.BASE_URL}{GameJoltDataStore.UPDATE_ENDPOINT}?key=Key&operation={GameJoltDataStore.GetStringOperation(operation)}&value=Value"));
			});
		}

		[Test]
		[TestCase(NumericOperation.Add)]
		[TestCase(NumericOperation.Subtract)]
		[TestCase(NumericOperation.Multiply)]
		[TestCase(NumericOperation.Divide)]
		[TestCase(NumericOperation.Append)]
		[TestCase(NumericOperation.Prepend)]
		public async Task UpdateAsync_Int_ValidUrl(NumericOperation operation)
		{
			await TestUrlAsync(() => GameJoltAPI.DataStore.UpdateAsync("Key", 1, operation), url =>
			{
				Assert.That(url, Does.StartWith(
					$"{GameJoltUrlBuilder.BASE_URL}{GameJoltDataStore.UPDATE_ENDPOINT}?key=Key&operation={GameJoltDataStore.GetNumberOperation(operation)}&value=1"));
			});
		}

		[Test]
		[TestCase(StringOperation.Prepend)]
		[TestCase(StringOperation.Append)]
		public async Task UpdateAsyncAsCurrentUser_String_ValidUrl(StringOperation operation)
		{
			await AuthenticateAsync();

			await TestUrlAsync(() => GameJoltAPI.DataStore.UpdateAsCurrentUserAsync("Key", "Value", operation), url =>
			{
				Assert.That(url, Does.StartWith(
					$"{GameJoltUrlBuilder.BASE_URL}{GameJoltDataStore.UPDATE_ENDPOINT}?key=Key&username={Username}&user_token={Token}&operation={GameJoltDataStore.GetStringOperation(operation)}&value=Value"));
			});
		}

		[Test]
		[TestCase(NumericOperation.Add)]
		[TestCase(NumericOperation.Subtract)]
		[TestCase(NumericOperation.Multiply)]
		[TestCase(NumericOperation.Divide)]
		[TestCase(NumericOperation.Append)]
		[TestCase(NumericOperation.Prepend)]
		public async Task UpdateAsyncAsCurrentUser_Int_ValidUrl(NumericOperation operation)
		{
			await AuthenticateAsync();

			await TestUrlAsync(() => GameJoltAPI.DataStore.UpdateAsCurrentUserAsync("Key", 1, operation), url =>
			{
				Assert.That(url, Does.StartWith(
					$"{GameJoltUrlBuilder.BASE_URL}{GameJoltDataStore.UPDATE_ENDPOINT}?key=Key&username={Username}&user_token={Token}&operation={GameJoltDataStore.GetNumberOperation(operation)}&value=1"));
			});
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
						Assert.That(url, Does.StartWith($"{GameJoltUrlBuilder.BASE_URL}{GameJoltDataStore.FETCH_KEYS_ENDPOINT}?username={Username}&user_token={Token}"));
					}
					else
					{
						Assert.That(url, Does.StartWith($"{GameJoltUrlBuilder.BASE_URL}{GameJoltDataStore.FETCH_KEYS_ENDPOINT}?pattern={pattern}&username={Username}&user_token={Token}"));
					}
				});
		}
	}
}