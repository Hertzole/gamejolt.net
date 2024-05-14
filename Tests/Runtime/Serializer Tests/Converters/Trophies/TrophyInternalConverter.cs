#nullable enable

using System.Text;
using GameJolt.NET.Tests.Enums;
using GameJolt.NET.Tests.Extensions;
using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests.Serialization.Converters
{
	public sealed class TrophyInternalConverter : BaseSerializationTest
	{
		[Test]
		public void WriteJson()
		{
			int id = faker.Random.Int();
			string title = faker.Lorem.Word();
			string description = faker.Lorem.Sentence();
			TrophyDifficulty difficulty = faker.PickRandom<TrophyDifficulty>();
			string image = faker.Internet.Url();
			bool achieved = faker.Random.Bool();

			TrophyInternal trophy = new TrophyInternal(id, title, description, difficulty, image, achieved);

			string json = Serialize(trophy);

			string expected = WriteTrophyJson(id, title, description, difficulty, image, achieved);

			Assert.That(json, Is.EqualTo(expected));
		}

		[Test]
		public void ReadJson([Values] bool randomCapitalize,
			[Values] StringInitialization titleInitialization,
			[Values] StringInitialization descriptionInitialization,
			[Values] StringInitialization imageInitialization,
			[Values] bool writeNull)
		{
			int id = faker.Random.Int();
			string? title = titleInitialization.GetData();
			string? description = descriptionInitialization.GetData();
			TrophyDifficulty difficulty = faker.PickRandom<TrophyDifficulty>();
			string? image = imageInitialization.GetData();
			bool achieved = faker.Random.Bool();

			string json = WriteTrophyJson(id, title, description, difficulty, image, achieved, randomCapitalize, writeNull);

			TrophyInternal trophy = Deserialize<TrophyInternal>(json);

			// Serializers returns empty string if null.
			title ??= string.Empty;
			description ??= string.Empty;
			image ??= string.Empty;

			Assert.That(trophy.id, Is.EqualTo(id));
			Assert.That(trophy.title, Is.EqualTo(title));
			Assert.That(trophy.description, Is.EqualTo(description));
			Assert.That(trophy.difficulty, Is.EqualTo(difficulty));
			Assert.That(trophy.imageUrl, Is.EqualTo(image));
			Assert.That(trophy.achieved, Is.EqualTo(achieved));
		}

		[Test]
		public void ReadJson_TooManyFields([Values] StringInitialization titleInitialization,
			[Values] StringInitialization descriptionInitialization,
			[Values] StringInitialization imageInitialization,
			[Values] bool randomCapitalize,
			[Values] bool beforeData,
			[Values] bool writeNull)
		{
			int id = faker.Random.Int();
			string? title = titleInitialization.GetData();
			string? description = descriptionInitialization.GetData();
			TrophyDifficulty difficulty = faker.PickRandom<TrophyDifficulty>();
			string? image = imageInitialization.GetData();
			bool achieved = faker.Random.Bool();

			string json = WriteTrophyJson(id, title, description, difficulty, image, achieved, randomCapitalize, writeNull, true, beforeData);

			TrophyInternal trophy = Deserialize<TrophyInternal>(json);

			// Serializers returns empty string if null.
			title ??= string.Empty;
			description ??= string.Empty;
			image ??= string.Empty;

			Assert.That(trophy.id, Is.EqualTo(id));
			Assert.That(trophy.title, Is.EqualTo(title));
			Assert.That(trophy.description, Is.EqualTo(description));
			Assert.That(trophy.difficulty, Is.EqualTo(difficulty));
			Assert.That(trophy.imageUrl, Is.EqualTo(image));
			Assert.That(trophy.achieved, Is.EqualTo(achieved));
		}

		private static string WriteTrophyJson(int id,
			string? title,
			string? description,
			TrophyDifficulty difficulty,
			string? image,
			bool achieved,
			bool randomCapitalize = false,
			bool writeNull = false,
			bool hasExtraData = false,
			bool beforeData = false)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append('{');

			if (hasExtraData && beforeData)
			{
				sb.AppendJsonPropertyName("extra_field", randomCapitalize);
				sb.AppendStringValue("haha");
				sb.Append(',');
			}

			sb.AppendJsonPropertyName("id", randomCapitalize);
			sb.Append(id);
			sb.Append(',');
			sb.AppendJsonPropertyName("title", randomCapitalize);
			sb.AppendStringValue(title, writeNull);
			sb.Append(',');
			sb.AppendJsonPropertyName("description", randomCapitalize);
			sb.AppendStringValue(description, writeNull);
			sb.Append(',');
			sb.AppendJsonPropertyName("difficulty", randomCapitalize);
			sb.AppendStringValue(difficulty.ToString());
			sb.Append(',');
			sb.AppendJsonPropertyName("image_url", randomCapitalize);
			sb.AppendStringValue(image, writeNull);
			sb.Append(',');
			sb.AppendJsonPropertyName("achieved", randomCapitalize);
			if (achieved)
			{
				sb.AppendStringValue("true");
			}
			else
			{
				sb.Append("false");
			}

			if (hasExtraData && !beforeData)
			{
				sb.Append(',');
				sb.AppendJsonPropertyName("extra_field", randomCapitalize);
				sb.AppendStringValue("haha");
			}

			sb.Append('}');

			return sb.ToString();
		}
	}
}