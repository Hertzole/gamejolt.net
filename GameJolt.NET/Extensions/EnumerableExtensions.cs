using System.Collections.Generic;

namespace Hertzole.GameJolt
{
	internal static class EnumerableExtensions
	{
		public static string ToCommaSeparatedString<T>(this IEnumerable<T> array)
		{
#if NETSTANDARD2_1_OR_GREATER || UNITY_2021_1_OR_NEWER || NET5_0_OR_GREATER
			return string.Join(',', array);
#else
			return string.Join(",", array);
#endif
		}
	}
}