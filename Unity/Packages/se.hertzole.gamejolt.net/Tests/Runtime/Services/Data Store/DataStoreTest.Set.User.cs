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
		/// ------------------------------
		/// When the user is authenticated
		/// ------------------------------
		[Test]
		public async Task SetUser_Authenticated_String_Success()
		{
			await AuthenticateAsync();

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string json = serializer.SerializeResponse(new Response(true, null));

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
				string json = serializer.SerializeResponse(new Response(true, null));

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
				string json = serializer.SerializeResponse(new Response(true, null));

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
				string json = serializer.SerializeResponse(new Response(true, null));

				return FromResult(json);
			});

			GameJoltResult result = await GameJoltAPI.DataStore.SetAsCurrentUserAsync("key", true);

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
		}

		[Test]
		public async Task SetUser_Authenticated_Null_Fail([Values] bool emptyString)
		{
			await AuthenticateAsync();

			GameJoltResult result = await GameJoltAPI.DataStore.SetAsCurrentUserAsync("key", emptyString ? string.Empty : null!);

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception, Is.TypeOf<ArgumentException>());
		}

		[Test]
		public async Task SetUser_Authenticated_Error_Fail()
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
				return GameJoltAPI.DataStore.SetAsCurrentUserAsync("key", "value");
			}
		}

		/// ---------------------------------
		/// When the user isn't authenticated
		/// ---------------------------------
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
		public async Task SetAsCurrentUser_Bool_ValidUrl([Values] bool value)
		{
			await AuthenticateAsync();

			await TestUrlAsync(() => GameJoltAPI.DataStore.SetAsCurrentUserAsync("Key", value), url =>
			{
				Assert.That(url, Does.StartWith(
					$"{GameJoltUrlBuilder.BASE_URL}{GameJoltDataStore.SET_ENDPOINT}?key=Key&data={value.ToString().ToLowerInvariant()}&username={Username}&user_token={Token}"));
			});
		}
	}
}