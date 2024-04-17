using System;
using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests.Equality
{
	public sealed class Trophies : EqualityTest
	{
		[Test]
		public void FetchTrophiesResponse()
		{
			TestEquality((a, b) => a == b, (a, b) => a != b,
				new FetchTrophiesResponse(true, "message", Array.Empty<TrophyInternal>()),
				new FetchTrophiesResponse(false, "message", Array.Empty<TrophyInternal>()),
				new FetchTrophiesResponse(true, "message2", Array.Empty<TrophyInternal>()),
				new FetchTrophiesResponse(true, "message", new[] { DummyData.Trophy() }));
		}

		[Test]
		public void TrophyInternal()
		{
			TestEquality((a, b) => a == b, (a, b) => a != b,
				new TrophyInternal(0, "title", "description", TrophyDifficulty.Bronze, "image", true),
				new TrophyInternal(1, "title", "description", TrophyDifficulty.Bronze, "image", true),
				new TrophyInternal(0, "title2", "description", TrophyDifficulty.Bronze, "image", true),
				new TrophyInternal(0, "title", "description2", TrophyDifficulty.Bronze, "image", true),
				new TrophyInternal(0, "title", "description", TrophyDifficulty.Silver, "image", true),
				new TrophyInternal(0, "title", "description", TrophyDifficulty.Bronze, "image2", true),
				new TrophyInternal(0, "title", "description", TrophyDifficulty.Bronze, "image", false));
		}

		[Test]
		public void GameJoltTrophy()
		{
			TestEquality((a, b) => a == b, (a, b) => a != b,
				new GameJoltTrophy(0, "title", "description", TrophyDifficulty.Bronze, "image", true),
				new GameJoltTrophy(1, "title", "description", TrophyDifficulty.Bronze, "image", true),
				new GameJoltTrophy(0, "title2", "description", TrophyDifficulty.Bronze, "image", true),
				new GameJoltTrophy(0, "title", "description2", TrophyDifficulty.Bronze, "image", true),
				new GameJoltTrophy(0, "title", "description", TrophyDifficulty.Silver, "image", true),
				new GameJoltTrophy(0, "title", "description", TrophyDifficulty.Bronze, "image2", true),
				new GameJoltTrophy(0, "title", "description", TrophyDifficulty.Bronze, "image", false));
		}
	}
}