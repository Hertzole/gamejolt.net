#if !DISABLE_GAMEJOLT
#nullable enable

using System;
using System.Threading.Tasks;
using GameJolt.NET.Tests.Enums;
using GameJolt.NET.Tests.Extensions;
using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests
{
	partial class ScoresTest
	{
		[Test]
		public async Task SubmitScore_Uint_MissingScore_ThrowsException([Values] StringInitializationNoNormal init)
		{
			// Arrange
			string? score = init.GetData();

			// Act & Assert
			switch (init)
			{
				case StringInitializationNoNormal.Empty:
					await AssertThrowsAsync<ArgumentException>(() => GameJoltAPI.Scores.SubmitScoreAsync(0, (uint) 0, score!));
					break;
				case StringInitializationNoNormal.Null:
					await AssertThrowsAsync<ArgumentNullException>(() => GameJoltAPI.Scores.SubmitScoreAsync(0, (uint) 0, score!));
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(init), init, null);
			}
		}

		[Test]
		public async Task SubmitScore_Int_MissingScore_ThrowsException([Values] StringInitializationNoNormal init)
		{
			// Arrange
			string? score = init.GetData();

			// Act & Assert
			switch (init)
			{
				case StringInitializationNoNormal.Empty:
					await AssertThrowsAsync<ArgumentException>(() => GameJoltAPI.Scores.SubmitScoreAsync(0, 0, score!));
					break;
				case StringInitializationNoNormal.Null:
					await AssertThrowsAsync<ArgumentNullException>(() => GameJoltAPI.Scores.SubmitScoreAsync(0, 0, score!));
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(init), init, null);
			}
		}

		[Test]
		public async Task SubmitScoreAsGuest_Uint_MissingGuestName_ThrowsException([Values] StringInitializationNoNormal init)
		{
			// Arrange
			string? guestName = init.GetData();

			// Act & Assert
			switch (init)
			{
				case StringInitializationNoNormal.Empty:
					await AssertThrowsAsync<ArgumentException>(() => GameJoltAPI.Scores.SubmitScoreAsGuestAsync(0, guestName!, (uint) 0, "score!"));
					break;
				case StringInitializationNoNormal.Null:
					await AssertThrowsAsync<ArgumentNullException>(() => GameJoltAPI.Scores.SubmitScoreAsGuestAsync(0, guestName!, (uint) 0, "score!"));
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(init), init, null);
			}
		}

		[Test]
		public async Task SubmitScoreAsGuest_Int_MissingGuestName_ThrowsException([Values] StringInitializationNoNormal init)
		{
			// Arrange
			string? guestName = init.GetData();

			// Act & Assert
			switch (init)
			{
				case StringInitializationNoNormal.Empty:
					await AssertThrowsAsync<ArgumentException>(() => GameJoltAPI.Scores.SubmitScoreAsGuestAsync(0, guestName!, 0, "score!"));
					break;
				case StringInitializationNoNormal.Null:
					await AssertThrowsAsync<ArgumentNullException>(() => GameJoltAPI.Scores.SubmitScoreAsGuestAsync(0, guestName!, 0, "score!"));
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(init), init, null);
			}
		}

		[Test]
		public async Task SubmitScoreAsGuest_Uint_MissingScore_ThrowsException([Values] StringInitializationNoNormal init)
		{
			// Arrange
			string? score = init.GetData();

			// Act & Assert
			switch (init)
			{
				case StringInitializationNoNormal.Empty:
					await AssertThrowsAsync<ArgumentException>(() => GameJoltAPI.Scores.SubmitScoreAsGuestAsync(0, "guest", (uint) 0, score!));
					break;
				case StringInitializationNoNormal.Null:
					await AssertThrowsAsync<ArgumentNullException>(() => GameJoltAPI.Scores.SubmitScoreAsGuestAsync(0, "guest", (uint) 0, score!));
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(init), init, null);
			}
		}

		[Test]
		public async Task SubmitScoreAsGuest_Int_MissingScore_ThrowsException([Values] StringInitializationNoNormal init)
		{
			// Arrange
			string? score = init.GetData();

			// Act & Assert
			switch (init)
			{
				case StringInitializationNoNormal.Empty:
					await AssertThrowsAsync<ArgumentException>(() => GameJoltAPI.Scores.SubmitScoreAsGuestAsync(0, "guest", 0, score!));
					break;
				case StringInitializationNoNormal.Null:
					await AssertThrowsAsync<ArgumentNullException>(() => GameJoltAPI.Scores.SubmitScoreAsGuestAsync(0, "guest", 0, score!));
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(init), init, null);
			}
		}

		[Test]
		public async Task GetTables_NullBuffer_ThrowsException()
		{
			await AssertThrowsAsync<ArgumentNullException>(() => GameJoltAPI.Scores.GetTablesAsync(null!));
		}

		[Test]
		public async Task GetScores_NullBuffer_ThrowsException()
		{
			await AssertThrowsAsync<ArgumentNullException>(() => GameJoltAPI.Scores.QueryScores().GetAsync(null!));
		}
	}
}
#endif