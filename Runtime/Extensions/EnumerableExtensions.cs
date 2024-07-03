#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

#nullable enable

using System.Collections.Generic;

namespace Hertzole.GameJolt
{
	internal static class EnumerableExtensions
	{
		public static string ToCommaSeparatedString<T>(this IEnumerable<T>? array)
		{
			if (array == null)
			{
				return string.Empty;
			}

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER || UNITY_2021_3_OR_NEWER
			return string.Join(',', array);
#else
			return string.Join(",", array);
#endif
		}
	}
}
#endif // DISABLE_GAMEJOLT