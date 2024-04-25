#nullable enable

using System.Text;
using GameJolt.NET.Tests.Enums;
using GameJolt.NET.Tests.Extensions;
using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests.Serialization.Converters
{
	public sealed class ScoreInternalConverter : BaseSerializationTest
	{
		[Test]
		public void WriteJson([Values] bool hasExtraData, [Values] bool hasUsername, [Values] bool hasUserId, [Values] bool hasGuestName)
		{
			int sort = faker.Random.Int();
			string score = faker.Random.Int().ToString();
			string? extraData = hasExtraData ? faker.Lorem.Sentence() : null;
			string? username = hasUsername ? faker.Internet.UserName() : null;
			int userId = hasUserId ? faker.Random.Int() : 0;
			string? guestName = hasGuestName ? faker.Internet.UserName() : null;
			long storedTimestamp = faker.Date.PastOffset().ToUnixTimeSeconds();
			string stored = storedTimestamp.ToString();

			ScoreInternal scoreInternal = new ScoreInternal(sort, score, extraData, username, userId, guestName, stored, storedTimestamp);

			string json = Serialize(scoreInternal);

			string expected = WriteScoreJson(sort, score, extraData, username, userId, guestName, storedTimestamp, stored, false, false);

			Assert.That(json, Is.EqualTo(expected));
		}

		[Test]
		public void ReadJson([Values] StringInitialization scoreInitialization,
			[Values] StringInitialization extraDataInitialization,
			[Values] StringInitialization userInitialization,
			[Values] StringInitialization guestNameInitialization,
			[Values] StringInitialization storedInitialization,
			[Values] bool hasUserId,
			[Values] bool writeNull,
			[Values] bool randomCapitalize)
		{
			int sort = faker.Random.Int();
			string? score = scoreInitialization.GetData();
			string? extraData = extraDataInitialization.GetData();
			string? username = userInitialization.GetData();
			int userId = hasUserId ? faker.Random.Int() : 0;
			string? guestName = guestNameInitialization.GetData();
			long storedTimestamp = faker.Date.PastOffset().ToUnixTimeSeconds();
			string? stored = storedInitialization.GetData();

			string json = WriteScoreJson(sort, score, extraData, username, userId, guestName, storedTimestamp, stored, writeNull, randomCapitalize);

			ScoreInternal scoreInternal = Deserialize<ScoreInternal>(json);

			// Serializers return empty strings instead of null.
			score ??= string.Empty;
			extraData ??= string.Empty;
			username ??= string.Empty;
			guestName ??= string.Empty;
			stored ??= string.Empty;

			Assert.That(scoreInternal.sort, Is.EqualTo(sort));
			Assert.That(scoreInternal.score, Is.EqualTo(score));
			Assert.That(scoreInternal.extraData, Is.EqualTo(extraData));
			Assert.That(scoreInternal.username, Is.EqualTo(username));
			Assert.That(scoreInternal.userId, Is.EqualTo(userId));
			Assert.That(scoreInternal.guestName, Is.EqualTo(guestName));
			Assert.That(scoreInternal.storedTimestamp, Is.EqualTo(storedTimestamp));
			Assert.That(scoreInternal.stored, Is.EqualTo(stored));
		}

		[Test]
		public void ReadJson_TooManyFields([Values] StringInitialization scoreInitialization,
			[Values] StringInitialization extraDataInitialization,
			[Values] StringInitialization userInitialization,
			[Values] StringInitialization guestNameInitialization,
			[Values] StringInitialization storedInitialization,
			[Values] bool hasUserId,
			[Values] bool writeNull,
			[Values] bool randomCapitalize,
			[Values] bool beforeFields)
		{
			int sort = faker.Random.Int();
			string? score = scoreInitialization.GetData();
			string? extraData = extraDataInitialization.GetData();
			string? username = userInitialization.GetData();
			int userId = hasUserId ? faker.Random.Int() : 0;
			string? guestName = guestNameInitialization.GetData();
			long storedTimestamp = faker.Date.PastOffset().ToUnixTimeSeconds();
			string? stored = storedInitialization.GetData();

			string json = WriteScoreJson(sort, score, extraData, username, userId, guestName, storedTimestamp, stored, writeNull, randomCapitalize, true,
				beforeFields);

			ScoreInternal scoreInternal = Deserialize<ScoreInternal>(json);

			// Serializers return empty strings instead of null.
			score ??= string.Empty;
			extraData ??= string.Empty;
			username ??= string.Empty;
			guestName ??= string.Empty;
			stored ??= string.Empty;

			Assert.That(scoreInternal.sort, Is.EqualTo(sort));
			Assert.That(scoreInternal.score, Is.EqualTo(score));
			Assert.That(scoreInternal.extraData, Is.EqualTo(extraData));
			Assert.That(scoreInternal.username, Is.EqualTo(username));
			Assert.That(scoreInternal.userId, Is.EqualTo(userId));
			Assert.That(scoreInternal.guestName, Is.EqualTo(guestName));
			Assert.That(scoreInternal.storedTimestamp, Is.EqualTo(storedTimestamp));
			Assert.That(scoreInternal.stored, Is.EqualTo(stored));
		}

		private static string WriteScoreJson(int sort,
			string? score,
			string? extraData,
			string? username,
			int userId,
			string? guestName,
			long storedTimestamp,
			string? stored,
			bool writeNull,
			bool randomCapitalize,
			bool hasUnknownType = false,
			bool unknownBefore = false)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("{");

			if (hasUnknownType && unknownBefore)
			{
				sb.AppendJsonPropertyName("unknown_field", randomCapitalize);
				sb.AppendStringValue("hehehehe");
				sb.Append(",");
			}

			sb.AppendJsonPropertyName("sort", randomCapitalize);
			sb.Append(sort);
			sb.Append(",");
			sb.AppendJsonPropertyName("score", randomCapitalize);
			sb.AppendStringValue(score, writeNull);
			sb.Append(",");
			sb.AppendJsonPropertyName("extra_data", randomCapitalize);
			sb.AppendStringValue(extraData, writeNull);
			sb.Append(",");
			sb.AppendJsonPropertyName("user", randomCapitalize);
			sb.AppendStringValue(username, writeNull);
			sb.Append(",");
			sb.AppendJsonPropertyName("user_id", randomCapitalize);
			sb.Append(userId);
			sb.Append(",");
			sb.AppendJsonPropertyName("guest", randomCapitalize);
			sb.AppendStringValue(guestName, writeNull);
			sb.Append(",");
			sb.AppendJsonPropertyName("stored_timestamp", randomCapitalize);
			sb.Append(storedTimestamp);
			sb.Append(",");
			sb.AppendJsonPropertyName("stored", randomCapitalize);
			sb.AppendStringValue(stored, writeNull);

			if (hasUnknownType && !unknownBefore)
			{
				sb.Append(",");
				sb.AppendJsonPropertyName("unknown_field", randomCapitalize);
				sb.AppendStringValue("hehehehe");
			}

			sb.Append("}");

			return sb.ToString();
		}
	}
}