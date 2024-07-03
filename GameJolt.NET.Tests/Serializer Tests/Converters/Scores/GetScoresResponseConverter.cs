#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

#nullable enable

using System;
using System.Text;
using GameJolt.NET.Tests.Enums;
using GameJolt.NET.Tests.Extensions;
using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests.Serialization.Converters
{
	public sealed class GetScoresResponseConverter : BaseSerializationTest
	{
		[Test]
		public void WriteJson([Values] bool success, [Values] bool nullMessage, [Values] ArrayInitialization scoresInitialization)
		{
			ScoreInternal[]? scores = scoresInitialization.CreateArray(f => new ScoreInternal(f.Random.Int(), f.Random.Int().ToString(), f.Lorem.Sentence(),
				f.Internet.UserName(), f.Random.Int(), f.Internet.UserName(), f.Lorem.Sentence(), f.Date.PastOffset().ToUnixTimeSeconds()));

			string? message = nullMessage ? null : faker.Lorem.Sentence();

			GetScoresResponse response = new GetScoresResponse(success, message, scores);
			string json = Serialize(response);

			string expected = WriteExpectedResponse(success, message, sb =>
			{
				sb.Append(',');
				sb.AppendJsonPropertyName("scores");
				sb.AppendArray(scores, WriteScore, true);
			});

			Assert.That(json, Is.EqualTo(expected));
		}

		[Test]
		public void ReadJson([Values] bool success,
			[Values] bool nullMessage,
			[Values] ArrayInitialization scoresInitialization,
			[Values] bool randomCapitalize)
		{
			ScoreInternal[]? scores = scoresInitialization.CreateArray(f => new ScoreInternal(f.Random.Int(), f.Random.Int().ToString(), f.Lorem.Sentence(),
				f.Internet.UserName(), f.Random.Int(), f.Internet.UserName(), f.Lorem.Sentence(), f.Date.PastOffset().ToUnixTimeSeconds()));

			string? message = nullMessage ? null : faker.Lorem.Sentence();

			ScoreInternal[]? scores1 = scores;
			string json = WriteExpectedResponse(success, message, sb =>
			{
				sb.Append(',');
				sb.AppendJsonPropertyName("scores", randomCapitalize);
				sb.AppendArray(scores1, WriteScore);
			});

			GetScoresResponse response = Deserialize<GetScoresResponse>(json);

			scores ??= Array.Empty<ScoreInternal>();

			Assert.That(response.success, Is.EqualTo(success));
			Assert.That(response.message, Is.EqualTo(message));
			Assert.That(response.scores, Is.EqualTo(scores));
		}

		[Test]
		public void ReadJson_TooManyElements([Values] bool success,
			[Values] bool nullMessage,
			[Values] ArrayInitialization scoresInitialization,
			[Values] bool randomCapitalize,
			[Values] bool beforeScores)
		{
			ScoreInternal[]? scores = scoresInitialization.CreateArray(f => new ScoreInternal(f.Random.Int(), f.Random.Int().ToString(), f.Lorem.Sentence(),
				f.Internet.UserName(), f.Random.Int(), f.Internet.UserName(), f.Lorem.Sentence(), f.Date.PastOffset().ToUnixTimeSeconds()));

			string? message = nullMessage ? null : faker.Lorem.Sentence();

			ScoreInternal[]? scores1 = scores;
			string json = WriteExpectedResponse(success, message, sb =>
			{
				if (beforeScores)
				{
					sb.Append(',');
					sb.AppendJsonPropertyName("extra", randomCapitalize);
					sb.AppendStringValue("extra data");
				}

				sb.Append(',');
				sb.AppendJsonPropertyName("scores", randomCapitalize);
				sb.AppendArray(scores1, WriteScore);

				if (!beforeScores)
				{
					sb.Append(',');
					sb.AppendJsonPropertyName("extra", randomCapitalize);
					sb.AppendStringValue("extra data");
				}
			});

			// Arrays should never be null once they are deserialized.
			GetScoresResponse response = Deserialize<GetScoresResponse>(json);

			scores ??= Array.Empty<ScoreInternal>();

			Assert.That(response.success, Is.EqualTo(success));
			Assert.That(response.message, Is.EqualTo(message));
			Assert.That(response.scores, Is.EqualTo(scores));
		}

		private static void WriteScore(StringBuilder builder, ScoreInternal score)
		{
			builder.Append('{');
			builder.AppendJsonPropertyName("sort");
			builder.Append(score.sort);
			builder.Append(',');
			builder.AppendJsonPropertyName("score");
			builder.AppendStringValue(score.score);
			builder.Append(',');
			builder.AppendJsonPropertyName("extra_data");
			builder.AppendStringValue(score.extraData);
			builder.Append(',');
			builder.AppendJsonPropertyName("user");
			builder.AppendStringValue(score.username);
			builder.Append(',');
			builder.AppendJsonPropertyName("user_id");
			builder.Append(score.userId);
			builder.Append(',');
			builder.AppendJsonPropertyName("guest");
			builder.AppendStringValue(score.guestName);
			builder.Append(',');
			builder.AppendJsonPropertyName("stored_timestamp");
			builder.Append(score.storedTimestamp);
			builder.Append(',');
			builder.AppendJsonPropertyName("stored");
			builder.AppendStringValue(score.stored);
			builder.Append('}');
		}
	}
}
#endif // DISABLE_GAMEJOLT