#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

#nullable enable

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GameJolt.NET.Tests.Attributes;
using Hertzole.GameJolt;
using NSubstitute;
using NUnit.Framework;

namespace GameJolt.NET.Tests
{
	[NeedsAuthentication]
	internal partial class TrophiesTest : BaseTest
	{
		private static readonly int[] trophyIds = { 0, 1 };

		[Test]
		public async Task GetTrophies_Authenticated_ReturnsTrophies()
		{
			TrophyInternal trophy = DummyData.Trophy();

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("trophies/?"))
				{
					return FromResult(serializer.SerializeResponse(new FetchTrophiesResponse(true, null, new[] { trophy })));
				}

				return FromResult("");
			});

			GameJoltResult<GameJoltTrophy[]> result = await GameJoltAPI.Trophies.GetTrophiesAsync();

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Value, Is.Not.Null);
			Assert.That(result.Value, Has.Length.GreaterThan(0));
			Assert.That(result.Value![0].Id, Is.EqualTo(trophy.id));
			Assert.That(result.Value[0].Title, Is.EqualTo(trophy.title));
			Assert.That(result.Value[0].Description, Is.EqualTo(trophy.description));
			Assert.That(result.Value[0].Difficulty, Is.EqualTo(trophy.difficulty));
			Assert.That(result.Value[0].ImageUrl, Is.EqualTo(trophy.imageUrl));
			Assert.That(result.Value[0].HasAchieved, Is.EqualTo(trophy.achieved));
		}

		[Test]
		public async Task GetTrophies_Buffer_Authenticated_ReturnsTrophies()
		{
			// Arrange
			List<GameJoltTrophy> buffer = new List<GameJoltTrophy>(DummyData.Many(100, () => DummyData.Trophy().ToPublicTrophy()));

			TrophyInternal trophy = DummyData.Trophy();
			GameJoltAPI.webClient.GetStringAsync("", CancellationToken.None).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("trophies/?"))
				{
					return FromResult(serializer.SerializeResponse(new FetchTrophiesResponse(true, null, new[] { trophy })));
				}

				return FromResult("");
			});

			// Act
			GameJoltResult result = await GameJoltAPI.Trophies.GetTrophiesAsync(buffer);

			// Assert
			Assert.That(result.HasError, Is.False);
			Assert.That(buffer, Is.Not.Empty);
			Assert.That(buffer, Has.Count.EqualTo(1)); // Also makes sure the buffer was cleared.
			Assert.That(buffer[0].Id, Is.EqualTo(trophy.id));
			Assert.That(buffer[0].Title, Is.EqualTo(trophy.title));
			Assert.That(buffer[0].Description, Is.EqualTo(trophy.description));
			Assert.That(buffer[0].Difficulty, Is.EqualTo(trophy.difficulty));
			Assert.That(buffer[0].ImageUrl, Is.EqualTo(trophy.imageUrl));
			Assert.That(buffer[0].HasAchieved, Is.EqualTo(trophy.achieved));
		}

		[Test]
		[DoNotAuthenticate]
		public async Task GetTrophies_NotAuthenticated_ReturnsError()
		{
			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("trophies/?"))
				{
					return FromResult(serializer.SerializeResponse(new FetchTrophiesResponse(false, "Not authenticated.", Array.Empty<TrophyInternal>())));
				}

				return FromResult("");
			});

			GameJoltResult<GameJoltTrophy[]> result = await GameJoltAPI.Trophies.GetTrophiesAsync();

			Assert.That(result.HasError, Is.True, "Result should have an error.");
			Assert.That(result.Value, Is.Null, "Value should be null when there is an error.");
			Assert.That(result.Exception, Is.Not.Null, "Exception should not be null when there is an error.");
			Assert.That(result.Exception, Is.TypeOf<GameJoltAuthorizedException>(),
				"Exception should be of type GameJoltAuthorizedException when not authenticated.");
		}

		[Test]
		[DoNotAuthenticate]
		public async Task GetTrophies_Buffer_NotAuthenticated_ReturnsError()
		{
			// Arrange
			List<GameJoltTrophy> buffer = new List<GameJoltTrophy>();

			// Act
			await AssertGetTrophiesReturnsError(() => GameJoltAPI.Trophies.GetTrophiesAsync(buffer));

			// Assert
			Assert.That(buffer, Is.Empty);
		}

		[Test]
		public async Task GetTrophies_Authenticated_Achieved_ReturnsTrophies()
		{
			TrophyInternal trophy = DummyData.Trophy();

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("trophies/?"))
				{
					return FromResult(serializer.SerializeResponse(new FetchTrophiesResponse(true, null, new[] { trophy })));
				}

				return FromResult("");
			});

			GameJoltResult<GameJoltTrophy[]> result = await GameJoltAPI.Trophies.GetTrophiesAsync(true);

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Value, Is.Not.Null);
			Assert.That(result.Value, Has.Length.GreaterThan(0));
			Assert.That(result.Value![0].Id, Is.EqualTo(trophy.id));
			Assert.That(result.Value[0].Title, Is.EqualTo(trophy.title));
			Assert.That(result.Value[0].Description, Is.EqualTo(trophy.description));
			Assert.That(result.Value[0].Difficulty, Is.EqualTo(trophy.difficulty));
			Assert.That(result.Value[0].ImageUrl, Is.EqualTo(trophy.imageUrl));
			Assert.That(result.Value[0].HasAchieved, Is.EqualTo(trophy.achieved));
		}

		[Test]
		public async Task GetTrophies_Buffer_Authenticated_Achieved_ReturnsTrophies([Values] bool achieved)
		{
			// Arrange
			List<GameJoltTrophy> buffer = new List<GameJoltTrophy>(DummyData.Many(100, () => DummyData.Trophy().ToPublicTrophy()));

			TrophyInternal trophy = DummyData.Trophy();
			GameJoltAPI.webClient.GetStringAsync("", CancellationToken.None).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("trophies/?"))
				{
					return FromResult(serializer.SerializeResponse(new FetchTrophiesResponse(true, null, new[] { trophy })));
				}

				return FromResult("");
			});

			// Act
			GameJoltResult result = await GameJoltAPI.Trophies.GetTrophiesAsync(achieved, buffer);

			// Assert
			Assert.That(result.HasError, Is.False);
			Assert.That(buffer, Is.Not.Empty);
			Assert.That(buffer, Has.Count.EqualTo(1)); // Also makes sure the buffer was cleared.
			Assert.That(buffer[0].Id, Is.EqualTo(trophy.id));
			Assert.That(buffer[0].Title, Is.EqualTo(trophy.title));
			Assert.That(buffer[0].Description, Is.EqualTo(trophy.description));
			Assert.That(buffer[0].Difficulty, Is.EqualTo(trophy.difficulty));
			Assert.That(buffer[0].ImageUrl, Is.EqualTo(trophy.imageUrl));
			Assert.That(buffer[0].HasAchieved, Is.EqualTo(trophy.achieved));
		}

		[Test]
		public async Task GetTrophies_Authenticated_NotAchieved_ReturnsTrophies()
		{
			TrophyInternal trophy = DummyData.Trophy();

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("trophies/?"))
				{
					return FromResult(serializer.SerializeResponse(new FetchTrophiesResponse(true, null, new[] { trophy })));
				}

				return FromResult("");
			});

			GameJoltResult<GameJoltTrophy[]> result = await GameJoltAPI.Trophies.GetTrophiesAsync(false);

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Value, Is.Not.Null);
			Assert.That(result.Value, Has.Length.GreaterThan(0));
			Assert.That(result.Value![0].Id, Is.EqualTo(trophy.id));
			Assert.That(result.Value[0].Title, Is.EqualTo(trophy.title));
			Assert.That(result.Value[0].Description, Is.EqualTo(trophy.description));
			Assert.That(result.Value[0].Difficulty, Is.EqualTo(trophy.difficulty));
			Assert.That(result.Value[0].ImageUrl, Is.EqualTo(trophy.imageUrl));
			Assert.That(result.Value[0].HasAchieved, Is.EqualTo(trophy.achieved));
		}

		[Test]
		[DoNotAuthenticate]
		public async Task GetTrophies_Buffer_NotAuthenticated_Achieved_ReturnsError([Values] bool achieved)
		{
			// Arrange
			List<GameJoltTrophy> buffer = new List<GameJoltTrophy>();

			// Act
			await AssertGetTrophiesReturnsError(() => GameJoltAPI.Trophies.GetTrophiesAsync(achieved, buffer));

			// Assert
			Assert.That(buffer, Is.Empty);
		}

		[Test]
		public async Task GetTrophies_Authenticated_Ids_ReturnsTrophies()
		{
			TrophyInternal trophy1 = DummyData.Trophy();
			TrophyInternal trophy2 = DummyData.Trophy();

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("trophies/?"))
				{
					return FromResult(serializer.SerializeResponse(new FetchTrophiesResponse(true, null, new[] { trophy1, trophy2 })));
				}

				return FromResult("");
			});

			GameJoltResult<GameJoltTrophy[]> result = await GameJoltAPI.Trophies.GetTrophiesAsync(new[] { trophy1.id, trophy2.id });

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Value, Is.Not.Null);
			Assert.That(result.Value, Has.Length.GreaterThan(0));
			Assert.That(result.Value![0].Id, Is.EqualTo(trophy1.id));
			Assert.That(result.Value[0].Title, Is.EqualTo(trophy1.title));
			Assert.That(result.Value[0].Description, Is.EqualTo(trophy1.description));
			Assert.That(result.Value[0].Difficulty, Is.EqualTo(trophy1.difficulty));
			Assert.That(result.Value[0].ImageUrl, Is.EqualTo(trophy1.imageUrl));
			Assert.That(result.Value[0].HasAchieved, Is.EqualTo(trophy1.achieved));

			Assert.That(result.Value[1].Id, Is.EqualTo(trophy2.id));
			Assert.That(result.Value[1].Title, Is.EqualTo(trophy2.title));
			Assert.That(result.Value[1].Description, Is.EqualTo(trophy2.description));
			Assert.That(result.Value[1].Difficulty, Is.EqualTo(trophy2.difficulty));
			Assert.That(result.Value[1].ImageUrl, Is.EqualTo(trophy2.imageUrl));
			Assert.That(result.Value[1].HasAchieved, Is.EqualTo(trophy2.achieved));
		}

		[Test]
		public async Task GetTrophies_Authenticated_Id_ReturnsTrophies()
		{
			TrophyInternal trophy = DummyData.Trophy();

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("trophies/?"))
				{
					return FromResult(serializer.SerializeResponse(new FetchTrophiesResponse(true, null, new[] { trophy })));
				}

				return FromResult("");
			});

			GameJoltResult<GameJoltTrophy> result = await GameJoltAPI.Trophies.GetTrophyAsync(0);

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Value.Id, Is.EqualTo(trophy.id));
			Assert.That(result.Value.Title, Is.EqualTo(trophy.title));
			Assert.That(result.Value.Description, Is.EqualTo(trophy.description));
			Assert.That(result.Value.Difficulty, Is.EqualTo(trophy.difficulty));
			Assert.That(result.Value.ImageUrl, Is.EqualTo(trophy.imageUrl));
			Assert.That(result.Value.HasAchieved, Is.EqualTo(trophy.achieved));
		}

		[Test]
		[DoNotAuthenticate]
		public async Task GetTrophies_NotAuthenticated_Id_ReturnsError()
		{
			// Act
			await AssertGetTrophiesReturnsError(() => GameJoltAPI.Trophies.GetTrophiesAsync(trophyIds));
		}

		[Test]
		[DoNotAuthenticate]
		public async Task GetTrophies_Buffer_NotAuthenticated_Id_ReturnsError()
		{
			// Arrange
			List<GameJoltTrophy> buffer = new List<GameJoltTrophy>();

			// Act
			await AssertGetTrophiesReturnsError(() => GameJoltAPI.Trophies.GetTrophiesAsync(trophyIds, buffer));

			// Assert
			Assert.That(buffer, Is.Empty);
		}

		[Test]
		public async Task GetTrophies_Error_Fail()
		{
			await AssertErrorAsync<FetchTrophiesResponse, GameJoltTrophy[], GameJoltInvalidTrophyException>(CreateResponse, GetResult,
				GameJoltInvalidTrophyException.DOES_NOT_BELONG_MESSAGE);

			return;

			FetchTrophiesResponse CreateResponse()
			{
				return new FetchTrophiesResponse(false, GameJoltInvalidTrophyException.DOES_NOT_BELONG_MESSAGE, null);
			}

			Task<GameJoltResult<GameJoltTrophy[]>> GetResult()
			{
				return GameJoltAPI.Trophies.GetTrophiesAsync();
			}
		}

		[Test]
		public async Task GetTrophies_Achieved_Error_Fail([Values] bool achieved)
		{
			await AssertErrorAsync<FetchTrophiesResponse, GameJoltTrophy[], GameJoltInvalidTrophyException>(CreateResponse, GetResult,
				GameJoltInvalidTrophyException.DOES_NOT_BELONG_MESSAGE);

			FetchTrophiesResponse CreateResponse()
			{
				return new FetchTrophiesResponse(false, GameJoltInvalidTrophyException.DOES_NOT_BELONG_MESSAGE, null);
			}

			Task<GameJoltResult<GameJoltTrophy[]>> GetResult()
			{
				return GameJoltAPI.Trophies.GetTrophiesAsync(achieved);
			}
		}

		[Test]
		public async Task GetTrophies_Ids_Error_Fail()
		{
			await AssertErrorAsync<FetchTrophiesResponse, GameJoltTrophy[], GameJoltInvalidTrophyException>(CreateResponse, GetResult,
				GameJoltInvalidTrophyException.DOES_NOT_BELONG_MESSAGE);

			FetchTrophiesResponse CreateResponse()
			{
				return new FetchTrophiesResponse(false, GameJoltInvalidTrophyException.DOES_NOT_BELONG_MESSAGE, null);
			}

			Task<GameJoltResult<GameJoltTrophy[]>> GetResult()
			{
				return GameJoltAPI.Trophies.GetTrophiesAsync(trophyIds);
			}
		}

		// This method is mainly just to allow the foreach loop end by itself instead of having to manually break it.
		[Test]
		public async Task GetTrophies_TooManyIds_Success()
		{
			TrophyInternal[] trophies =
			{
				DummyData.Trophy(),
				DummyData.Trophy(),
				DummyData.Trophy()
			};

			List<GameJoltTrophy> results = new List<GameJoltTrophy>();

			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("trophies/?"))
				{
					return FromResult(serializer.SerializeResponse(new FetchTrophiesResponse(true, null, trophies)));
				}

				return FromResult("");
			});

			int[] ids =
			{
				trophies[0].id,
				trophies[1].id,
				trophies[2].id
			};

			GameJoltResult result = await GameJoltAPI.Trophies.GetTrophiesInternalAsync(ids, 5, true, results, default);

			Assert.That(result.HasError, Is.False);
			Assert.That(results, Has.Count.EqualTo(3));
			Assert.That(results![0].Id, Is.EqualTo(trophies[0].id));
			Assert.That(results[1].Id, Is.EqualTo(trophies[1].id));
			Assert.That(results[2].Id, Is.EqualTo(trophies[2].id));
		}

		[Test]
		public async Task GetTrophy_Error_Fail()
		{
			await AssertErrorAsync<FetchTrophiesResponse, GameJoltTrophy, GameJoltInvalidTrophyException>(CreateResponse, GetResult,
				GameJoltInvalidTrophyException.DOES_NOT_BELONG_MESSAGE);

			return;

			FetchTrophiesResponse CreateResponse()
			{
				return new FetchTrophiesResponse(false, GameJoltInvalidTrophyException.DOES_NOT_BELONG_MESSAGE, null);
			}

			Task<GameJoltResult<GameJoltTrophy>> GetResult()
			{
				return GameJoltAPI.Trophies.GetTrophyAsync(0);
			}
		}

		[Test]
		public async Task GetTrophies_NoTrophies_Success()
		{
			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("trophies/?"))
				{
					return FromResult(serializer.SerializeResponse(new FetchTrophiesResponse(true, null, Array.Empty<TrophyInternal>())));
				}

				return FromResult("");
			});

			GameJoltResult<GameJoltTrophy[]> result = await GameJoltAPI.Trophies.GetTrophiesAsync();

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Value, Is.Not.Null);
			Assert.That(result.Value, Is.Empty);
		}

		[Test]
		public async Task UnlockTrophy_Authenticated_ReturnsSuccess()
		{
			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains(GameJoltTrophies.ADD_ENDPOINT))
				{
					return FromResult(serializer.SerializeResponse(new Response(true, null)));
				}

				return FromResult("");
			});

			GameJoltResult result = await GameJoltAPI.Trophies.UnlockTrophyAsync(0);

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
		}

		[Test]
		[DoNotAuthenticate]
		public async Task UnlockTrophy_NotAuthenticated_ReturnsError()
		{
			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains(GameJoltTrophies.ADD_ENDPOINT))
				{
					return FromResult(serializer.SerializeResponse(new Response(false, "Not authenticated.")));
				}

				return FromResult("");
			});

			GameJoltResult result = await GameJoltAPI.Trophies.UnlockTrophyAsync(0);

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception is GameJoltAuthorizedException);
		}

		[Test]
		public async Task UnlockTrophy_Authenticated_InvalidTrophy_ReturnsError()
		{
			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains(GameJoltTrophies.ADD_ENDPOINT))
				{
					return FromResult(serializer.SerializeResponse(new Response(false, GameJoltInvalidTrophyException.DOES_NOT_BELONG_MESSAGE)));
				}

				return FromResult("");
			});

			GameJoltResult result = await GameJoltAPI.Trophies.UnlockTrophyAsync(0);

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception is GameJoltInvalidTrophyException);
		}

		[Test]
		public async Task UnlockTrophy_AlreadyUnlocked_Error()
		{
			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains(GameJoltTrophies.ADD_ENDPOINT))
				{
					return FromResult(serializer.SerializeResponse(new Response(false, GameJoltTrophyException.ALREADY_UNLOCKED_MESSAGE)));
				}

				return FromResult("");
			});

			GameJoltResult result = await GameJoltAPI.Trophies.UnlockTrophyAsync(0, true);

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception is GameJoltTrophyException);
		}

		[Test]
		public async Task UnlockTrophy_AlreadyUnlocked_NoError()
		{
			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains(GameJoltTrophies.ADD_ENDPOINT))
				{
					return FromResult(serializer.SerializeResponse(new Response(false, GameJoltTrophyException.ALREADY_UNLOCKED_MESSAGE)));
				}

				return FromResult("");
			});

			GameJoltResult result = await GameJoltAPI.Trophies.UnlockTrophyAsync(0);

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
		}

		[Test]
		public async Task RemoveTrophy_Authenticated_ReturnsSuccess()
		{
			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains(GameJoltTrophies.REMOVE_ENDPOINT))
				{
					return FromResult(serializer.SerializeResponse(new Response(true, null)));
				}

				return FromResult("");
			});

			GameJoltResult result = await GameJoltAPI.Trophies.RemoveUnlockedTrophyAsync(0);

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
		}

		[Test]
		[DoNotAuthenticate]
		public async Task RemoveTrophy_NotAuthenticated_ReturnsError()
		{
			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains(GameJoltTrophies.REMOVE_ENDPOINT))
				{
					return FromResult(serializer.SerializeResponse(new Response(false, "Not authenticated.")));
				}

				return FromResult("");
			});

			GameJoltResult result = await GameJoltAPI.Trophies.RemoveUnlockedTrophyAsync(0);

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception is GameJoltAuthorizedException);
		}

		[Test]
		public async Task RemoveTrophy_Authenticated_InvalidTrophy_ReturnsError()
		{
			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains(GameJoltTrophies.REMOVE_ENDPOINT))
				{
					return FromResult(serializer.SerializeResponse(new Response(false, GameJoltInvalidTrophyException.DOES_NOT_BELONG_MESSAGE)));
				}

				return FromResult("");
			});

			GameJoltResult result = await GameJoltAPI.Trophies.RemoveUnlockedTrophyAsync(0);

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception is GameJoltInvalidTrophyException);
		}

		[Test]
		public async Task RemoveTrophy_NotUnlocked_ReturnsError()
		{
			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains(GameJoltTrophies.REMOVE_ENDPOINT))
				{
					return FromResult(serializer.SerializeResponse(new Response(false, GameJoltTrophyException.DOES_NOT_HAVE_MESSAGE)));
				}

				return FromResult("");
			});

			GameJoltResult result = await GameJoltAPI.Trophies.RemoveUnlockedTrophyAsync(0);

			Assert.That(result.HasError, Is.True);
			Assert.That(result.Exception, Is.Not.Null);
			Assert.That(result.Exception is GameJoltTrophyException);
		}

		[Test]
		public async Task RemoveTrophy_NotUnlocked_NoError()
		{
			GameJoltAPI.webClient.GetStringAsync("", default).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains(GameJoltTrophies.REMOVE_ENDPOINT))
				{
					return FromResult(serializer.SerializeResponse(new Response(false, GameJoltTrophyException.DOES_NOT_HAVE_MESSAGE)));
				}

				return FromResult("");
			});

			GameJoltResult result = await GameJoltAPI.Trophies.RemoveUnlockedTrophyAsync(0, false);

			Assert.That(result.HasError, Is.False);
			Assert.That(result.Exception, Is.Null);
		}

		[Test]
		public async Task GetTrophies_ValidUrl()
		{
			await TestUrlAsync(() => GameJoltAPI.Trophies.GetTrophiesAsync(),
				url =>
				{
					Assert.That(url, Does.StartWith(GameJoltUrlBuilder.BASE_URL + GameJoltTrophies.ENDPOINT + $"?username={Username}&user_token={Token}"));
				});
		}

		[Test]
		public async Task GetTrophies_Achieved_ValidUrl([Values] bool achieved)
		{
			await TestUrlAsync(() => GameJoltAPI.Trophies.GetTrophiesAsync(achieved),
				url =>
				{
					Assert.That(url,
						Does.StartWith(GameJoltUrlBuilder.BASE_URL + GameJoltTrophies.ENDPOINT +
						               $"?username={Username}&user_token={Token}&achieved={(achieved ? "true" : "false")}"));
				});
		}

		[Test]
		public async Task GetTrophies_Ids_ValidUrl()
		{
			await TestUrlAsync(() => GameJoltAPI.Trophies.GetTrophiesAsync(trophyIds),
				url =>
				{
					Assert.That(url,
						Does.StartWith(GameJoltUrlBuilder.BASE_URL + GameJoltTrophies.ENDPOINT + $"?username={Username}&user_token={Token}&trophy_id=0,1"));
				});
		}

		[Test]
		public async Task GetTrophy_ValidUrl()
		{
			int id = DummyData.randomizer.Int();

			await TestUrlAsync(() => GameJoltAPI.Trophies.GetTrophyAsync(id),
				url =>
				{
					Assert.That(url,
						Does.StartWith(GameJoltUrlBuilder.BASE_URL + GameJoltTrophies.ENDPOINT + $"?username={Username}&user_token={Token}&trophy_id={id}"));
				});
		}

		[Test]
		public async Task UnlockTrophy_ValidUrl()
		{
			int id = DummyData.randomizer.Int();

			await TestUrlAsync(() => GameJoltAPI.Trophies.UnlockTrophyAsync(id),
				url =>
				{
					Assert.That(url,
						Does.StartWith(GameJoltUrlBuilder.BASE_URL + GameJoltTrophies.ADD_ENDPOINT +
						               $"?username={Username}&user_token={Token}&trophy_id={id}"));
				});
		}

		[Test]
		public async Task RemoveUnlockedTrophy_ValidUrl()
		{
			int id = DummyData.randomizer.Int();

			await TestUrlAsync(() => GameJoltAPI.Trophies.RemoveUnlockedTrophyAsync(id),
				url =>
				{
					Assert.That(url,
						Does.StartWith(GameJoltUrlBuilder.BASE_URL + GameJoltTrophies.REMOVE_ENDPOINT +
						               $"?username={Username}&user_token={Token}&trophy_id={id}"));
				});
		}

		private static async Task AssertGetTrophiesReturnsError<TResult>(Func<Task<TResult>> callFunc) where TResult : IGameJoltResult
		{
			// Arrange
			GameJoltAPI.webClient.GetStringAsync("", CancellationToken.None).ReturnsForAnyArgs(info =>
			{
				string? arg = info.Arg<string>();

				if (arg.Contains("trophies/?"))
				{
					return FromResult(serializer.SerializeResponse(new FetchTrophiesResponse(false, "Not authenticated.", Array.Empty<TrophyInternal>())));
				}

				return FromResult("");
			});

			// Act
			TResult result = await callFunc();

			// Assert
			Assert.That(result.HasError, Is.True, "Result should have an error.");
			Assert.That(result.Exception, Is.Not.Null, "Exception should not be null when there is an error.");
			Assert.That(result.Exception, Is.TypeOf<GameJoltAuthorizedException>(),
				"Exception should be of type GameJoltAuthorizedException when not authenticated.");
		}
	}
}
#endif // DISABLE_GAMEJOLT