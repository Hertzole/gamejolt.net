#nullable enable

using System;
using System.Text;
using GameJolt.NET.Tests.Enums;
using GameJolt.NET.Tests.Extensions;
using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests.ToString
{
	public sealed class Scores : BaseToStringTest
	{
		[Test]
		public void GetScoreRankResponse()
		{
			bool success = faker.Random.Bool();
			string message = faker.Lorem.Sentence();
			int rank = faker.Random.Int();

			GetScoreRankResponse response = new GetScoreRankResponse(success, message, rank);

			Assert.That(response.ToString(), Is.EqualTo($"{nameof(GetScoreRankResponse)} (Success: {success}, Message: {message}, rank: {rank})"));
		}

		[Test]
		public void GetScoresResponse([Values] ArrayInitialization arrayInitialization)
		{
			ScoreInternal[]? scores = CreateArray(arrayInitialization, f => new ScoreInternal(f.Random.Int(), f.Random.Utf16String(), f.Lorem.Sentence(),
				f.Internet.UserName(),
				f.Random.Int(), f.Internet.UserName(), f.Random.Utf16String(), f.Random.Long()));

			bool success = faker.Random.Bool();
			string message = faker.Lorem.Sentence();

			GetScoresResponse response = new GetScoresResponse(success, message, scores);

			Assert.That(response.ToString(),
				Is.EqualTo($"{nameof(GetScoresResponse)} (Success: {success}, Message: {message}, scores: {scores.GetExpectedString()})"));
		}

		[Test]
		public void GetTablesResponse([Values] ArrayInitialization arrayInitialization)
		{
			TableInternal[]? tables = CreateArray(arrayInitialization,
				f => new TableInternal(f.Random.Int(), f.Internet.UserName(), f.Lorem.Sentence(), f.Random.Bool()));

			bool success = faker.Random.Bool();
			string message = faker.Lorem.Sentence();

			GetTablesResponse response = new GetTablesResponse(success, message, tables);

			Assert.That(response.ToString(),
				Is.EqualTo($"{nameof(GetTablesResponse)} (Success: {success}, Message: {message}, tables: {tables.GetExpectedString()})"));
		}

		[Test]
		public void ScoreInternal()
		{
			int score = faker.Random.Int();
			string scoreString = faker.Random.Utf16String();
			string extraData = faker.Lorem.Sentence();
			string user = faker.Internet.UserName();
			int userId = faker.Random.Int();
			string guest = faker.Internet.UserName();
			string storedExtraData = faker.Random.Utf16String();
			long storedTimestamp = faker.Random.Long();

			ScoreInternal scoreInternal = new ScoreInternal(score, scoreString, extraData, user, userId, guest, storedExtraData, storedTimestamp);

			Assert.That(scoreInternal.ToString(),
				Is.EqualTo(
					$"{nameof(ScoreInternal)} (sort: {score}, score: {scoreString}, extraData: {extraData}, username: {user}, userId: {userId}, guestName: {guest}, storedTimestamp: {storedTimestamp}, stored: {storedExtraData})"));
		}

		[Test]
		public void SubmitScoreResponse()
		{
			bool success = faker.Random.Bool();
			string message = faker.Lorem.Sentence();

			SubmitScoreResponse response = new SubmitScoreResponse(success, message);

			Assert.That(response.ToString(), Is.EqualTo($"{nameof(SubmitScoreResponse)} (Success: {success}, Message: {message})"));
		}

		[Test]
		public void TableInternal()
		{
			int id = faker.Random.Int();
			string name = faker.Internet.UserName();
			string description = faker.Lorem.Sentence();
			bool primary = faker.Random.Bool();

			TableInternal tableInternal = new TableInternal(id, name, description, primary);

			Assert.That(tableInternal.ToString(),
				Is.EqualTo($"{nameof(TableInternal)} (id: {id}, name: {name}, description: {description}, isPrimary: {primary})"));
		}

		[Test]
		public void GameJoltScore()
		{
			int sort = faker.Random.Int();
			string score = faker.Random.Utf16String();
			string extraData = faker.Lorem.Sentence();
			string username = faker.Internet.UserName();
			int userId = faker.Random.Int();
			string guestName = faker.Internet.UserName();
			DateTime stored = faker.Date.Past();

			GameJoltScore gameJoltScore = new GameJoltScore(sort, score, extraData, username, userId, guestName, stored);

			Assert.That(gameJoltScore.ToString(),
				Is.EqualTo(
					$"{nameof(GameJoltScore)} (Sort: {sort}, Score: {score}, ExtraData: {extraData}, Username: {username}, UserId: {userId}, GuestName: {guestName}, Stored: {stored})"));
		}

		[Test]
		public void GameJoltTable()
		{
			int id = faker.Random.Int();
			string name = faker.Internet.UserName();
			string description = faker.Lorem.Sentence();
			bool primary = faker.Random.Bool();

			GameJoltTable gameJoltTable = new GameJoltTable(id, name, description, primary);

			Assert.That(gameJoltTable.ToString(),
				Is.EqualTo($"{nameof(GameJoltTable)} (Id: {id}, Name: {name}, Description: {description}, IsPrimary: {primary})"));
		}

		[Test]
		public void GetScoresQuery([Values] bool hasTableId,
			[Values] bool hasUser,
			[Values] bool hasGuest,
			[Values] bool hasBetterThan,
			[Values] bool hasWorseThan)
		{
			StringBuilder sb = new StringBuilder();
			int limit = faker.Random.Int(1, 100);

			sb.Append($"{nameof(GetScoresQuery)} (limit: {limit}");

			int tableId = faker.Random.Int();
			if (hasTableId)
			{
				sb.Append($", tableId: {tableId}");
			}

			string username = faker.Internet.UserName();
			string userToken = faker.Random.Utf16String();
			if (hasUser)
			{
				sb.Append($", username: {username}, userToken: {userToken}");
			}

			string guest = faker.Internet.UserName();
			if (hasGuest)
			{
				sb.Append($", guest: {guest}");
			}

			int betterThan = faker.Random.Int();
			if (hasBetterThan)
			{
				sb.Append($", betterThan: {betterThan}");
			}

			int worseThan = faker.Random.Int();
			if (hasWorseThan)
			{
				sb.Append($", worseThan: {worseThan}");
			}

			sb.Append(')');

			GetScoresQuery query = new GetScoresQuery(null, hasTableId ? tableId : null, limit, hasUser ? username : null, hasUser ? userToken : null,
				hasGuest ? guest : null, hasBetterThan ? betterThan : null, hasWorseThan ? worseThan : null);

			Assert.That(query.ToString(), Is.EqualTo(sb.ToString()));
		}
	}
}