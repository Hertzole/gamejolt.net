#nullable enable

using System.Text;
using GameJolt.NET.Tests.Enums;
using GameJolt.NET.Tests.Extensions;
using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests.Serialization.Converters
{
	public sealed class DataKeyConverter : BaseSerializationTest
	{
		[Test]
		public void WriteJson()
		{
			string key = faker.Lorem.Word();
			DataKey dataKey = new DataKey(key);
			
			string json = Serialize(dataKey);
			
			Assert.That(json, Is.EqualTo($"{{\"key\":\"{key}\"}}"));
		}

		[Test]
		public void ReadJson([Values] bool randomCapitalize, [Values] StringInitialization keyInitialization)
		{
			string? key = keyInitialization.GetData();

			StringBuilder sb = new StringBuilder();
			sb.Append("{");
			sb.AppendJsonPropertyName(randomCapitalize ? "key".RandomCapitalize() : "key");
			keyInitialization.AppendToBuilder(sb, key);
			sb.Append("}");
			
			string json = sb.ToString();
			
			key ??= string.Empty;
			
			DataKey dataKey = Deserialize<DataKey>(json);
			
			Assert.That(dataKey.key, Is.EqualTo(key));
		}

		[Test]
		public void ReadJson_TooManyFields([Values] bool randomCapitalize, [Values] StringInitialization keyInitialization, [Values] bool beforeKey)
		{
			string? key = keyInitialization.GetData();

			StringBuilder sb = new StringBuilder();
			sb.Append("{");

			if (beforeKey)
			{
				sb.AppendJsonPropertyName(randomCapitalize ? "extra".RandomCapitalize() : "extra");
				sb.AppendStringValue(faker.Lorem.Sentence());
				sb.Append(",");
			}
			
			sb.AppendJsonPropertyName(randomCapitalize ? "key".RandomCapitalize() : "key");
			keyInitialization.AppendToBuilder(sb, key);
			
			if (!beforeKey)
			{
				sb.Append(",");
				sb.AppendJsonPropertyName(randomCapitalize ? "extra".RandomCapitalize() : "extra");
				sb.AppendStringValue(faker.Lorem.Sentence());
			}
			
			sb.Append("}");
			
			string json = sb.ToString();
			
			key ??= string.Empty;
			
			DataKey dataKey = Deserialize<DataKey>(json);
			
			Assert.That(dataKey.key, Is.EqualTo(key));
		}
	}
}