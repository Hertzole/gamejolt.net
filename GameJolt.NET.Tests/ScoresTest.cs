using System.Threading.Tasks;
using Hertzole.GameJolt;
using NSubstitute;
using NUnit.Framework;

namespace GameJolt.NET.Tests
{
	public class ScoresTest : BaseTest
	{
		[Test]
		public async Task SubmitScore_Authenticated_Success()
		{
			await AuthenticateAsync();

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains(GameJoltScores.ADD_ENDPOINT))
				{
					return FromResult(serializer.Serialize(new SubmitScoreResponse(true, null)));
				}

				return FromResult("");
			});

			GameJoltResult result = await GameJoltAPI.Scores.SubmitScoreAsync(0, 0, "0", "Extra Data");

			Assert.IsFalse(result.HasError);
			Assert.IsNull(result.Exception);
		}

		[Test]
		public async Task SubmitScoreAsGuest_Success()
		{
			await AuthenticateAsync();

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains(GameJoltScores.ADD_ENDPOINT))
				{
					return FromResult(serializer.Serialize(new SubmitScoreResponse(true, null)));
				}

				return FromResult("");
			});

			GameJoltResult result = await GameJoltAPI.Scores.SubmitScoreAsGuestAsync(0, "Guest", 0, "0", "Extra Data");

			Assert.IsFalse(result.HasError);
			Assert.IsNull(result.Exception);
		}

		[Test]
		public async Task SubmitScore_NotAuthenticated_Fail()
		{
			GameJoltResult result = await GameJoltAPI.Scores.SubmitScoreAsync(0, 0, "0", "Extra Data");

			Assert.IsTrue(result.HasError);
			Assert.IsNotNull(result.Exception);
			Assert.IsTrue(result.Exception is GameJoltAuthorizedException);
		}

		[Test]
		public async Task GetRank_Success()
		{
			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains(GameJoltScores.GET_RANK_ENDPOINT))
				{
					return FromResult(serializer.Serialize(new GetScoreRankResponse(true, null, 0)));
				}

				return FromResult("");
			});

			GameJoltResult<int> result = await GameJoltAPI.Scores.GetRankAsync(0, 0);

			Assert.IsFalse(result.HasError);
			Assert.IsNull(result.Exception);
			Assert.That(result.Value, Is.EqualTo(0));
		}

		[Test]
		public async Task GetTables_Success()
		{
			TableInternal table = CreateDummyTable();

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains(GameJoltScores.GET_TABLES_ENDPOINT))
				{
					return FromResult(serializer.Serialize(new GetTablesResponse(true, null, new TableInternal[1]
					{
						table
					})));
				}

				return FromResult("");
			});

			GameJoltResult<GameJoltTable[]> result = await GameJoltAPI.Scores.GetTablesAsync();

			Assert.IsFalse(result.HasError);
			Assert.IsNull(result.Exception);
			Assert.That(result.Value, Is.Not.Null);
			Assert.That(result.Value.Length, Is.EqualTo(1));
			Assert.That(result.Value[0].Id, Is.EqualTo(table.id));
			Assert.That(result.Value[0].Name, Is.EqualTo(table.name));
			Assert.That(result.Value[0].Description, Is.EqualTo(table.description));
			Assert.That(result.Value[0].IsPrimary, Is.EqualTo(table.isPrimary));
		}

		[Test]
		public async Task Query_Success()
		{
			ScoreInternal score = CreateDummyScore();

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains(GameJoltScores.ENDPOINT))
				{
					return FromResult(serializer.Serialize(new GetScoresResponse(true, null, new ScoreInternal[1]
					{
						score
					})));
				}

				return FromResult("");
			});

			GameJoltResult<GameJoltScore[]> result = await GameJoltAPI.Scores.QueryScores().ForTable(0).Limit(0).ForUser("test", "test")
			                                                          .BetterThan(0).WorseThan(0).GetAsync();

			Assert.IsFalse(result.HasError);
			Assert.IsNull(result.Exception);
			Assert.That(result.Value, Is.Not.Null);
			Assert.That(result.Value.Length, Is.EqualTo(1));
			Assert.That(result.Value[0].Score, Is.EqualTo(score.score));
			Assert.That(result.Value[0].Sort, Is.EqualTo(score.sort));
			Assert.That(result.Value[0].ExtraData, Is.EqualTo(score.extraData));
			Assert.That(result.Value[0].UserId, Is.EqualTo(score.userId));
			Assert.That(result.Value[0].Username, Is.EqualTo(score.username));
			Assert.That(result.Value[0].GuestName, Is.EqualTo(score.guestName));
			Assert.That(result.Value[0].Stored, Is.EqualTo(DateTimeHelper.FromUnixTimestamp(score.storedTimestamp)));

			result = await GameJoltAPI.Scores.QueryScores().ForTable(0).Limit(0).ForGuest("test").BetterThan(0).WorseThan(0).GetAsync();

			Assert.IsFalse(result.HasError);
			Assert.IsNull(result.Exception);
			Assert.That(result.Value, Is.Not.Null);
			Assert.That(result.Value.Length, Is.EqualTo(1));
			Assert.That(result.Value[0].Score, Is.EqualTo(score.score));
			Assert.That(result.Value[0].Sort, Is.EqualTo(score.sort));
			Assert.That(result.Value[0].ExtraData, Is.EqualTo(score.extraData));
			Assert.That(result.Value[0].UserId, Is.EqualTo(score.userId));
			Assert.That(result.Value[0].Username, Is.EqualTo(score.username));
			Assert.That(result.Value[0].GuestName, Is.EqualTo(score.guestName));
			Assert.That(result.Value[0].Stored, Is.EqualTo(DateTimeHelper.FromUnixTimestamp(score.storedTimestamp)));
		}

		[Test]
		public void ScoreDisplayName_Username()
		{
			string? name = faker.Internet.UserName();

			GameJoltScore score = new GameJoltScore(0, "", "", name, 0, "", default);

			Assert.That(score.DisplayName, Is.EqualTo(name));
		}
		
		[Test]
		public void ScoreDisplayName_GuestName()
		{
			string? name = faker.Internet.UserName();

			GameJoltScore score = new GameJoltScore(0, "", "", "", null, name, default);

			Assert.That(score.DisplayName, Is.EqualTo(name));
		}
		
		[Test]
		public void ScoreDisplayName_Empty()
		{
			GameJoltScore score = new GameJoltScore(0, "", "", "", null, "", default);

			Assert.That(score.DisplayName, Is.EqualTo(string.Empty));
		}
	}
}