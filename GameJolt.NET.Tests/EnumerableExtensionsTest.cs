#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

#nullable enable

using System;
using System.Text;
using Bogus;
using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests
{
	public sealed class EnumerableExtensionsTest
	{
		private readonly Faker faker = new Faker();

		[Test]
		public void ToCommaSeparatedString()
		{
			string[] array = faker.Random.WordsArray(10);

			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < array.Length; i++)
			{
				if (i > 0)
				{
					sb.Append(',');
				}

				sb.Append(array[i]);
			}

			Assert.That(array.ToCommaSeparatedString(), Is.EqualTo(sb.ToString()));
		}

		[Test]
		public void ToCommaSeparatedString_Empty()
		{
			string[] array = Array.Empty<string>();
			Assert.That(array.ToCommaSeparatedString(), Is.EqualTo(string.Empty));
		}

		[Test]
		public void ToCommaSeparatedString_Null()
		{
			string[]? array = null;
			Assert.That(array.ToCommaSeparatedString(), Is.EqualTo(string.Empty));
		}
	}
}
#endif // DISABLE_GAMEJOLT