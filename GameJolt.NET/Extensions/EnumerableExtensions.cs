#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

#nullable enable

using System.Collections.Generic;
using System.Runtime.CompilerServices;

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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ClearAndEnsureCapacity<T>(this IList<T> list, int capacity)
		{
			list.Clear();

			if (list is List<T> concreteList)
			{
				concreteList.EnsureCapacity(capacity);
			}
		}
	}
}
#endif // DISABLE_GAMEJOLT