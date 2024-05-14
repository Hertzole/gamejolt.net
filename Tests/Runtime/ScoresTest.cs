#nullable enable

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
					return FromResult(serializer.SerializeResponse(new Response(true, null)));
				}

				return FromResult("");
			});

			GameJoltResult result = await GameJoltAPI.Scores.SubmitScoreAsync(0, 0, "0", "Extra Data");

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
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
					return FromResult(serializer.SerializeResponse(new Response(true, null)));
				}

				return FromResult("");
			});

			GameJoltResult result = await GameJoltAPI.Scores.SubmitScoreAsGuestAsync(0, "Guest", 0, "0", "Extra Data");

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
		}

		[Test]
		public async Task SubmitScore_NotAuthenticated_Fail()
		{
			GameJoltResult result = await GameJoltAPI.Scores.SubmitScoreAsync(0, 0, "0", "Extra Data");

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception is GameJoltAuthorizedException, Is.True);
		}

		[Test]
		public async Task SubmitScore_Error_Fail()
		{
			await AuthenticateAsync();

			await AssertErrorAsync<Response, GameJoltInvalidTableException>(CreateResponse, GetResult, GameJoltInvalidTableException.MESSAGE);
			return;

			Response CreateResponse()
			{
				return new Response(false, GameJoltInvalidTableException.MESSAGE);
			}
			
			Task<GameJoltResult> GetResult()
			{
				return GameJoltAPI.Scores.SubmitScoreAsync(0, 0, "0", "Extra Data");
			}
		}

		[Test]
		public async Task GetRank_Success()
		{
			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains(GameJoltScores.GET_RANK_ENDPOINT))
				{
					return FromResult(serializer.SerializeResponse(new GetScoreRankResponse(true, null, 0)));
				}

				return FromResult("");
			});

			GameJoltResult<int> result = await GameJoltAPI.Scores.GetRankAsync(0, 0);

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			Assert.That(result.Value, Is.EqualTo(0));
		}
		
		[Test]
		public async Task GetRank_Error_Fail()
		{
			await AssertErrorAsync<GetScoreRankResponse, int, GameJoltInvalidTableException>(CreateResponse, GetResult, GameJoltInvalidTableException.MESSAGE);
			return;

			GetScoreRankResponse CreateResponse()
			{
				return new GetScoreRankResponse(false, GameJoltInvalidTableException.MESSAGE, 0);
			}
			
			Task<GameJoltResult<int>> GetResult()
			{
				return GameJoltAPI.Scores.GetRankAsync(0, 0);
			}
		}

		[Test]
		public async Task GetTables_Success()
		{
			TableInternal table = DummyData.Table();

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains(GameJoltScores.GET_TABLES_ENDPOINT))
				{
					return FromResult(serializer.SerializeResponse(new GetTablesResponse(true, null, new TableInternal[1]
					{
						table
					})));
				}

				return FromResult("");
			});

			GameJoltResult<GameJoltTable[]> result = await GameJoltAPI.Scores.GetTablesAsync();

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			Assert.That(result.Value, Is.Not.Null);
			Assert.That(result.Value!.Length, Is.EqualTo(1));
			Assert.That(result.Value[0].Id, Is.EqualTo(table.id));
			Assert.That(result.Value[0].Name, Is.EqualTo(table.name));
			Assert.That(result.Value[0].Description, Is.EqualTo(table.description));
			Assert.That(result.Value[0].IsPrimary, Is.EqualTo(table.isPrimary));
		}
		
		[Test]
		public async Task GetTables_Error_Fail()
		{
			await AssertErrorAsync<GetTablesResponse, GameJoltTable[], GameJoltException>(CreateResponse, GetResult);
			return;

			GetTablesResponse CreateResponse()
			{
				return new GetTablesResponse(false, GameJoltException.UNKNOWN_FATAL_ERROR, null);
			}
			
			Task<GameJoltResult<GameJoltTable[]>> GetResult()
			{
				return GameJoltAPI.Scores.GetTablesAsync();
			}
		}

		[Test]
		public async Task Query_Success()
		{
			ScoreInternal score = DummyData.Score();

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains(GameJoltScores.ENDPOINT))
				{
					return FromResult(serializer.SerializeResponse(new GetScoresResponse(true, null, new ScoreInternal[1]
					{
						score
					})));
				}

				return FromResult("");
			});

			GameJoltResult<GameJoltScore[]> result = await GameJoltAPI.Scores.QueryScores().ForTable(0).Limit(0).ForCurrentUser()
			                                                          .BetterThan(0).WorseThan(0).GetAsync();

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			Assert.That(result.Value, Is.Not.Null);
			Assert.That(result.Value!.Length, Is.EqualTo(1));
			Assert.That(result.Value[0].Score, Is.EqualTo(score.score));
			Assert.That(result.Value[0].Sort, Is.EqualTo(score.sort));
			Assert.That(result.Value[0].ExtraData, Is.EqualTo(score.extraData));
			Assert.That(result.Value[0].UserId, Is.EqualTo(score.userId));
			Assert.That(result.Value[0].Username, Is.EqualTo(score.username));
			Assert.That(result.Value[0].GuestName, Is.EqualTo(score.guestName));
			Assert.That(result.Value[0].Stored, Is.EqualTo(DateTimeHelper.FromUnixTimestamp(score.storedTimestamp)));

			result = await GameJoltAPI.Scores.QueryScores().ForTable(0).Limit(0).ForGuest("test").BetterThan(0).WorseThan(0).GetAsync();

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			Assert.That(result.Value, Is.Not.Null);
			Assert.That(result.Value!.Length, Is.EqualTo(1));
			Assert.That(result.Value[0].Score, Is.EqualTo(score.score));
			Assert.That(result.Value[0].Sort, Is.EqualTo(score.sort));
			Assert.That(result.Value[0].ExtraData, Is.EqualTo(score.extraData));
			Assert.That(result.Value[0].UserId, Is.EqualTo(score.userId));
			Assert.That(result.Value[0].Username, Is.EqualTo(score.username));
			Assert.That(result.Value[0].GuestName, Is.EqualTo(score.guestName));
			Assert.That(result.Value[0].Stored, Is.EqualTo(DateTimeHelper.FromUnixTimestamp(score.storedTimestamp)));
		}

		[Test]
		public async Task Query_NoScores_Success()
		{
			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains(GameJoltScores.ENDPOINT))
				{
					return FromResult(serializer.SerializeResponse(new GetScoresResponse(true, null, null)));
				}

				return FromResult("");
			});

			GameJoltResult<GameJoltScore[]> result = await GameJoltAPI.Scores.QueryScores().ForTable(0).Limit(0).ForCurrentUser()
			                                                          .BetterThan(0).WorseThan(0).GetAsync();

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
			Assert.That(result.Value, Is.Not.Null);
			Assert.That(result.Value, Is.Empty);
		}
		
		[Test]
		public async Task Query_Error_Fail()
		{
			await AssertErrorAsync<GetScoresResponse, GameJoltScore[], GameJoltException>(CreateResponse, GetResult);
			return;

			GetScoresResponse CreateResponse()
			{
				return new GetScoresResponse(false, GameJoltException.UNKNOWN_FATAL_ERROR, null);
			}
			
			Task<GameJoltResult<GameJoltScore[]>> GetResult()
			{
				return GameJoltAPI.Scores.QueryScores().ForTable(0).Limit(0).ForCurrentUser().BetterThan(0).WorseThan(0).GetAsync();
			}
		}
		
		[Test]
		public void ScoreDisplayName_Username()
		{
			string? name = DummyData.faker.Internet.UserName();

			GameJoltScore score = new GameJoltScore(0, "", "", name, 0, "", default);

			Assert.That(score.DisplayName, Is.EqualTo(name));
		}

		[Test]
		public void ScoreDisplayName_GuestName()
		{
			string? name = DummyData.faker.Internet.UserName();

			GameJoltScore score = new GameJoltScore(0, "", "", "", null, name, default);

			Assert.That(score.DisplayName, Is.EqualTo(name));
		}

		[Test]
		public void ScoreDisplayName_Empty()
		{
			GameJoltScore score = new GameJoltScore(0, "", "", "", null, "", default);

			Assert.That(score.DisplayName, Is.EqualTo(string.Empty));
		}

		[Test]
		[TestCase("")]
		[TestCase("Extra Data")]
		public async Task SubmitScore_ValidUrl(string extraData)
		{
			await AuthenticateAsync();
			
			await TestUrlAsync(() => GameJoltAPI.Scores.SubmitScoreAsync(0, 0, "0", extraData),
				url =>
				{
					if(string.IsNullOrEmpty(extraData))
					{
						Assert.That(url, Does.StartWith(GameJoltUrlBuilder.BASE_URL + GameJoltScores.ADD_ENDPOINT + $"?username={Username}&user_token={Token}&score=0&sort=0&table_id=0"));
					}
					else
					{
						Assert.That(url, Does.StartWith(GameJoltUrlBuilder.BASE_URL + GameJoltScores.ADD_ENDPOINT + $"?username={Username}&user_token={Token}&score=0&sort=0&extra_data={extraData}&table_id=0"));
					}
				});
		}
		
		[Test]
		[TestCase("")]
		[TestCase("Extra Data")]
		public async Task SubmitScoreAsGuest_ValidUrl(string extraData)
		{
			await AuthenticateAsync();
			
			await TestUrlAsync(() => GameJoltAPI.Scores.SubmitScoreAsGuestAsync(0, "Guest", 0, "0", extraData),
				url =>
				{
					if(string.IsNullOrEmpty(extraData))
					{
						Assert.That(url, Does.StartWith(GameJoltUrlBuilder.BASE_URL + GameJoltScores.ADD_ENDPOINT + $"?guest=Guest&score=0&sort=0&table_id=0"));
					}
					else
					{
						Assert.That(url, Does.StartWith(GameJoltUrlBuilder.BASE_URL + GameJoltScores.ADD_ENDPOINT + $"?guest=Guest&score=0&sort=0&extra_data={extraData}&table_id=0"));
					}
				});
		}
		
		[Test]
		public async Task GetRank_ValidUrl()
		{
			await TestUrlAsync(() => GameJoltAPI.Scores.GetRankAsync(0, 0),
				url =>
				{
					Assert.That(url, Does.StartWith(GameJoltUrlBuilder.BASE_URL + GameJoltScores.GET_RANK_ENDPOINT + "?sort=0&table_id=0"));
				});
		}

		[Test]
		public async Task GetTables_ValidUrl()
		{
			await TestUrlAsync(() => GameJoltAPI.Scores.GetTablesAsync(),
				url =>
				{
					Assert.That(url, Does.StartWith(GameJoltUrlBuilder.BASE_URL + GameJoltScores.GET_TABLES_ENDPOINT));
				});
		}
		
		[Test]
		public async Task GetScores_ValidUrl()
		{
			await AuthenticateAsync();
			
			await TestUrlAsync(() => GameJoltAPI.Scores.QueryScores().ForTable(0).Limit(0).ForCurrentUser().BetterThan(0).WorseThan(0).GetAsync(),
				url =>
				{
					Assert.That(url, Does.StartWith(GameJoltUrlBuilder.BASE_URL + GameJoltScores.ENDPOINT + $"?table_id=0&limit=0&username={Username}&user_token={Token}&better_than=0&worse_than=0"));
				});
		}
		
		[Test]
		public async Task GetScoresGuest_ValidUrl()
		{
			await TestUrlAsync(() => GameJoltAPI.Scores.QueryScores().ForTable(0).Limit(0).ForGuest("test").BetterThan(0).WorseThan(0).GetAsync(),
				url =>
				{
					Assert.That(url, Does.StartWith(GameJoltUrlBuilder.BASE_URL + GameJoltScores.ENDPOINT + "?table_id=0&limit=0&guest=test&better_than=0&worse_than=0"));
				});
		}
	}
}