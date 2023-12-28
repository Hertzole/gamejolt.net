using System;
using System.Globalization;
using Bogus;
using Hertzole.GameJolt;
using NUnit.Framework;
#if NET6_0_OR_GREATER
using JsonException = System.Text.Json.JsonException;
#else
using JsonException = Newtonsoft.Json.JsonSerializationException;
#endif

namespace GameJolt.NET.Tests
{
	public class SerializerStringOrNumberTests
	{
		private static readonly Randomizer randomizer = new Randomizer();
		private static readonly Faker faker = new Faker();

		private static object[] testCases =
		{
			GetRandomInt(true, true),
			GetRandomInt(true, false),
			GetRandomInt(false, true),
			GetRandomInt(false, false),
			GetRandomDouble(true, true),
			GetRandomDouble(true, false),
			GetRandomDouble(false, true),
			GetRandomDouble(false, false),
			GetRandomString()
		};

		[Test]
		[TestCaseSource(nameof(testCases))]
		public void CanDeserialize(object value, object expected)
		{
			UpdateDataResponse response = GameJoltAPI.serializer.Deserialize<UpdateDataResponse>("{\"response\":{\"success\":true,\"data\":" + value + "}}");
			if (expected is IFormattable formattable)
			{
				Assert.That(response.data, Is.EqualTo(formattable.ToString(null, CultureInfo.InvariantCulture)));
			}
			else
			{
				Assert.That(response.data, Is.EqualTo(expected.ToString()));
			}
		}

		[Test]
		public void EmptyString_Success()
		{
			UpdateDataResponse response = GameJoltAPI.serializer.Deserialize<UpdateDataResponse>("{\"response\":{\"success\":true,\"data\":\"\"}}");
			Assert.That(response.data, Is.EqualTo(string.Empty));
		}

		private static object[] GetRandomInt(bool asString, bool positive)
		{
			int number = randomizer.Int(1);
			if (!positive)
			{
				number *= -1;
			}

			if (asString)
			{
				return new object[] { $"\"{number}\"", number };
			}

			return new object[] { number.ToString(), number };
		}

		private static object[] GetRandomDouble(bool asString, bool positive)
		{
			double number = randomizer.Double();
			if (!positive)
			{
				number *= -1;
			}

			if (asString)
			{
				return new object[] { $"\"{number}\"", number };
			}

			return new object[] { number.ToString(CultureInfo.InvariantCulture), number };
		}

		private static object[] GetRandomString()
		{
			string str = faker.Lorem.Sentence();
			return new object[] { $"\"{str}\"", str };
		}
	}
}