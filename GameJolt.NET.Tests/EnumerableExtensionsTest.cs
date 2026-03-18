#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

#nullable enable

using System;
using System.Collections;
using System.Text;
using Bogus;
using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests
{
	public sealed class EnumerableExtensionsTest
	{
		private readonly Faker faker = new Faker();

		private static IEnumerable ValuesSource()
		{
			yield return new TestCaseData((object) DummyData.faker.Random.WordsArray(10)).SetName("String array");
			yield return new TestCaseData((object) DummyData.faker.Random.WordsArray(1)).SetName("Single string");
			yield return new TestCaseData(DummyData.faker.Random.Digits(10)).SetName("Int array");
			yield return new TestCaseData(DummyData.faker.Random.Digits(1)).SetName("Single int");
			yield return new TestCaseData(DummyData.faker.Random.Chars()).SetName("Char array");
			yield return new TestCaseData(DummyData.faker.Random.Chars(count: 1)).SetName("Single char");
			yield return new TestCaseData((object) Array.Empty<string>()).SetName("Empty string array");
			yield return new TestCaseData(Array.Empty<int>()).SetName("Empty int array");
			yield return new TestCaseData(Array.Empty<char>()).SetName("Empty char array");
		}

		[Test]
		[TestCaseSource(nameof(ValuesSource))]
		public void ToCommaSeparatedString<T>(T[] array) where T : notnull
		{
			// Arrange
			string expected = string.Join(",", array);

			// Result
			string result = array.ToCommaSeparatedString();

			Assert.That(result, Is.EqualTo(expected));
		}

		[Test]
		public void ToCommaSeparatedString_Null()
		{
			string[]? array = null;
			Assert.That(array.ToCommaSeparatedString(), Is.EqualTo(string.Empty));
		}

		[Test]
		[TestCaseSource(nameof(ValuesSource))]
		public void AppendCommaSeparatedString<T>(T[] array) where T : notnull
		{
			// Arrange
			string expected = string.Join(",", array);
			StringBuilder sb = new StringBuilder();

			// Act
			sb.AppendCommaSeparatedString<T>(array);

			// Assert
			Assert.That(sb.ToString(), Is.EqualTo(expected));
		}
	}
}
#endif // DISABLE_GAMEJOLT