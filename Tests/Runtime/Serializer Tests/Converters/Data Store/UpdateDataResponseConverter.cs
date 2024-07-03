#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

#nullable enable

using GameJolt.NET.Tests.Enums;
using GameJolt.NET.Tests.Extensions;
using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests.Serialization.Converters
{
	public sealed class UpdateDataResponseConverter : BaseSerializationTest
	{
		private static readonly object?[] dataValues = new object?[] { "String Data", 123, 123.456, true, false, null };
		
		[Test]
		public void WriteJson([Values] bool success, [Values] bool nullMessage, [ValueSource(nameof(dataValues))] object? value)
		{
			string? message = nullMessage ? null : faker.Lorem.Sentence();

			UpdateDataResponse response = new UpdateDataResponse(success, message, value == null ? "" : value.ToString());
			string json = Serialize(response);
			
			string expected = WriteExpectedResponse(success, message, sb =>
			{
				sb.Append(',');
				sb.AppendJsonPropertyName("data");
				if (value == null)
				{
					sb.Append("\"\"");
				}
				else
				{
					sb.AppendStringValue(value.ToString());
				}
			});
			
			Assert.That(json, Is.EqualTo(expected));
		}
		
		[Test]
		public void ReadJson([Values] bool success, [Values] bool nullMessage, [Values] bool randomCapitalize, [ValueSource(nameof(dataValues))] object? value)
		{
			string? message = nullMessage ? null : faker.Lorem.Sentence();
			
			string json = WriteExpectedResponse(success, message, sb =>
			{
				sb.Append(',');
				sb.AppendJsonPropertyName(randomCapitalize ? "data".RandomCapitalize() : "data");
				if (value == null)
				{
					sb.Append("\"\"");
				}
				else
				{
					sb.AppendStringValue(value.ToString());
				}
			});
			
			UpdateDataResponse response = Deserialize<UpdateDataResponse>(json);
			
			Assert.That(response.Success, Is.EqualTo(success));
			Assert.That(response.Message, Is.EqualTo(message));
			Assert.That(response.data, Is.EqualTo(value == null ? "" : value.ToString()));
		}
		
		[Test]
		public void ReadJson_TooManyFields([Values] bool success, [Values] bool nullMessage, [Values] bool randomCapitalize, [Values] bool beforeData, [ValueSource(nameof(dataValues))] object? value)
		{
			string? message = nullMessage ? null : faker.Lorem.Sentence();
			
			string json = WriteExpectedResponse(success, message, sb =>
			{
				if (beforeData)
				{
					sb.Append(',');
					sb.AppendJsonPropertyName("extra");
					sb.AppendStringValue(faker.Lorem.Sentence());
				}
				
				sb.Append(',');
				sb.AppendJsonPropertyName(randomCapitalize ? "data".RandomCapitalize() : "data");
				if (value == null)
				{
					sb.Append("\"\"");
				}
				else
				{
					sb.AppendStringValue(value.ToString());
				}
				
				if (!beforeData)
				{
					sb.Append(',');
					sb.AppendJsonPropertyName("extra");
					sb.AppendStringValue(faker.Lorem.Sentence());
				}
			});
			
			UpdateDataResponse response = Deserialize<UpdateDataResponse>(json);
			
			Assert.That(response.Success, Is.EqualTo(success));
			Assert.That(response.Message, Is.EqualTo(message));
			Assert.That(response.data, Is.EqualTo(value == null ? "" : value.ToString()));
		}
	}
}
#endif // DISABLE_GAMEJOLT