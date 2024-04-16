#nullable enable

using System.Collections.Generic;
using System.Linq;
using Hertzole.GameJolt;

namespace GameJolt.NET.Tests.Extensions
{
	internal static class EnumerableExtensions
	{
		public static string GetExpectedString<T>(this IEnumerable<T>? enumerable)
		{
			if (enumerable == null)
			{
				return string.Empty;
			}

			T[] array = enumerable as T[] ?? enumerable.ToArray();
			return array.Length == 0 ? string.Empty : array.ToCommaSeparatedString();
		}
	}
}