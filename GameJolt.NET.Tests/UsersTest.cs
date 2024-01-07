using System.Threading.Tasks;
using Hertzole.GameJolt;
using NSubstitute;
using NUnit.Framework;

namespace GameJolt.NET.Tests
{
	public class UsersTest : BaseTest
	{
		[Test]
		public async Task Authenticate_ValidToken_Success()
		{
			string authJson = serializer.Serialize(new AuthResponse(true, null));
			string userJson =
				serializer.Serialize(new UsersFetchResponse(true, null, CreateDummyUser()));

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("users/?"))
				{
					return FromResult(userJson);
				}

				if (arg.Contains("users/auth"))
				{
					return FromResult(authJson);
				}

				return FromResult("");
			});

			GameJoltResult result = await GameJoltAPI.Users.AuthenticateAsync("test", "test");

			Assert.IsFalse(result.HasError);
			Assert.IsNull(result.Exception);
		}

		[Test]
		public async Task Authenticate_InvalidToken_Failure()
		{
			string authJson = serializer.Serialize(new AuthResponse(false, GameJoltAuthenticationException.MESSAGE));
			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(FromResult(authJson));

			GameJoltResult result = await GameJoltAPI.Users.AuthenticateAsync("test", "test");

			Assert.IsTrue(result.HasError);
			Assert.IsNotNull(result.Exception);
			Assert.IsTrue(result.Exception is GameJoltAuthenticationException);
		}

		[Test]
		public async Task Authenticate_InvokesAuthenticateEvent_Success()
		{
			string authJson = serializer.Serialize(new AuthResponse(true, null));
			string userJson =
				serializer.Serialize(new UsersFetchResponse(true, null, CreateDummyUser()));

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("users/?"))
				{
					return FromResult(userJson);
				}

				if (arg.Contains("users/auth"))
				{
					return FromResult(authJson);
				}

				return FromResult("");
			});

			bool invoked = false;
			GameJoltAPI.Users.OnUserAuthenticated += user => { invoked = true; };

			GameJoltResult result = await GameJoltAPI.Users.AuthenticateAsync("test", "test");

			Assert.IsFalse(result.HasError);
			Assert.IsNull(result.Exception);
			Assert.IsTrue(invoked);
		}

		[Test]
		public async Task Authenticate_DoesNotInvokeAuthenticateEvent_Failure()
		{
			string authJson = serializer.Serialize(new AuthResponse(false, GameJoltAuthenticationException.MESSAGE));
			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(FromResult(authJson));

			bool invoked = false;
			GameJoltAPI.Users.OnUserAuthenticated += user => { invoked = true; };

			GameJoltResult result = await GameJoltAPI.Users.AuthenticateAsync("test", "test");

			Assert.IsTrue(result.HasError);
			Assert.IsNotNull(result.Exception);
			Assert.IsTrue(result.Exception is GameJoltAuthenticationException);
			Assert.IsFalse(invoked);
		}

		[Test]
		[TestCase("https://gamejolt.com/game/bla/bla/bla?gjapi_username=test&gjapi_token=test")]
		[TestCase("https://gamejolt.net/game/bla/bla/bla?gjapi_username=test&gjapi_token=test")]
		public async Task Authenticate_Url_Success(string url)
		{
			string authJson = serializer.Serialize(new AuthResponse(true, null));
			string userJson = serializer.Serialize(new UsersFetchResponse(true, null, CreateDummyUser()));

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("users/?"))
				{
					return FromResult(userJson);
				}

				if (arg.Contains("users/auth"))
				{
					return FromResult(authJson);
				}

				return FromResult("");
			});

			GameJoltResult result = await GameJoltAPI.Users.AuthenticateFromUrlAsync(url);

			Assert.IsFalse(result.HasError);
			Assert.IsNull(result.Exception);
		}

		[Test]
		[TestCase("https://gamejolt.com/game/bla/bla/bla?gjapi_username=test&gjapi_token=test")]
		[TestCase("https://gamejolt.net/game/bla/bla/bla?gjapi_username=test&gjapi_token=test")]
		public async Task Authenticate_Url_Failure(string url)
		{
			string authJson = serializer.Serialize(new AuthResponse(false, GameJoltAuthenticationException.MESSAGE));
			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(FromResult(authJson));

			GameJoltResult result = await GameJoltAPI.Users.AuthenticateFromUrlAsync(url);

			Assert.IsTrue(result.HasError);
			Assert.IsNotNull(result.Exception);
			Assert.IsTrue(result.Exception is GameJoltAuthenticationException);
		}

		[Test]
		public async Task Authenticate_CredentialsFile_Success()
		{
			string authJson = serializer.Serialize(new AuthResponse(true, null));
			string userJson = serializer.Serialize(new UsersFetchResponse(true, null, CreateDummyUser()));

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("users/?"))
				{
					return FromResult(userJson);
				}

				if (arg.Contains("users/auth"))
				{
					return FromResult(authJson);
				}

				return FromResult("");
			});

			const string credentials = @"0.2.1
test
test
";

			GameJoltResult result = await GameJoltAPI.Users.AuthenticateFromCredentialsFileAsync(credentials);

			Assert.IsFalse(result.HasError);
			Assert.IsNull(result.Exception);
		}

		[Test]
		public async Task Fetch_ValidUsername_Success()
		{
			string userJson = serializer.Serialize(new UsersFetchResponse(true, null, CreateDummyUser()));

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("users/?"))
				{
					return FromResult(userJson);
				}

				return FromResult("");
			});

			GameJoltResult<GameJoltUser> result = await GameJoltAPI.Users.FetchUserAsync("Username");

			Assert.IsFalse(result.HasError);
			Assert.IsNull(result.Exception);
			Assert.IsNotNull(result.Value);
		}

		[Test]
		public async Task Fetch_ValidId_Success()
		{
			string userJson = serializer.Serialize(new UsersFetchResponse(true, null, CreateDummyUser()));

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("users/?"))
				{
					return FromResult(userJson);
				}

				return FromResult("");
			});

			GameJoltResult<GameJoltUser> result = await GameJoltAPI.Users.FetchUserAsync(0);

			Assert.IsFalse(result.HasError);
			Assert.IsNull(result.Exception);
			Assert.IsNotNull(result.Value);
		}

		[Test]
		public async Task Fetch_InvalidUsername_Failure()
		{
			string userJson = serializer.Serialize(new UsersFetchResponse(false, GameJoltInvalidUserException.MESSAGE, null));

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("users/?"))
				{
					return FromResult(userJson);
				}

				return FromResult("");
			});

			GameJoltResult<GameJoltUser> result = await GameJoltAPI.Users.FetchUserAsync("Username");

			Assert.IsTrue(result.HasError);
			Assert.IsNotNull(result.Exception);
			Assert.IsTrue(result.Exception is GameJoltInvalidUserException);
		}

		[Test]
		public async Task Fetch_InvalidId_Failure()
		{
			string userJson = serializer.Serialize(new UsersFetchResponse(false, GameJoltInvalidUserException.MESSAGE, null));

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("users/?"))
				{
					return FromResult(userJson);
				}

				return FromResult("");
			});

			GameJoltResult<GameJoltUser> result = await GameJoltAPI.Users.FetchUserAsync(0);

			Assert.IsTrue(result.HasError);
			Assert.IsNotNull(result.Exception);
			Assert.IsTrue(result.Exception is GameJoltInvalidUserException);
		}

		[Test]
		public async Task Fetch_Usernames_Success()
		{
			string userJson = serializer.Serialize(new UsersFetchResponse(true, null, new[] { CreateDummyUser(), CreateDummyUser() }));

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("username="))
				{
					return FromResult(userJson);
				}

				return FromResult("");
			});

			GameJoltResult<GameJoltUser[]> result = await GameJoltAPI.Users.FetchUsersAsync(new[] { "Username", "Username2" });

			Assert.IsFalse(result.HasError);
			Assert.IsNull(result.Exception);
			Assert.IsNotNull(result.Value);
			Assert.That(result.Value, Has.Length.EqualTo(2));
		}

		[Test]
		public async Task Fetch_Ids_Success()
		{
			string userJson = serializer.Serialize(new UsersFetchResponse(true, null, new[] { CreateDummyUser(), CreateDummyUser() }));

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("user_id="))
				{
					return FromResult(userJson);
				}

				return FromResult("");
			});

			GameJoltResult<GameJoltUser[]> result = await GameJoltAPI.Users.FetchUsersAsync(new[] { 0, 1 });

			Assert.IsFalse(result.HasError);
			Assert.IsNull(result.Exception);
			Assert.IsNotNull(result.Value);
			Assert.That(result.Value, Has.Length.EqualTo(2));
		}

		[Test]
		public async Task Fetch_Usernames_Failure()
		{
			string userJson = serializer.Serialize(new UsersFetchResponse(false, GameJoltInvalidUserException.MESSAGE, null));

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("username="))
				{
					return FromResult(userJson);
				}

				return FromResult("");
			});

			GameJoltResult<GameJoltUser[]> result = await GameJoltAPI.Users.FetchUsersAsync(new[] { "Username", "Username2" });

			Assert.IsTrue(result.HasError);
			Assert.IsNotNull(result.Exception);
			Assert.IsTrue(result.Exception is GameJoltInvalidUserException);
		}

		[Test]
		public async Task Fetch_Ids_Failure()
		{
			string userJson = serializer.Serialize(new UsersFetchResponse(false, GameJoltInvalidUserException.MESSAGE, null));

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("user_id="))
				{
					return FromResult(userJson);
				}

				return FromResult("");
			});

			GameJoltResult<GameJoltUser[]> result = await GameJoltAPI.Users.FetchUsersAsync(new[] { 0, 1 });

			Assert.IsTrue(result.HasError);
			Assert.IsNotNull(result.Exception);
			Assert.IsTrue(result.Exception is GameJoltInvalidUserException);
		}
	}
}