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
	public sealed class GetTablesResponseConverter : BaseSerializationTest
	{
		[Test]
		public void WriteJson([Values] bool success, [Values] bool nullMessage, [Values] ArrayInitialization tablesInitialization)
		{
			TableInternal[]? tables = tablesInitialization.CreateArray(CreateTable);
			string? message = nullMessage ? null : faker.Lorem.Sentence();

			GetTablesResponse response = new GetTablesResponse(success, message, tables);
			string json = Serialize(response);

			string expected = WriteExpectedResponse(success, message, sb =>
			{
				sb.Append(",");
				sb.AppendJsonPropertyName("tables");
				sb.AppendArray(tables, WriteTable, true);
			});

			Assert.That(json, Is.EqualTo(expected));
		}

		[Test]
		public void ReadJson([Values] bool success,
			[Values] bool nullMessage,
			[Values] ArrayInitialization tablesInitialization,
			[Values] bool randomCapitalize)
		{
			TableInternal[]? tables = tablesInitialization.CreateArray(CreateTable);
			string? message = nullMessage ? null : faker.Lorem.Sentence();

			TableInternal[]? tables1 = tables;
			string json = WriteExpectedResponse(success, message, sb =>
			{
				sb.Append(",");
				sb.AppendJsonPropertyName("tables", randomCapitalize);
				sb.AppendArray(tables1, WriteTable);
			});

			GetTablesResponse response = Deserialize<GetTablesResponse>(json);

			tables ??= Array.Empty<TableInternal>();

			Assert.That(response.success, Is.EqualTo(success));
			Assert.That(response.message, Is.EqualTo(message));
			Assert.That(response.tables, Is.EqualTo(tables));
		}

		[Test]
		public void ReadJson_TooManyElements([Values] bool success,
			[Values] bool nullMessage,
			[Values] ArrayInitialization tablesInitialization,
			[Values] bool randomCapitalize,
			[Values] bool beforeTables)
		{
			TableInternal[]? tables = tablesInitialization.CreateArray(CreateTable);
			string? message = nullMessage ? null : faker.Lorem.Sentence();

			TableInternal[]? tables1 = tables;
			string json = WriteExpectedResponse(success, message, sb =>
			{
				if (beforeTables)
				{
					sb.Append(",");
					sb.AppendJsonPropertyName("extra", randomCapitalize);
					sb.AppendStringValue("extra data");
				}

				sb.Append(",");
				sb.AppendJsonPropertyName("tables", randomCapitalize);
				sb.AppendArray(tables1, WriteTable);

				if (!beforeTables)
				{
					sb.Append(",");
					sb.AppendJsonPropertyName("extra", randomCapitalize);
					sb.AppendStringValue("extra data");
				}
			});

			// Arrays should never be null once they are deserialized.
			GetTablesResponse response = Deserialize<GetTablesResponse>(json);

			tables ??= Array.Empty<TableInternal>();

			Assert.That(response.success, Is.EqualTo(success));
			Assert.That(response.message, Is.EqualTo(message));
			Assert.That(response.tables, Is.EqualTo(tables));
		}

		private static TableInternal CreateTable(Faker faker)
		{
			return new TableInternal(faker.Random.Int(), faker.Lorem.Word(), faker.Lorem.Sentence(), faker.Random.Bool());
		}

		private static void WriteTable(StringBuilder builder, TableInternal table)
		{
			builder.Append("{");
			builder.AppendJsonPropertyName("id");
			builder.Append(table.id);
			builder.Append(",");
			builder.AppendJsonPropertyName("name");
			builder.AppendStringValue(table.name);
			builder.Append(",");
			builder.AppendJsonPropertyName("description");
			builder.AppendStringValue(table.description);
			builder.Append(",");
			builder.AppendJsonPropertyName("primary");
			builder.Append(table.isPrimary.ToString().ToLowerInvariant());
			builder.Append("}");
		}
	}
}