#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

#nullable enable

using GameJolt.NET.Tests.Enums;
using GameJolt.NET.Tests.Extensions;
using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests.Serialization.Converters
{
	public sealed class FetchTimeResponseConverter : BaseSerializationTest
	{
		[Test]
		public void WriteJson([Values] bool success, [Values] bool nullMessage)
		{
			long timestamp = faker.Random.Long();
			string timezone = faker.Date.TimeZoneString();
			int year = faker.Random.Int();
			int month = faker.Random.Int();
			int day = faker.Random.Int();
			int hour = faker.Random.Int();
			int minute = faker.Random.Int();
			int second = faker.Random.Int();
			string? message = nullMessage ? null : faker.Lorem.Sentence();

			FetchTimeResponse response = new FetchTimeResponse(timestamp, timezone, year, month, day, hour, minute, second, success, message);
			string json = Serialize(response);

			string expected = WriteExpectedResponse(success, message, sb =>
			{
				sb.Append(',');
				sb.AppendJsonPropertyName("timestamp");
				sb.Append(timestamp);
				sb.Append(',');
				sb.AppendJsonPropertyName("timezone");
				sb.AppendStringValue(timezone);
				sb.Append(',');
				sb.AppendJsonPropertyName("year");
				sb.Append(year);
				sb.Append(',');
				sb.AppendJsonPropertyName("month");
				sb.Append(month);
				sb.Append(',');
				sb.AppendJsonPropertyName("day");
				sb.Append(day);
				sb.Append(',');
				sb.AppendJsonPropertyName("hour");
				sb.Append(hour);
				sb.Append(',');
				sb.AppendJsonPropertyName("minute");
				sb.Append(minute);
				sb.Append(',');
				sb.AppendJsonPropertyName("second");
				sb.Append(second);
			});

			Assert.That(json, Is.EqualTo(expected));
		}

		[Test]
		public void ReadJson([Values] bool success,
			[Values] bool nullMessage,
			[Values] bool randomCapitalize,
			[Values] StringInitialization timezoneInitialization)
		{
			long timestamp = faker.Random.Long();
			string? timezone = timezoneInitialization.GetData();
			int year = faker.Random.Int();
			int month = faker.Random.Int();
			int day = faker.Random.Int();
			int hour = faker.Random.Int();
			int minute = faker.Random.Int();
			int second = faker.Random.Int();
			string? message = nullMessage ? null : faker.Lorem.Sentence();

			string json = WriteExpectedResponse(success, message, sb =>
			{
				sb.Append(',');
				sb.AppendJsonPropertyName("timestamp", randomCapitalize);
				sb.Append(timestamp);
				sb.Append(',');
				sb.AppendJsonPropertyName("timezone", randomCapitalize);
				timezoneInitialization.AppendToBuilder(sb, timezone);
				sb.Append(',');
				sb.AppendJsonPropertyName("year", randomCapitalize);
				sb.Append(year);
				sb.Append(',');
				sb.AppendJsonPropertyName("month", randomCapitalize);
				sb.Append(month);
				sb.Append(',');
				sb.AppendJsonPropertyName("day", randomCapitalize);
				sb.Append(day);
				sb.Append(',');
				sb.AppendJsonPropertyName("hour", randomCapitalize);
				sb.Append(hour);
				sb.Append(',');
				sb.AppendJsonPropertyName("minute", randomCapitalize);
				sb.Append(minute);
				sb.Append(',');
				sb.AppendJsonPropertyName("second", randomCapitalize);
				sb.Append(second);
			});

			FetchTimeResponse response = Deserialize<FetchTimeResponse>(json);

			// Serializers will never return null for strings.
			timezone ??= string.Empty;

			Assert.That(response.timestamp, Is.EqualTo(timestamp));
			Assert.That(response.timezone, Is.EqualTo(timezone));
			Assert.That(response.year, Is.EqualTo(year));
			Assert.That(response.month, Is.EqualTo(month));
			Assert.That(response.day, Is.EqualTo(day));
			Assert.That(response.hour, Is.EqualTo(hour));
			Assert.That(response.minute, Is.EqualTo(minute));
			Assert.That(response.second, Is.EqualTo(second));
		}

		[Test]
		public void ReadJson_TooManyFields([Values] bool success,
			[Values] bool nullMessage,
			[Values] bool randomCapitalize,
			[Values] StringInitialization timezoneInitialization,
			[Values] bool beforeData)
		{
			long timestamp = faker.Random.Long();
			string? timezone = timezoneInitialization.GetData();
			int year = faker.Random.Int();
			int month = faker.Random.Int();
			int day = faker.Random.Int();
			int hour = faker.Random.Int();
			int minute = faker.Random.Int();
			int second = faker.Random.Int();
			string? message = nullMessage ? null : faker.Lorem.Sentence();

			string json = WriteExpectedResponse(success, message, sb =>
			{
				if (beforeData)
				{
					sb.Append(',');
					sb.AppendJsonPropertyName("extra");
					sb.AppendStringValue(faker.Lorem.Word());
				}

				sb.Append(',');
				sb.AppendJsonPropertyName("timestamp", randomCapitalize);
				sb.Append(timestamp);
				sb.Append(',');
				sb.AppendJsonPropertyName("timezone", randomCapitalize);
				timezoneInitialization.AppendToBuilder(sb, timezone);
				sb.Append(',');
				sb.AppendJsonPropertyName("year", randomCapitalize);
				sb.Append(year);
				sb.Append(',');
				sb.AppendJsonPropertyName("month", randomCapitalize);
				sb.Append(month);
				sb.Append(',');
				sb.AppendJsonPropertyName("day", randomCapitalize);
				sb.Append(day);
				sb.Append(',');
				sb.AppendJsonPropertyName("hour", randomCapitalize);
				sb.Append(hour);
				sb.Append(',');
				sb.AppendJsonPropertyName("minute", randomCapitalize);
				sb.Append(minute);
				sb.Append(',');
				sb.AppendJsonPropertyName("second", randomCapitalize);
				sb.Append(second);

				if (!beforeData)
				{
					sb.Append(',');
					sb.AppendJsonPropertyName("extra");
					sb.AppendStringValue(faker.Lorem.Word());
				}
			});

			FetchTimeResponse response = Deserialize<FetchTimeResponse>(json);

			// Serializers will never return null for strings.
			timezone ??= string.Empty;

			Assert.That(response.timestamp, Is.EqualTo(timestamp));
			Assert.That(response.timezone, Is.EqualTo(timezone));
			Assert.That(response.year, Is.EqualTo(year));
			Assert.That(response.month, Is.EqualTo(month));
			Assert.That(response.day, Is.EqualTo(day));
			Assert.That(response.hour, Is.EqualTo(hour));
			Assert.That(response.minute, Is.EqualTo(minute));
			Assert.That(response.second, Is.EqualTo(second));
		}
	}
}
#endif // DISABLE_GAMEJOLT