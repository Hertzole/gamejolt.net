using System.Threading.Tasks;
using Hertzole.GameJolt;
using NSubstitute;
using NUnit.Framework;

namespace GameJolt.NET.Tests
{
	public class SessionsTest : BaseTest
	{
		[Test]
		public async Task Open_Authenticated_Success()
		{
			await AuthenticateAsync();

			string json = serializer.Serialize(new SessionResponse(true, null));

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("sessions/open"))
				{
					return FromResult(json);
				}

				return FromResult("");
			});

			GameJoltResult result = await GameJoltAPI.Sessions.OpenAsync();

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			Assert.That(GameJoltAPI.Sessions.IsSessionOpen, Is.True);
		}

		[Test]
		public async Task Open_AlreadyOpen_Fail()
		{
			await AuthenticateAsync();

			string json = serializer.Serialize(new SessionResponse(true, null));

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("sessions/open"))
				{
					return FromResult(json);
				}

				return FromResult("");
			});

			GameJoltResult result = await GameJoltAPI.Sessions.OpenAsync();

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			Assert.That(GameJoltAPI.Sessions.IsSessionOpen, Is.True);

			GameJoltResult result2 = await GameJoltAPI.Sessions.OpenAsync();

			Assert.That(result2.HasError, Is.True);
			Assert.That(result2.Exception is GameJoltSessionException, Is.True);
			Assert.That(result2.Exception!.Message, Is.EqualTo(GameJoltSessions.SESSION_ALREADY_OPEN));
			Assert.That(GameJoltAPI.Sessions.IsSessionOpen, Is.True);
		}

		[Test]
		public async Task Open_NotAuthenticated_Fail()
		{
			string json = serializer.Serialize(new SessionResponse(false, null));

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("sessions/open"))
				{
					return FromResult(json);
				}

				return FromResult("");
			});

			GameJoltResult result = await GameJoltAPI.Sessions.OpenAsync();

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception is GameJoltAuthorizedException, Is.True);
			Assert.That(GameJoltAPI.Sessions.IsSessionOpen, Is.False);
		}

		[Test]
		public async Task Close_Authenticated_Success()
		{
			await AuthenticateAsync();

			string json = serializer.Serialize(new SessionResponse(true, null));

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("sessions/close") || arg.Contains("sessions/open"))
				{
					return FromResult(json);
				}

				return FromResult("");
			});

			await GameJoltAPI.Sessions.OpenAsync();
			Assert.That(GameJoltAPI.Sessions.IsSessionOpen, Is.True);

			GameJoltResult result = await GameJoltAPI.Sessions.CloseAsync();

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			Assert.That(GameJoltAPI.Sessions.IsSessionOpen, Is.False);
		}

		[Test]
		public async Task Close_NotAuthenticated_Fail()
		{
			string json = serializer.Serialize(new SessionResponse(false, null));

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("sessions/close"))
				{
					return FromResult(json);
				}

				return FromResult("");
			});

			GameJoltResult result = await GameJoltAPI.Sessions.CloseAsync();

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception is GameJoltAuthorizedException, Is.True);
			Assert.That(GameJoltAPI.Sessions.IsSessionOpen, Is.False);
		}

		[Test]
		public async Task Close_NotOpen_Fail()
		{
			await AuthenticateAsync();

			string json = serializer.Serialize(new SessionResponse(true, null));

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("sessions/close") || arg.Contains("sessions/open"))
				{
					return FromResult(json);
				}

				return FromResult("");
			});

			GameJoltResult result = await GameJoltAPI.Sessions.CloseAsync();

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception is GameJoltSessionException, Is.True);
			Assert.That(result.Exception!.Message, Is.EqualTo(GameJoltSessions.CANT_CLOSE_SESSION));
			Assert.That(GameJoltAPI.Sessions.IsSessionOpen, Is.False);
		}

		[Test]
		[TestCase(SessionStatus.Active)]
		[TestCase(SessionStatus.Idle)]
		public async Task Ping_Authenticated_Success(SessionStatus status)
		{
			await AuthenticateAsync();

			string json = serializer.Serialize(new SessionResponse(true, null));

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("sessions/ping") || arg.Contains("sessions/open"))
				{
					return FromResult(json);
				}

				return FromResult("");
			});

			await GameJoltAPI.Sessions.OpenAsync();
			Assert.That(GameJoltAPI.Sessions.IsSessionOpen, Is.True);

			GameJoltResult result = await GameJoltAPI.Sessions.PingAsync(status);

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			Assert.That(GameJoltAPI.Sessions.IsSessionOpen, Is.True);
		}

		[Test]
		public async Task Ping_NotAuthenticated_Fail()
		{
			string json = serializer.Serialize(new SessionResponse(false, null));

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("sessions/ping"))
				{
					return FromResult(json);
				}

				return FromResult("");
			});

			GameJoltResult result = await GameJoltAPI.Sessions.PingAsync(SessionStatus.Active);

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception is GameJoltAuthorizedException, Is.True);
			Assert.That(GameJoltAPI.Sessions.IsSessionOpen, Is.False);
		}

		[Test]
		public async Task Ping_NotOpen_Fail()
		{
			await AuthenticateAsync();

			string json = serializer.Serialize(new SessionResponse(true, null));

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("sessions/ping") || arg.Contains("sessions/open"))
				{
					return FromResult(json);
				}

				return FromResult("");
			});

			GameJoltResult result = await GameJoltAPI.Sessions.PingAsync(SessionStatus.Active);

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception is GameJoltSessionException, Is.True);
			Assert.That(result.Exception!.Message, Is.EqualTo(GameJoltSessions.CANT_PING_SESSION));
			Assert.That(GameJoltAPI.Sessions.IsSessionOpen, Is.False);
		}

		[Test]
		public async Task Check_Authenticated_OpenSession_Success()
		{
			await AuthenticateAsync();

			string json = serializer.Serialize(new SessionResponse(true, null));

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.ArgAt<string>(0);

				if (arg.Contains("sessions/check") || arg.Contains("sessions/open"))
				{
					return FromResult(json);
				}

				return FromResult("");
			});

			await GameJoltAPI.Sessions.OpenAsync();
			Assert.That(GameJoltAPI.Sessions.IsSessionOpen, Is.True);

			GameJoltResult<bool> result = await GameJoltAPI.Sessions.CheckAsync();

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			Assert.That(result.Value, Is.True);
			Assert.That(GameJoltAPI.Sessions.IsSessionOpen, Is.True);
		}

		[Test]
		public async Task Check_Authenticated_ClosedSession_Success()
		{
			await AuthenticateAsync();

			string json = serializer.Serialize(new SessionResponse(false, null));

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("sessions/check") || arg.Contains("sessions/open"))
				{
					return FromResult(json);
				}

				return FromResult("");
			});

			GameJoltResult<bool> result = await GameJoltAPI.Sessions.CheckAsync();

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			Assert.That(result.Value, Is.False);
			Assert.That(GameJoltAPI.Sessions.IsSessionOpen, Is.False);
		}

		[Test]
		public async Task Check_NotAuthenticated_Fail()
		{
			string json = serializer.Serialize(new SessionResponse(false, null));

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("sessions/check"))
				{
					return FromResult(json);
				}

				return FromResult("");
			});

			GameJoltResult<bool> result = await GameJoltAPI.Sessions.CheckAsync();

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception is GameJoltAuthorizedException, Is.True);
			Assert.That(GameJoltAPI.Sessions.IsSessionOpen, Is.False);
		}

		[Test]
		public async Task Open_ValidUrl()
		{
			await AuthenticateAsync();

			await TestUrlAsync(() => GameJoltAPI.Sessions.OpenAsync(),
				url =>
				{
					Assert.That(url, Does.StartWith(GameJoltUrlBuilder.BASE_URL + GameJoltSessions.OPEN_ENDPOINT + $"?username={Username}&user_token={Token}"));
				});
		}
		
		[Test]
		public async Task Close_ValidUrl()
		{
			await AuthenticateAsync();

			await TestUrlAsync(() => GameJoltAPI.Sessions.CloseAsync(),
				url =>
				{
					Assert.That(url, Does.StartWith(GameJoltUrlBuilder.BASE_URL + GameJoltSessions.CLOSE_ENDPOINT + $"?username={Username}&user_token={Token}"));
				});
		}
		
		[Test]
		[TestCase(SessionStatus.Active)]
		[TestCase(SessionStatus.Idle)]
		public async Task Ping_ValidUrl(SessionStatus status)
		{
			await AuthenticateAsync();

			await TestUrlAsync(() => GameJoltAPI.Sessions.PingAsync(status),
				url =>
				{
					Assert.That(url, Does.StartWith(GameJoltUrlBuilder.BASE_URL + GameJoltSessions.PING_ENDPOINT + $"?username={Username}&user_token={Token}&status={GameJoltSessions.GetStatusString(status)}"));
				});
		}
		
		[Test]
		public async Task Check_ValidUrl()
		{
			await AuthenticateAsync();

			await TestUrlAsync(() => GameJoltAPI.Sessions.CheckAsync(),
				url =>
				{
					Assert.That(url, Does.StartWith(GameJoltUrlBuilder.BASE_URL + GameJoltSessions.CHECK_ENDPOINT + $"?username={Username}&user_token={Token}"));
				});
		}
	}
}