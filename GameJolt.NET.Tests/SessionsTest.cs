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

			Assert.IsFalse(result.HasError);
			Assert.IsNull(result.Exception);
			Assert.IsTrue(GameJoltAPI.Sessions.IsSessionOpen);
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

			Assert.IsFalse(result.HasError);
			Assert.IsNull(result.Exception);
			Assert.IsTrue(GameJoltAPI.Sessions.IsSessionOpen);

			GameJoltResult result2 = await GameJoltAPI.Sessions.OpenAsync();

			Assert.IsTrue(result2.HasError);
			Assert.IsTrue(result2.Exception is GameJoltSessionException);
			Assert.That(result2.Exception!.Message, Is.EqualTo(GameJoltSessions.SESSION_ALREADY_OPEN));
			Assert.IsTrue(GameJoltAPI.Sessions.IsSessionOpen);
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

			Assert.IsTrue(result.HasError);
			Assert.IsTrue(result.Exception is GameJoltAuthorizedException);
			Assert.IsFalse(GameJoltAPI.Sessions.IsSessionOpen);
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
			Assert.IsTrue(GameJoltAPI.Sessions.IsSessionOpen);

			GameJoltResult result = await GameJoltAPI.Sessions.CloseAsync();

			Assert.IsFalse(result.HasError);
			Assert.IsNull(result.Exception);
			Assert.IsFalse(GameJoltAPI.Sessions.IsSessionOpen);
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

			Assert.IsTrue(result.HasError);
			Assert.IsTrue(result.Exception is GameJoltAuthorizedException);
			Assert.IsFalse(GameJoltAPI.Sessions.IsSessionOpen);
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

			Assert.IsTrue(result.HasError);
			Assert.IsTrue(result.Exception is GameJoltSessionException);
			Assert.That(result.Exception!.Message, Is.EqualTo(GameJoltSessions.CANT_CLOSE_SESSION));
			Assert.IsFalse(GameJoltAPI.Sessions.IsSessionOpen);
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
			Assert.IsTrue(GameJoltAPI.Sessions.IsSessionOpen);

			GameJoltResult result = await GameJoltAPI.Sessions.PingAsync(status);

			Assert.IsFalse(result.HasError);
			Assert.IsNull(result.Exception);
			Assert.IsTrue(GameJoltAPI.Sessions.IsSessionOpen);
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

			Assert.IsTrue(result.HasError);
			Assert.IsTrue(result.Exception is GameJoltAuthorizedException);
			Assert.IsFalse(GameJoltAPI.Sessions.IsSessionOpen);
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

			Assert.IsTrue(result.HasError);
			Assert.IsTrue(result.Exception is GameJoltSessionException);
			Assert.That(result.Exception!.Message, Is.EqualTo(GameJoltSessions.CANT_PING_SESSION));
			Assert.IsFalse(GameJoltAPI.Sessions.IsSessionOpen);
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
			Assert.IsTrue(GameJoltAPI.Sessions.IsSessionOpen);

			GameJoltResult<bool> result = await GameJoltAPI.Sessions.CheckAsync();

			Assert.IsFalse(result.HasError);
			Assert.IsNull(result.Exception);
			Assert.IsTrue(result.Value);
			Assert.IsTrue(GameJoltAPI.Sessions.IsSessionOpen);
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

			Assert.IsFalse(result.HasError);
			Assert.IsNull(result.Exception);
			Assert.IsFalse(result.Value);
			Assert.IsFalse(GameJoltAPI.Sessions.IsSessionOpen);
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

			Assert.IsTrue(result.HasError);
			Assert.IsTrue(result.Exception is GameJoltAuthorizedException);
			Assert.IsFalse(GameJoltAPI.Sessions.IsSessionOpen);
		}
	}
}