#nullable enable

using System;
using System.Text;
using Bogus;
using GameJolt.NET.Tests.Enums;
using GameJolt.NET.Tests.Extensions;
using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests.Serialization.Converters
{
	public sealed class FetchTrophiesResponseConverter : BaseSerializationTest
	{
		[Test]
		public void WriteJson([Values] bool success, [Values] bool nullMessage, [Values] ArrayInitialization trophiesInitialization)
		{
			TrophyInternal[]? trophies = trophiesInitialization.CreateArray(CreateTrophy);
			string? message = nullMessage ? null : faker.Lorem.Sentence();

			FetchTrophiesResponse response = new FetchTrophiesResponse(success, message, trophies);
			string json = Serialize(response);

			string expected = WriteExpectedResponse(success, message, sb =>
			{
				sb.Append(',');
				sb.AppendJsonPropertyName("trophies");
				sb.AppendArray(trophies, WriteTrophy, true);
			});

			Assert.That(json, Is.EqualTo(expected));
		}

		[Test]
		public void ReadJson([Values] bool success,
			[Values] bool nullMessage,
			[Values] bool randomCapitalize,
			[Values] ArrayInitialization trophiesInitialization)
		{
			TrophyInternal[]? trophies = trophiesInitialization.CreateArray(CreateTrophy);
			string? message = nullMessage ? null : faker.Lorem.Sentence();

			TrophyInternal[]? trophies1 = trophies;
			string json = WriteExpectedResponse(success, message, sb =>
			{
				sb.Append(',');
				sb.AppendJsonPropertyName("trophies", randomCapitalize);
				sb.AppendArray(trophies1, WriteTrophy);
			});

			FetchTrophiesResponse response = Deserialize<FetchTrophiesResponse>(json);

			// Serializers returns empty array if null.
			trophies ??= Array.Empty<TrophyInternal>();

			Assert.That(response.Success, Is.EqualTo(success));
			Assert.That(response.Message, Is.EqualTo(message));
			Assert.That(response.trophies, Is.EqualTo(trophies));
		}

		[Test]
		public void ReadJson_TooManyFields([Values] bool success,
			[Values] bool nullMessage,
			[Values] bool randomCapitalize,
			[Values] bool beforeData,
			[Values] ArrayInitialization trophiesInitialization)
		{
			TrophyInternal[]? trophies = trophiesInitialization.CreateArray(CreateTrophy);
			string? message = nullMessage ? null : faker.Lorem.Sentence();

			TrophyInternal[]? trophies1 = trophies;
			string json = WriteExpectedResponse(success, message, sb =>
			{
				if (beforeData)
				{
					sb.Append(',');
					sb.AppendJsonPropertyName("extra_field");
					sb.AppendStringValue(faker.Lorem.Word());
				}

				sb.Append(',');
				sb.AppendJsonPropertyName("trophies", randomCapitalize);
				sb.AppendArray(trophies1, WriteTrophy);

				if (!beforeData)
				{
					sb.Append(',');
					sb.AppendJsonPropertyName("extra_field");
					sb.AppendStringValue(faker.Lorem.Word());
				}
			});

			FetchTrophiesResponse response = Deserialize<FetchTrophiesResponse>(json);

			// Serializers returns empty array if null.
			trophies ??= Array.Empty<TrophyInternal>();

			Assert.That(response.Success, Is.EqualTo(success));
			Assert.That(response.Message, Is.EqualTo(message));
			Assert.That(response.trophies, Is.EqualTo(trophies));
		}

		private static TrophyInternal CreateTrophy(Faker faker)
		{
			return new TrophyInternal(faker.Random.Int(), faker.Lorem.Word(), faker.Lorem.Sentence(), faker.PickRandom<TrophyDifficulty>(),
				faker.Internet.Avatar(), faker.Random.Bool());
		}

		private static void WriteTrophy(StringBuilder sb, TrophyInternal trophy)
		{
			sb.Append('{');
			sb.AppendJsonPropertyName("id");
			sb.Append(trophy.id);
			sb.Append(',');
			sb.AppendJsonPropertyName("title");
			sb.AppendStringValue(trophy.title);
			sb.Append(',');
			sb.AppendJsonPropertyName("description");
			sb.AppendStringValue(trophy.description);
			sb.Append(',');
			sb.AppendJsonPropertyName("difficulty");
			sb.AppendStringValue(trophy.difficulty.ToString());
			sb.Append(',');
			sb.AppendJsonPropertyName("image_url");
			sb.AppendStringValue(trophy.imageUrl);
			sb.Append(',');
			sb.AppendJsonPropertyName("achieved");
			// Achieved can also be a date, because Game Jolt, so it writes 
			if (trophy.achieved)
			{
				sb.AppendStringValue("true");
			}
			else
			{
				sb.Append("false");
			}

			sb.Append('}');
		}
	}
}