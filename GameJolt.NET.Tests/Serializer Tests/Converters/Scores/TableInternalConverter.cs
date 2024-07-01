#nullable enable

using System.Text;
using GameJolt.NET.Tests.Enums;
using GameJolt.NET.Tests.Extensions;
using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests.Serialization.Converters
{
	public sealed class TableInternalConverter : BaseSerializationTest
	{
		[Test]
		public void WriteJson([Values] bool hasName, [Values] bool hasDescription, [Values] bool isPrimary)
		{
			int id = faker.Random.Int();
			string? name = hasName ? faker.Lorem.Word() : null;
			string? description = hasDescription ? faker.Lorem.Sentence() : null;

			TableInternal table = new TableInternal(id, name, description, isPrimary);

			string json = Serialize(table);
			string expected = WriteTableJson(id, name, description, isPrimary, false, false);

			Assert.That(json, Is.EqualTo(expected));
		}

		[Test]
		public void ReadJson([Values] StringInitialization nameInitialization,
			[Values] StringInitialization descriptionInitialization,
			[Values] bool isPrimary,
			[Values] bool randomCapitalize,
			[Values] bool writeNull)
		{
			int id = faker.Random.Int();
			string? name = nameInitialization.GetData();
			string? description = descriptionInitialization.GetData();

			string json = WriteTableJson(id, name, description, isPrimary, writeNull, randomCapitalize);
			TableInternal table = Deserialize<TableInternal>(json);

			// Serializers return empty strings instead of null.
			name ??= string.Empty;
			description ??= string.Empty;

			Assert.That(table.id, Is.EqualTo(id));
			Assert.That(table.name, Is.EqualTo(name));
			Assert.That(table.description, Is.EqualTo(description));
			Assert.That(table.isPrimary, Is.EqualTo(isPrimary));
		}

		[Test]
		public void ReadJson_TooManyFields([Values] StringInitialization nameInitialization,
			[Values] StringInitialization descriptionInitialization,
			[Values] bool isPrimary,
			[Values] bool randomCapitalize,
			[Values] bool writeNull,
			[Values] bool unknownBefore)
		{
			int id = faker.Random.Int();
			string? name = nameInitialization.GetData();
			string? description = descriptionInitialization.GetData();

			string json = WriteTableJson(id, name, description, isPrimary, writeNull, randomCapitalize, true, unknownBefore);
			TableInternal table = Deserialize<TableInternal>(json);

			// Serializers return empty strings instead of null.
			name ??= string.Empty;
			description ??= string.Empty;

			Assert.That(table.id, Is.EqualTo(id));
			Assert.That(table.name, Is.EqualTo(name));
			Assert.That(table.description, Is.EqualTo(description));
			Assert.That(table.isPrimary, Is.EqualTo(isPrimary));
		}

		private static string WriteTableJson(int id,
			string? name,
			string? description,
			bool isPrimary,
			bool writeNull,
			bool randomCapitalize,
			bool hasUnknownType = false,
			bool unknownBefore = false)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append('{');

			if (hasUnknownType && unknownBefore)
			{
				sb.AppendJsonPropertyName("unknown");
				sb.AppendStringValue("unknown");
				sb.Append(',');
			}

			sb.AppendJsonPropertyName("id", randomCapitalize);
			sb.Append(id);
			sb.Append(',');
			sb.AppendJsonPropertyName("name", randomCapitalize);
			sb.AppendStringValue(name, writeNull);
			sb.Append(',');
			sb.AppendJsonPropertyName("description", randomCapitalize);
			sb.AppendStringValue(description, writeNull);
			sb.Append(',');
			sb.AppendJsonPropertyName("primary", randomCapitalize);
			sb.Append(isPrimary ? "true" : "false");

			if (hasUnknownType && !unknownBefore)
			{
				sb.Append(',');
				sb.AppendJsonPropertyName("unknown");
				sb.AppendStringValue("unknown");
			}

			sb.Append('}');
			return sb.ToString();
		}
	}
}