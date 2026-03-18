#if !DISABLE_GAMEJOLT
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests
{
	internal partial class TrophiesTest
	{
		private static readonly Predicate<ArgumentNullException> trophyResultsPredicate = e => MustNotBeNullPredicate<IList<GameJoltTrophy>>(e, "results");
		private static readonly Predicate<ArgumentNullException> trophyIdsPredicate = e => MustNotBeNullPredicate<IEnumerable<int>>(e, "trophyIds");

		[Test]
		public async Task GetTrophies_WithList_Guards()
		{
			await AssertThrowsAsync(() => GameJoltAPI.Trophies.GetTrophiesAsync(((IList<GameJoltTrophy>) null)!), trophyResultsPredicate);
		}

		[Test]
		public async Task GetTrophies_Achieved_WithList_Guards([Values] bool achieved)
		{
			await AssertThrowsAsync(() => GameJoltAPI.Trophies.GetTrophiesAsync(achieved, null!), trophyResultsPredicate);
		}

		[Test]
		public async Task GetTrophies_Ids_Guards()
		{
			await AssertThrowsAsync(() => GameJoltAPI.Trophies.GetTrophiesAsync(((IEnumerable<int>) null)!), trophyIdsPredicate);
		}

		[Test]
		public async Task GetTrophies_Ids_WithList_Guards()
		{
			// Null ids
			await AssertThrowsAsync(() => GameJoltAPI.Trophies.GetTrophiesAsync(null!, new List<GameJoltTrophy>()), trophyIdsPredicate);

			// Null results
			await AssertThrowsAsync(() => GameJoltAPI.Trophies.GetTrophiesAsync(new[] { 1, 2, 3 }, null!), trophyResultsPredicate);
		}
	}
}
#endif