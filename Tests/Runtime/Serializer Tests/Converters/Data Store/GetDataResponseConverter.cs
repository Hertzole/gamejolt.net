#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

#nullable enable

using GameJolt.NET.Tests.Enums;
using GameJolt.NET.Tests.Extensions;
using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests.Serialization.Converters
{
	public sealed class GetDataResponseConverter : BaseSerializationTest
	{
		[Test]
		public void WriteJson([Values] bool success, [Values] bool nullMessage)
		{
			string data = faker.Lorem.Sentence();
			string? message = nullMessage ? null : faker.Lorem.Sentence();

			GetDataResponse response = new GetDataResponse(success, message, data);
			string json = Serialize(response);
			
			string expected = WriteExpectedResponse(success, message, sb =>
			{
				sb.Append(',');
				sb.AppendJsonPropertyName("data");
				sb.AppendStringValue(data);
			});
			
			Assert.That(json, Is.EqualTo(expected));
		}
		
		[Test]
		public void ReadJson([Values] bool success, [Values] bool nullMessage, [Values] bool randomCapitalize, [Values] StringInitialization dataInitialization)
		{
			string? data = dataInitialization.GetData();
			string? message = nullMessage ? null : faker.Lorem.Sentence();

			string? data1 = data;
			string json = WriteExpectedResponse(success, message, sb =>
			{
				sb.Append(',');
				sb.AppendJsonPropertyName(randomCapitalize ? "data".RandomCapitalize() : "data");
				dataInitialization.AppendToBuilder(sb, data1);
			});
			
			GetDataResponse response = Deserialize<GetDataResponse>(json);
			
			data ??= string.Empty;
			
			Assert.That(response.Success, Is.EqualTo(success));
			Assert.That(response.Message, Is.EqualTo(message));
			Assert.That(response.data, Is.EqualTo(data));
		}
		
		[Test]
		public void ReadJson_TooManyFields([Values] bool success, [Values] bool nullMessage, [Values] bool randomCapitalize, [Values] bool beforeData, [Values] StringInitialization dataInitialization)
		{
			string? data = dataInitialization.GetData();
			string? message = nullMessage ? null : faker.Lorem.Sentence();
			
			string? data1 = data;
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
				dataInitialization.AppendToBuilder(sb, data1);
				
				if (!beforeData)
				{
					sb.Append(',');
					sb.AppendJsonPropertyName("extra");
					sb.AppendStringValue(faker.Lorem.Sentence());
				}
			});
			
			data ??= string.Empty;
			
			GetDataResponse response = Deserialize<GetDataResponse>(json);
			
			Assert.That(response.Success, Is.EqualTo(success));
			Assert.That(response.Message, Is.EqualTo(message));
			Assert.That(response.data, Is.EqualTo(data));
		}
	}
}
#endif // DISABLE_GAMEJOLT