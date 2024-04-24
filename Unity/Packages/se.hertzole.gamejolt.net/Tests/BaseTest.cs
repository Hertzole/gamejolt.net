#nullable enable

using System;
using System.Threading.Tasks;
using GameJolt.NET.Tests.Extensions;
using Hertzole.GameJolt;
using NSubstitute;
using NUnit.Framework;
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER || UNITY_2021_3_OR_NEWER
using StringTask = System.Threading.Tasks.ValueTask<string>;

#else
using StringTask = System.Threading.Tasks.Task<string>;
#endif

namespace GameJolt.NET.Tests
{
	[TestFixture]
	public abstract class BaseTest
	{
		internal static readonly IGameJoltSerializer serializer = GameJoltAPI.serializer;

		protected string Username { get; set; } = null!;

		protected string Token { get; set; } = null!;

#if UNITY_64
		private float originalPingInterval;
#if UNITY_EDITOR
		private string? originalSignInUsername;
		private string? originalSignInToken;
#endif
#endif

		[SetUp]
		public async Task Setup()
		{
#if UNITY_64
			originalPingInterval = GameJoltSettings.PingInterval;

			Assert.That(GameJoltSettings.AutoInitialize, Is.False, "AutoInitialize is not supported when running tests.");
			Assert.That(GameJoltSettings.AutoShutdown, Is.False, "AutoShutdown is not supported when running tests.");
			Assert.That(GameJoltSettings.AutoSignInFromWeb, Is.False, "AutoSignInFromWeb is not supported when running tests.");
			Assert.That(GameJoltSettings.AutoSignInFromClient, Is.False, "AutoSignInFromClient is not supported when running tests.");
			Assert.That(GameJoltSettings.AutoStartSessions, Is.False, "AutoStartSessions is not supported when running tests.");
			Assert.That(GameJoltSettings.AutoCloseSessions, Is.False, "AutoCloseSessions is not supported when running tests.");
			Assert.That(GameJoltSettings.AutoPingSessions, Is.False, "AutoPingSessions is not supported when running tests.");
#if UNITY_EDITOR
			Assert.That(GameJoltSettings.AutoSignIn, Is.False, "AutoSignIn is not supported when running tests.");

			originalSignInUsername = GameJoltSettings.SignInUsername;
			originalSignInToken = GameJoltSettings.SignInToken;
#endif
#endif

			Username = DummyData.faker.Internet.UserName();
			Token = DummyData.faker.Random.AlphaNumeric(6);

#if UNITY_EDITOR
			GameJoltSettings.SignInUsername = Username;
			GameJoltSettings.SignInToken = Token;
#endif

			GameJoltAPI.webClient = Substitute.For<IGameJoltWebClient>();

			if (!TestContext.CurrentContext.Test.HasSkipInitializationAttribute())
			{
				GameJoltAPI.Initialize(0, "");
			}

			await OnSetupAsync();
		}

		protected virtual Task OnSetupAsync()
		{
			return Task.CompletedTask;
		}

		[TearDown]
		public async Task TearDown()
		{
			if (GameJoltAPI.IsInitialized)
			{
				GameJoltAPI.Shutdown();
			}

			Assert.That(GameJoltAPI.IsInitialized, Is.False, "GameJoltAPI is initialized.");

#if UNITY_64
			GameJoltSettings.PingInterval = originalPingInterval;

			GameJoltSettings.AutoInitialize = false;
			GameJoltSettings.AutoShutdown = false;
			GameJoltSettings.AutoSignInFromWeb = false;
			GameJoltSettings.AutoSignInFromClient = false;
			GameJoltSettings.AutoStartSessions = false;
			GameJoltSettings.AutoCloseSessions = false;
			GameJoltSettings.AutoPingSessions = false;
#if UNITY_EDITOR
			GameJoltSettings.AutoSignIn = false;
			GameJoltSettings.SignInUsername = originalSignInUsername;
			GameJoltSettings.SignInToken = originalSignInToken;
#endif
#endif

			await OnTearDownAsync();
		}

		protected virtual Task OnTearDownAsync()
		{
			return Task.CompletedTask;
		}

		protected async Task AuthenticateAsync()
		{
			SetUpWebClientForAuth();

			GameJoltResult result = await GameJoltAPI.Users.AuthenticateAsync(Username, Token);

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			Assert.That(GameJoltAPI.Users.IsAuthenticated, Is.True);
		}

		protected void SetUpWebClientForAuth()
		{
			GameJoltAPI.webClient.GetStringAsync("https://api.gamejolt.com/api/game/v1_2/users/auth", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("users/?"))
				{
					return FromResult(serializer.SerializeResponse(new UsersFetchResponse(true, null, DummyData.User(username: Username))));
				}

				if (arg.Contains("users/auth"))
				{
					return FromResult(serializer.SerializeResponse(new Response(true, null)));
				}

				if (arg.Contains("sessions/close") || arg.Contains("sessions/open") || arg.Contains("sessions/ping") || arg.Contains("sessions/check"))
				{
					return FromResult(serializer.SerializeResponse(new Response(true, null)));
				}

				return FromResult("");
			});
		}

		protected static async Task TestUrlAsync(Func<Task> call, Action<string> assert)
		{
			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string url = info.Arg<string>();

				assert.Invoke(url);

				return FromResult("");
			});

			try
			{
				await call.Invoke();
			}
			catch (Exception)
			{
				// Do nothing
			}
		}

		protected static StringTask FromResult(string result)
		{
#if NET5_0_OR_GREATER
			return ValueTask.FromResult(result);
#elif NETSTANDARD2_1_OR_GREATER || UNITY_2021_3_OR_NEWER
			return new StringTask(result);
#else
			return Task.FromResult(result);
#endif
		}
	}
}