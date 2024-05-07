#nullable enable

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
		public async Task SetGlobal_String_Success()
		{
			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string json = serializer.SerializeResponse(new Response(true, null));

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
				string json = serializer.SerializeResponse(new Response(true, null));

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
				string json = serializer.SerializeResponse(new Response(true, null));

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
				string json = serializer.SerializeResponse(new Response(true, null));

				return FromResult(json);
			});

			GameJoltResult result = await GameJoltAPI.DataStore.SetAsync("key", true);

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
		}

		[Test]
		public async Task SetGlobal_Null_Fail([Values] bool emptyString)
		{
			GameJoltResult result = await GameJoltAPI.DataStore.SetAsync("key", emptyString ? string.Empty : null!);

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception, Is.TypeOf<ArgumentException>());
		}

		[Test]
		public async Task SetGlobal_Error_Fail()
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
				return GameJoltAPI.DataStore.SetAsync("key", "value");
			}
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
		public async Task Set_Bool_ValidUrl([Values] bool value)
		{
			await TestUrlAsync(() => GameJoltAPI.DataStore.SetAsync("Key", value),
				url =>
				{
					Assert.That(url,
						Does.StartWith($"{GameJoltUrlBuilder.BASE_URL}{GameJoltDataStore.SET_ENDPOINT}?key=Key&data={value.ToString().ToLowerInvariant()}"));
				});
		}
	}
}