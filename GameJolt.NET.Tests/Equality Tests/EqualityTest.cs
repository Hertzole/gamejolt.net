using System;
using NUnit.Framework;

namespace GameJolt.NET.Tests.Equality
{
	public class EqualityTest
	{
		protected static void TestEquality<T>(Func<T, T, bool> compare, Func<T, T, bool> invertCompare, params T[] values) where T : struct, IEquatable<T>
		{
			T a = values[0];
			T b = values[0];

			object bObj = b;

			Assert.That(a, Is.EqualTo(b), "First value should be equal to itself.");
			Assert.That(a, Is.EqualTo(bObj), "First value should be equal to itself in object form.");
			Assert.That(a.GetHashCode(), Is.EqualTo(b.GetHashCode()), "First value should have the same hash code as itself.");
			Assert.That(compare.Invoke(a, b), Is.True, "First value should be equal to itself by comparison.");

			for (int i = 1; i < values.Length; i++)
			{
				b = values[i];
				bObj = b;

				Assert.That(a, Is.Not.EqualTo(b), "First value should not be equal to the second value.");
				Assert.That(a, Is.Not.EqualTo(bObj), "First value should not be equal to the second value in object form.");
				Assert.That(a.GetHashCode(), Is.Not.EqualTo(b.GetHashCode()), "First value should not have the same hash code as the second value.");
				Assert.That(invertCompare.Invoke(a, b), Is.True, "First value should not be equal to the second value by comparison.");
			}
		}
	}
}