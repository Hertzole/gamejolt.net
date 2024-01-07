using System;
using System.Threading.Tasks;
using Bogus;
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

		internal static readonly Randomizer randomizer = new Randomizer();
		internal static readonly Faker faker = new Faker();

		[SetUp]
		public async Task Setup()
		{
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

		public static async Task AuthenticateAsync()
		{
			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("users/?"))
				{
					return FromResult(serializer.Serialize(new UsersFetchResponse(true, null, CreateDummyUser())));
				}

				if (arg.Contains("users/auth"))
				{
					return FromResult(serializer.Serialize(new AuthResponse(true, null)));
				}

				return FromResult("");
			});

			GameJoltResult result = await GameJoltAPI.Users.AuthenticateAsync("test", "test");

			Assert.IsFalse(result.HasError);
			Assert.IsNull(result.Exception);
			Assert.IsTrue(GameJoltAPI.Users.IsAuthenticated);
		}

		internal static User CreateDummyUser()
		{
			int id = randomizer.Int(1, (int) (int.MaxValue * 0.5f));
			UserType type = (UserType) randomizer.Int(0, 3);
			string username = faker.Internet.UserName();
			string avatarUrl = faker.Internet.Avatar();
			DateTime signedUp = faker.Date.Past();
			DateTime lastLoggedIn = faker.Date.Past();
			UserStatus status = randomizer.Enum<UserStatus>();
			string displayName = faker.Name.FullName();
			string? userWebsite = faker.Internet.Url();
			string userDescription = faker.Lorem.Sentence();

			string signedUpString = signedUp.ToString("MMMM d, yyyy");
			string lastLoggedInString = lastLoggedIn.ToString("MMMM d, yyyy");

			int signedUpTimestamp = (int) (signedUp - new DateTime(1970, 1, 1)).TotalSeconds;
			int lastLoggedInTimestamp = (int) (lastLoggedIn - new DateTime(1970, 1, 1)).TotalSeconds;

			return new User(id, type, username, avatarUrl, signedUpString, signedUpTimestamp, lastLoggedInString, lastLoggedInTimestamp, status,
				displayName, userWebsite, userDescription);
		}

		internal static TableInternal CreateDummyTable()
		{
			int id = randomizer.Int(1, (int) (int.MaxValue * 0.5f));
			string name = faker.Lorem.Word();
			string description = faker.Lorem.Sentence();
			bool isPrimary = randomizer.Bool();

			return new TableInternal(id, name, description, isPrimary);
		}

		internal static ScoreInternal CreateDummyScore()
		{
			int sort = randomizer.Int(1, (int) (int.MaxValue * 0.5f));
			string score = sort.ToString();
			string extraData = faker.Lorem.Sentence();
			int userId = randomizer.Int(1, (int) (int.MaxValue * 0.5f));
			string username = faker.Internet.UserName();
			string guestName = faker.Name.FullName();
			DateTime stored = faker.Date.Past();

			return new ScoreInternal(sort, score, extraData, username, userId, guestName, stored.ToString("MMMM d, yyyy"),
				(int) (stored - new DateTime(1970, 1, 1)).TotalSeconds);
		}

		internal static TrophyInternal CreateDummyTrophy()
		{
			int id = randomizer.Int(1, (int) (int.MaxValue * 0.5f));
			string title = faker.Lorem.Word();
			string description = faker.Lorem.Sentence();
			TrophyDifficulty difficulty = randomizer.Enum<TrophyDifficulty>();
			string imageUrl = faker.Internet.Avatar();
			bool achieved = randomizer.Bool();

			return new TrophyInternal(id, title, description, difficulty, imageUrl, achieved);
		}

		internal static byte[] CreateDummyBytes()
		{
			return randomizer.Bytes(1024);
		}

		protected static StringTask FromResult(string result)
		{
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER || UNITY_2021_3_OR_NEWER
			return ValueTask.FromResult(result);
#else
			return Task.FromResult(result);
#endif
		}
	}
}