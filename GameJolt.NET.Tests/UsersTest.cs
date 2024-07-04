#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

#nullable enable

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hertzole.GameJolt;
using NSubstitute;
using NUnit.Framework;

namespace GameJolt.NET.Tests
{
	public class UsersTest : BaseTest
	{
		private const string CREDENTIALS_NORMAL = @"0.2.1
test
test
";

		private const string CREDENTIALS_LONG = @"0.2.1
test
test
filler
strings
";
		
		private static readonly string[] usernames = { "Username", "Username2" };
		private static readonly int[] ids = { 0, 1 };

		[Test]
		public async Task Authenticate_ValidToken_Success()
		{
			string authJson = serializer.SerializeResponse(new Response(true, null));
			string userJson =
				serializer.SerializeResponse(new UsersFetchResponse(true, null, DummyData.User()));

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

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
		}

		[Test]
		public async Task Authenticate_InvalidToken_Failure()
		{
			string authJson = serializer.SerializeResponse(new Response(false, GameJoltAuthenticationException.MESSAGE));
			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(FromResult(authJson));

			GameJoltResult result = await GameJoltAPI.Users.AuthenticateAsync("test", "test");

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception, Is.TypeOf<GameJoltAuthenticationException>());
		}

		[Test]
		public async Task Authenticate_InvokesAuthenticateEvent_Success()
		{
			string authJson = serializer.SerializeResponse(new Response(true, null));
			string userJson =
				serializer.SerializeResponse(new UsersFetchResponse(true, null, DummyData.User()));

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

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			Assert.That(invoked, Is.True);
		}

		[Test]
		public async Task Authenticate_DoesNotInvokeAuthenticateEvent_Failure()
		{
			string authJson = serializer.SerializeResponse(new Response(false, GameJoltAuthenticationException.MESSAGE));
			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(FromResult(authJson));

			bool invoked = false;
			GameJoltAPI.Users.OnUserAuthenticated += user => { invoked = true; };

			GameJoltResult result = await GameJoltAPI.Users.AuthenticateAsync("test", "test");

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception, Is.TypeOf<GameJoltAuthenticationException>());
			Assert.That(invoked, Is.False);
		}

		[Test]
		[TestCase("https://gamejolt.com/game/bla/bla/bla?gjapi_username=test&gjapi_token=test")]
		[TestCase("https://gamejolt.net/game/bla/bla/bla?gjapi_username=test&gjapi_token=test")]
		public async Task Authenticate_Url_Success(string url)
		{
			string authJson = serializer.SerializeResponse(new Response(true, null));
			string userJson = serializer.SerializeResponse(new UsersFetchResponse(true, null, DummyData.User()));

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

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
		}

		[Test]
		[TestCase("https://gamejolt.com/game/bla/bla/bla?gjapi_username=test&gjapi_token=test")]
		[TestCase("https://gamejolt.net/game/bla/bla/bla?gjapi_username=test&gjapi_token=test")]
		public async Task Authenticate_Url_Failure(string url)
		{
			string authJson = serializer.SerializeResponse(new Response(false, GameJoltAuthenticationException.MESSAGE));
			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(FromResult(authJson));

			GameJoltResult result = await GameJoltAPI.Users.AuthenticateFromUrlAsync(url);

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception, Is.TypeOf<GameJoltAuthenticationException>());
		}

		[Test]
		public async Task Authenticate_InvalidUrl_Failure()
		{
			GameJoltResult result = await GameJoltAPI.Users.AuthenticateFromUrlAsync("https://google.com");

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception, Is.TypeOf<ArgumentException>());
		}

		[Test]
		[TestCase(CREDENTIALS_NORMAL)]
		[TestCase(CREDENTIALS_LONG)]
		public async Task Authenticate_CredentialsFile_Success(string credentials)
		{
			string authJson = serializer.SerializeResponse(new Response(true, null));
			string userJson = serializer.SerializeResponse(new UsersFetchResponse(true, null, DummyData.User()));

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

			GameJoltResult result = await GameJoltAPI.Users.AuthenticateFromCredentialsFileAsync(credentials);

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
		}

		[Test]
		public async Task Authenticate_CredentialsFile_InvalidFile_Failure()
		{
			GameJoltResult result = await GameJoltAPI.Users.AuthenticateFromCredentialsFileAsync("invalid");

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception, Is.TypeOf<ArgumentException>());
		}
		
		[Test]
		public async Task Authenticate_CredentialsFile_Null_Failure()
		{
			GameJoltResult result = await GameJoltAPI.Users.AuthenticateFromCredentialsFileAsync((string) null!);

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception, Is.TypeOf<ArgumentException>());
		}
		
		[Test]
		public async Task Authenticate_CredentialsFile_Empty_Failure()
		{
			GameJoltResult result = await GameJoltAPI.Users.AuthenticateFromCredentialsFileAsync(string.Empty);

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception, Is.TypeOf<ArgumentException>());
		}
		
		[Test]
		public async Task Authenticate_CredentialsFile_Whitespace_Failure()
		{
			GameJoltResult result = await GameJoltAPI.Users.AuthenticateFromCredentialsFileAsync(" ");

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception, Is.TypeOf<ArgumentException>());
		}

		[Test]
		public async Task Fetch_ValidUsername_Success()
		{
			User user = DummyData.User();

			string userJson = serializer.SerializeResponse(new UsersFetchResponse(true, null, user));

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("users/?"))
				{
					return FromResult(userJson);
				}

				return FromResult("");
			});

			GameJoltResult<GameJoltUser> result = await GameJoltAPI.Users.GetUserAsync("Username");

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			Assert.That(result.Value, Is.EqualTo(user.ToPublicUser()));
		}

		[Test]
		public async Task Fetch_ValidId_Success()
		{
			User user = DummyData.User();

			string userJson = serializer.SerializeResponse(new UsersFetchResponse(true, null, user));

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("users/?"))
				{
					return FromResult(userJson);
				}

				return FromResult("");
			});

			GameJoltResult<GameJoltUser> result = await GameJoltAPI.Users.GetUserAsync(0);

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			Assert.That(result.Value, Is.EqualTo(user.ToPublicUser()));
		}

		[Test]
		public async Task Fetch_InvalidUsername_Failure()
		{
			string userJson = serializer.SerializeResponse(new UsersFetchResponse(false, GameJoltInvalidUserException.MESSAGE, Array.Empty<User>()));

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("users/?"))
				{
					return FromResult(userJson);
				}

				return FromResult("");
			});

			GameJoltResult<GameJoltUser> result = await GameJoltAPI.Users.GetUserAsync("Username");

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception, Is.TypeOf<GameJoltInvalidUserException>());
		}

		[Test]
		public async Task Fetch_InvalidId_Failure()
		{
			string userJson = serializer.SerializeResponse(new UsersFetchResponse(false, GameJoltInvalidUserException.MESSAGE, Array.Empty<User>()));

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("users/?"))
				{
					return FromResult(userJson);
				}

				return FromResult("");
			});

			GameJoltResult<GameJoltUser> result = await GameJoltAPI.Users.GetUserAsync(0);

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception, Is.TypeOf<GameJoltInvalidUserException>());
		}

		[Test]
		public async Task Fetch_Usernames_Success()
		{
			User user1 = DummyData.User();
			User user2 = DummyData.User();

			string userJson = serializer.SerializeResponse(new UsersFetchResponse(true, null, new[] { user1, user2 }));

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("username="))
				{
					return FromResult(userJson);
				}

				return FromResult("");
			});

			GameJoltResult<GameJoltUser[]> result = await GameJoltAPI.Users.GetUsersAsync(usernames);

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			Assert.That(result.Value, Is.Not.Null);
			Assert.That(result.Value, Has.Length.EqualTo(2));
			Assert.That(result.Value![0], Is.EqualTo(user1.ToPublicUser()));
			Assert.That(result.Value![1], Is.EqualTo(user2.ToPublicUser()));
		}

		[Test]
		public async Task Fetch_Ids_Success()
		{
			User user1 = DummyData.User();
			User user2 = DummyData.User();

			string userJson = serializer.SerializeResponse(new UsersFetchResponse(true, null, new[] { user1, user2 }));

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("user_id="))
				{
					return FromResult(userJson);
				}

				return FromResult("");
			});

			GameJoltResult<GameJoltUser[]> result = await GameJoltAPI.Users.GetUsersAsync(ids);

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			Assert.That(result.Value, Is.Not.Null);
			Assert.That(result.Value, Has.Length.EqualTo(2));
			Assert.That(result.Value![0], Is.EqualTo(user1.ToPublicUser()));
			Assert.That(result.Value![1], Is.EqualTo(user2.ToPublicUser()));
		}

		[Test]
		public async Task Fetch_Usernames_Failure()
		{
			string userJson = serializer.SerializeResponse(new UsersFetchResponse(false, GameJoltInvalidUserException.MESSAGE, Array.Empty<User>()));

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("username="))
				{
					return FromResult(userJson);
				}

				return FromResult("");
			});

			GameJoltResult<GameJoltUser[]> result = await GameJoltAPI.Users.GetUsersAsync(usernames);

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception, Is.TypeOf<GameJoltInvalidUserException>());
		}

		[Test]
		public async Task Fetch_Ids_Failure()
		{
			string userJson = serializer.SerializeResponse(new UsersFetchResponse(false, GameJoltInvalidUserException.MESSAGE, Array.Empty<User>()));

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("user_id="))
				{
					return FromResult(userJson);
				}

				return FromResult("");
			});

			GameJoltResult<GameJoltUser[]> result = await GameJoltAPI.Users.GetUsersAsync(ids);

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception, Is.TypeOf<GameJoltInvalidUserException>());
		}

		[Test]
		public async Task Fetch_Usernames_NullArgument()
		{
			GameJoltResult<GameJoltUser[]> result = await GameJoltAPI.Users.GetUsersAsync((IEnumerable<string>) null!);

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception, Is.TypeOf<ArgumentNullException>());
		}

		[Test]
		public async Task Fetch_Ids_NullArgument()
		{
			GameJoltResult<GameJoltUser[]> result = await GameJoltAPI.Users.GetUsersAsync((IEnumerable<int>) null!);

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception, Is.TypeOf<ArgumentNullException>());
		}

		[Test]
		[TestCase("Username")]
		[TestCase(12345)]
		public async Task FetchUser_ReturnsNullUser(object argument)
		{
			string userJson = serializer.SerializeResponse(new UsersFetchResponse(true, null, null));

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("username=") || arg.Contains("user_id="))
				{
					return FromResult(userJson);
				}

				return FromResult("");
			});

			GameJoltResult<GameJoltUser> result;
			switch (argument)
			{
				case string username:
					result = await GameJoltAPI.Users.GetUserAsync(username);
					break;
				case int id:
					result = await GameJoltAPI.Users.GetUserAsync(id);
					break;
				default:
					throw new ArgumentException("Invalid argument type.");
			}

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception, Is.TypeOf<GameJoltInvalidUserException>());
		}

		[Test]
		public async Task FetchUsers_Usernames_ReturnsNullUser()
		{
			string userJson = serializer.SerializeResponse(new UsersFetchResponse(true, null, null));

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("username=") || arg.Contains("user_id="))
				{
					return FromResult(userJson);
				}

				return FromResult("");
			});

			GameJoltResult<GameJoltUser[]> result = await GameJoltAPI.Users.GetUsersAsync(usernames);

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception, Is.TypeOf<GameJoltInvalidUserException>());
		}

		[Test]
		public async Task FetchUsers_Ids_ReturnsNullUser()
		{
			string userJson = serializer.SerializeResponse(new UsersFetchResponse(true, null, null));

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("username=") || arg.Contains("user_id="))
				{
					return FromResult(userJson);
				}

				return FromResult("");
			});

			GameJoltResult<GameJoltUser[]> result = await GameJoltAPI.Users.GetUsersAsync(ids);

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception, Is.TypeOf<GameJoltInvalidUserException>());
		}

		[Test]
		public async Task Authenticate_ValidUrl()
		{
			await TestUrlAsync(() => GameJoltAPI.Users.AuthenticateAsync("Username", "Token"),
				url =>
				{
					Assert.That(url, Does.StartWith(GameJoltUrlBuilder.BASE_URL + GameJoltUsers.AUTH_ENDPOINT + "?username=Username&user_token=Token"));
				});
		}

		[Test]
		public async Task Fetch_Username_ValidUrl()
		{
			await TestUrlAsync(() => GameJoltAPI.Users.GetUserAsync("Username"),
				url => { Assert.That(url, Does.StartWith(GameJoltUrlBuilder.BASE_URL + GameJoltUsers.ENDPOINT + "?username=Username")); });
		}

		[Test]
		public async Task Fetch_Id_ValidUrl()
		{
			await TestUrlAsync(() => GameJoltAPI.Users.GetUserAsync(0),
				url => { Assert.That(url, Does.StartWith(GameJoltUrlBuilder.BASE_URL + GameJoltUsers.ENDPOINT + "?user_id=0")); });
		}

		[Test]
		public async Task Fetch_Usernames_ValidUrl()
		{
			await TestUrlAsync(() => GameJoltAPI.Users.GetUsersAsync(usernames),
				url => { Assert.That(url, Does.StartWith(GameJoltUrlBuilder.BASE_URL + GameJoltUsers.ENDPOINT + "?username=Username,Username2")); });
		}

		[Test]
		public async Task Fetch_Ids_ValidUrl()
		{
			await TestUrlAsync(() => GameJoltAPI.Users.GetUsersAsync(ids),
				url => { Assert.That(url, Does.StartWith(GameJoltUrlBuilder.BASE_URL + GameJoltUsers.ENDPOINT + "?user_id=0,1")); });
		}
	}
}
#endif // DISABLE_GAMEJOLT