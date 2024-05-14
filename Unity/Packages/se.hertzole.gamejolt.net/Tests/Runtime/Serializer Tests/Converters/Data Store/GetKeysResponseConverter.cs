#nullable enable

using System;
using GameJolt.NET.Tests.Enums;
using GameJolt.NET.Tests.Extensions;
using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests.Serialization.Converters
{
	public sealed class GetKeysResponseConverter : BaseSerializationTest
	{
		[Test]
		public void WriteJson([Values] bool success, [Values] bool nullMessage, [Values] ArrayInitialization arrayInitialization)
		{
			DataKey[]? keys = arrayInitialization.CreateArray(f => new DataKey(faker.Lorem.Word()));
			string? message = nullMessage ? null : faker.Lorem.Sentence();

			GetKeysResponse response = new GetKeysResponse(success, message, keys);
			string json = Serialize(response);
			
			string expected = WriteExpectedResponse(success, message, sb =>
			{
				sb.Append(",");
				sb.AppendJsonPropertyName("keys");
				sb.AppendArray(keys, (builder, key) =>
				{
					builder.Append("{");
					builder.AppendJsonPropertyName("key");
					builder.AppendStringValue(key.key);
					builder.Append("}");
				}, true); // Serializers write [] instead of null for null arrays.
			});
			
			Assert.That(json, Is.EqualTo(expected));
		}
		
		[Test]
		public void ReadJson([Values] bool success, [Values] bool nullMessage, [Values] bool randomCapitalize, [Values] ArrayInitialization keysInitialization)
		{
			DataKey[]? keys = keysInitialization.CreateArray(f => new DataKey(faker.Lorem.Word()));
			string? message = nullMessage ? null : faker.Lorem.Sentence();

			DataKey[]? keys1 = keys;
			string json = WriteExpectedResponse(success, message, sb =>
			{
				sb.Append(",");
				sb.AppendJsonPropertyName(randomCapitalize ? "keys".RandomCapitalize() : "keys");
				sb.AppendArray(keys1, (builder, key) =>
				{
					builder.Append("{");
					builder.AppendJsonPropertyName("key");
					builder.AppendStringValue(key.key);
					builder.Append("}");
				});
			});
			
			GetKeysResponse response = Deserialize<GetKeysResponse>(json);

			// Arrays should never be null once they are deserialized.
			keys ??= Array.Empty<DataKey>();
			
			Assert.That(response.Success, Is.EqualTo(success));
			Assert.That(response.Message, Is.EqualTo(message));
			Assert.That(response.keys, Is.EqualTo(keys));
		}
		
		[Test]
		public void ReadJson_TooManyFields([Values] bool success, [Values] bool nullMessage, [Values] bool randomCapitalize, [Values] bool beforeData, [Values] ArrayInitialization keysInitialization)
		{
			DataKey[]? keys = keysInitialization.CreateArray(f => new DataKey(faker.Lorem.Word()));
			string? message = nullMessage ? null : faker.Lorem.Sentence();
			
			DataKey[]? keys1 = keys;
			string json = WriteExpectedResponse(success, message, sb =>
			{
				if (beforeData)
				{
					sb.Append(",");
					sb.AppendJsonPropertyName("extra");
					sb.AppendStringValue(faker.Lorem.Sentence());
				}
				
				sb.Append(",");
				sb.AppendJsonPropertyName(randomCapitalize ? "keys".RandomCapitalize() : "keys");
				sb.AppendArray(keys1, (builder, key) =>
				{
					builder.Append("{");
					builder.AppendJsonPropertyName("key");
					builder.AppendStringValue(key.key);
					builder.Append("}");
				});
				
				if (!beforeData)
				{
					sb.Append(",");
					sb.AppendJsonPropertyName("extra");
					sb.AppendStringValue(faker.Lorem.Sentence());
				}
			});
			
			keys ??= Array.Empty<DataKey>();
			
			GetKeysResponse response = Deserialize<GetKeysResponse>(json);
			
			Assert.That(response.Success, Is.EqualTo(success));
			Assert.That(response.Message, Is.EqualTo(message));
			Assert.That(response.keys, Is.EqualTo(keys));
		}
	}
}