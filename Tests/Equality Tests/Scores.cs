using System;
using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests.Equality
{
	public sealed class Scores : EqualityTest
	{
		[Test]
		public void GetScoreRankResponse()
		{
			TestEquality((a, b) => a == b, (a, b) => a != b,
				new GetScoreRankResponse(true, "message", 1),
				new GetScoreRankResponse(false, "message", 1),
				new GetScoreRankResponse(true, "message2", 1),
				new GetScoreRankResponse(true, "message", 2));
		}

		[Test]
		public void GetScoresResponse()
		{
			ScoreInternal score1 = DummyData.Score();
			ScoreInternal score2 = DummyData.Score();

			TestEquality((a, b) => a == b, (a, b) => a != b,
				new GetScoresResponse(true, "message", new[] { score1, score2 }),
				new GetScoresResponse(true, "message", new[] { score1 }),
				new GetScoresResponse(false, "message", new[] { score1, score2 }),
				new GetScoresResponse(true, "message2", new[] { score1, score2 }),
				new GetScoresResponse(true, "message", new[] { DummyData.Score(), DummyData.Score() }));
		}

		[Test]
		public void GetTablesResponse()
		{
			TableInternal table1 = DummyData.Table();
			TableInternal table2 = DummyData.Table();

			TestEquality((a, b) => a == b, (a, b) => a != b,
				new GetTablesResponse(true, "message", new[] { table1, table2 }),
				new GetTablesResponse(true, "message", new[] { table1 }),
				new GetTablesResponse(false, "message", new[] { table1, table2 }),
				new GetTablesResponse(true, "message2", new[] { table1, table2 }),
				new GetTablesResponse(true, "message", new[] { DummyData.Table(), DummyData.Table() }));
		}

		[Test]
		public void ScoreInternal()
		{
			TestEquality((a, b) => a == b, (a, b) => a != b,
				new ScoreInternal(0, "score", "extra", "username", 1, "guest", "stored", 2),
				new ScoreInternal(1, "score", "extra", "username", 1, "guest", "stored", 2),
				new ScoreInternal(0, "score2", "extra", "username", 1, "guest", "stored", 2),
				new ScoreInternal(0, "score", "extra2", "username", 1, "guest", "stored", 2),
				new ScoreInternal(0, "score", "extra", "username2", 1, "guest", "stored", 2),
				new ScoreInternal(0, "score", "extra", "username", 2, "guest", "stored", 2),
				new ScoreInternal(0, "score", "extra", "username", 1, "guest2", "stored", 2),
				new ScoreInternal(0, "score", "extra", "username", 1, "guest", "stored2", 2),
				new ScoreInternal(0, "score", "extra", "username", 1, "guest", "stored", 3));
		}

		[Test]
		public void SubmitScoreResponse()
		{
			TestEquality((a, b) => a == b, (a, b) => a != b,
				new SubmitScoreResponse(true, "message"),
				new SubmitScoreResponse(false, "message"),
				new SubmitScoreResponse(true, "message2"));
		}

		[Test]
		public void TableInternal()
		{
			TestEquality((a, b) => a == b, (a, b) => a != b,
				new TableInternal(0, "name", "description", false),
				new TableInternal(1, "name", "description", false),
				new TableInternal(0, "name2", "description", false),
				new TableInternal(0, "name", "description2", false),
				new TableInternal(0, "name", "description", true));
		}

		[Test]
		public void GameJoltScore()
		{
			DateTime storeTime = new DateTime(2024, 4, 7, 19, 16, 0);

			TestEquality((a, b) => a == b, (a, b) => a != b,
				new GameJoltScore(0, "score", "extraData", "username", 1, "guestName", storeTime),
				new GameJoltScore(1, "score", "extraData", "username", 1, "guestName", storeTime),
				new GameJoltScore(0, "score2", "extraData", "username", 1, "guestName", storeTime),
				new GameJoltScore(0, "score", "extraData2", "username", 1, "guestName", storeTime),
				new GameJoltScore(0, "score", null, "username", 1, "guestName", storeTime),
				new GameJoltScore(0, "score", "extraData", "username2", 1, "guestName", storeTime),
				new GameJoltScore(0, "score", "extraData", null, 1, "guestName", storeTime),
				new GameJoltScore(0, "score", "extraData", "username", 2, "guestName", storeTime),
				new GameJoltScore(0, "score", "extraData", "username", 1, "guestName2", storeTime),
				new GameJoltScore(0, "score", "extraData", "username", 1, null, storeTime),
				new GameJoltScore(0, "score", "extraData", "username", 1, "guestName", storeTime.AddSeconds(1)));
		}

		[Test]
		public void GameJoltTable()
		{
			TestEquality((a, b) => a == b, (a, b) => a != b,
				new GameJoltTable(0, "name", "description", false),
				new GameJoltTable(1, "name", "description", false),
				new GameJoltTable(0, "name2", "description", false),
				new GameJoltTable(0, "name", "description2", false),
				new GameJoltTable(0, "name", "description", true));
		}

		[Test]
		public void GetScoresQuery()
		{
			TestEquality((a, b) => a == b, (a, b) => a != b,
				new GetScoresQuery(null, 0, 1, "username", "token", "guest", 3, 4),
				new GetScoresQuery(null, 1, 1, "username", "token", "guest", 3, 4),
				new GetScoresQuery(null, 0, 2, "username", "token", "guest", 3, 4),
				new GetScoresQuery(null, 0, 1, "username2", "token", "guest", 3, 4),
				new GetScoresQuery(null, 0, 1, null, "token", "guest", 3, 4),
				new GetScoresQuery(null, 0, 1, "username", "token2", "guest", 3, 4),
				new GetScoresQuery(null, 0, 1, "username", null, "guest", 3, 4),
				new GetScoresQuery(null, 0, 1, "username", "token", "guest2", 3, 4),
				new GetScoresQuery(null, 0, 1, "username", "token", null, 3, 4),
				new GetScoresQuery(null, 0, 1, "username", "token", "guest", 4, 4),
				new GetScoresQuery(null, 0, 1, "username", "token", "guest", 3, 5));
		}
	}
}