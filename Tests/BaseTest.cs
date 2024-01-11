using System;
using System.Threading.Tasks;
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

		[SetUp]
		public async Task Setup()
		{
			Username = DummyData.faker.Internet.UserName();
			Token = DummyData.faker.Random.AlphaNumeric(6);
			
			GameJoltAPI.webClient = Substitute.For<IGameJoltWebClient>();

			GameJoltAPI.Initialize(0, "");
			await OnSetupAsync();
		}

		protected virtual Task OnSetupAsync()
		{
			return Task.CompletedTask;
		}

		[TearDown]
		public async Task TearDown()
		{
			GameJoltAPI.Shutdown();
			await OnTearDownAsync();
		}

		protected virtual Task OnTearDownAsync()
		{
			return Task.CompletedTask;
		}

		protected async Task AuthenticateAsync()
		{
			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("users/?"))
				{
					return FromResult(serializer.Serialize(new UsersFetchResponse(true, null, DummyData.User())));
				}

				if (arg.Contains("users/auth"))
				{
					return FromResult(serializer.Serialize(new AuthResponse(true, null)));
				}

				return FromResult("");
			});

			GameJoltResult result = await GameJoltAPI.Users.AuthenticateAsync(Username, Token);

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			Assert.That(GameJoltAPI.Users.IsAuthenticated, Is.True);
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