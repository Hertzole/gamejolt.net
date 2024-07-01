#nullable enable

using GameJolt.NET.Tests.Extensions;
using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests.Serialization.Converters
{
	public sealed class GetScoreRankResponseConverter : BaseSerializationTest
	{
		[Test]
		public void WriteJson([Values] bool success, [Values] bool nullMessage)
		{
			int rank = faker.Random.Int();
			string? message = nullMessage ? null : faker.Lorem.Sentence();

			GetScoreRankResponse response = new GetScoreRankResponse(success, message, rank);

			string json = Serialize(response);

			string expected = WriteExpectedResponse(success, message, sb =>
			{
				sb.Append(',');
				sb.AppendJsonPropertyName("rank");
				sb.Append(rank);
			});

			Assert.That(json, Is.EqualTo(expected));
		}

		[Test]
		public void ReadJson([Values] bool success, [Values] bool nullMessage, [Values] bool randomCapitalize, [Values] bool stringRank)
		{
			int rank = faker.Random.Int();
			string? message = nullMessage ? null : faker.Lorem.Sentence();

			string json = WriteExpectedResponse(success, message, sb =>
			{
				sb.Append(',');
				sb.AppendJsonPropertyName("rank", randomCapitalize);
				if (stringRank)
				{
					sb.Append('\"');
					sb.Append(rank);
					sb.Append('\"');
				}
				else
				{
					sb.Append(rank);
				}
			});

			GetScoreRankResponse response = Deserialize<GetScoreRankResponse>(json);

			Assert.That(response.success, Is.EqualTo(success));
			Assert.That(response.message, Is.EqualTo(message));
			Assert.That(response.rank, Is.EqualTo(rank));
		}

		[Test]
		public void ReadJson_TooManyFields([Values] bool success,
			[Values] bool nullMessage,
			[Values] bool randomCapitalize,
			[Values] bool stringRank,
			[Values] bool beforeRank)
		{
			int rank = faker.Random.Int();
			string? message = nullMessage ? null : faker.Lorem.Sentence();

			string json = WriteExpectedResponse(success, message, sb =>
			{
				if (beforeRank)
				{
					sb.Append(',');
					sb.AppendJsonPropertyName("extra_property", randomCapitalize);
					sb.AppendStringValue(faker.Lorem.Sentence());
				}

				sb.Append(',');
				sb.AppendJsonPropertyName("rank", randomCapitalize);
				if (stringRank)
				{
					sb.Append('\"');
					sb.Append(rank);
					sb.Append('\"');
				}
				else
				{
					sb.Append(rank);
				}

				if (!beforeRank)
				{
					sb.Append(',');
					sb.AppendJsonPropertyName("extra_property", randomCapitalize);
					sb.AppendStringValue(faker.Lorem.Sentence());
				}
			});

			GetScoreRankResponse response = Deserialize<GetScoreRankResponse>(json);

			Assert.That(response.success, Is.EqualTo(success));
			Assert.That(response.message, Is.EqualTo(message));
			Assert.That(response.rank, Is.EqualTo(rank));
		}
	}
}