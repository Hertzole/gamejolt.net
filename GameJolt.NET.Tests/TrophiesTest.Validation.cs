#if !DISABLE_GAMEJOLT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests
{
	internal partial class TrophiesTest
	{
		private static readonly Predicate<ArgumentNullException> trophyResultsPredicate = e => MustNotBeNullPredicate<IList<GameJoltTrophy>>(e, "results");

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
			// Null list
			await AssertThrowsAsync<ArgumentNullException>(() => GameJoltAPI.Trophies.GetTrophiesAsync(((List<int>) null)!),
				e => MustNotBeNullPredicate<IList<int>>(e, "trophyIds"));

			// Null enumerable
			await AssertThrowsAsync<ArgumentNullException>(() => GameJoltAPI.Trophies.GetTrophiesAsync(((IEnumerable<int>) null)!),
				e => MustNotBeNullPredicate<IEnumerable<int>>(e, "trophyIds"));
		}

		[Test]
		public async Task GetTrophies_Ids_WithList_Guards()
		{
			// Null ids (list)
			await AssertThrowsAsync<ArgumentNullException>(() => GameJoltAPI.Trophies.GetTrophiesAsync(((List<int>) null)!, new List<GameJoltTrophy>()),
				e => MustNotBeNullPredicate<IList<int>>(e, "trophyIds"));

			// Null ids (enumerable)
			await AssertThrowsAsync<ArgumentNullException>(() => GameJoltAPI.Trophies.GetTrophiesAsync(((IEnumerable<int>) null)!, new List<GameJoltTrophy>()),
				e => MustNotBeNullPredicate<IEnumerable<int>>(e, "trophyIds"));

			// Null results (list)
			await AssertThrowsAsync(() => GameJoltAPI.Trophies.GetTrophiesAsync(new List<int>(), null!), trophyResultsPredicate);

			// Null results (enumerable)
			await AssertThrowsAsync(() => GameJoltAPI.Trophies.GetTrophiesAsync(new List<int>().AsEnumerable(), null!), trophyResultsPredicate);
		}
	}
}
#endif