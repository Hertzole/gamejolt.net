#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

#nullable enable

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

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

		public static void AppendCommaSeparatedString<T>(this StringBuilder builder, ReadOnlySpan<T> list) where T : notnull
		{
			if (list.IsEmpty)
			{
				return;
			}

			int length = list.Length;
			int lastIndex = list.Length - 1;

#if NET6_0_OR_GREATER
			if (typeof(T).IsValueType && default(T) is ISpanFormattable)
			{
				DefaultInterpolatedStringHandler result = new DefaultInterpolatedStringHandler(0, 0, CultureInfo.InvariantCulture);
				for (int i = 0; i < length; i++)
				{
					result.AppendFormatted(list[i]);
					if (i < lastIndex)
					{
						result.AppendFormatted(',');
					}
				}

				builder.Append(result.ToStringAndClear());
				return;
			}
#endif

			if (length == 1)
			{
				builder.Append(list[0]);
				return;
			}

			for (int i = 0; i < length; i++)
			{
				builder.Append(list[i]);
				if (i < lastIndex)
				{
					builder.Append(',');
				}
			}
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

		public static MemoryOwner<T> GetMemory<T>(this IList<T> list)
		{
			T[] array = ArrayPool<T>.Shared.Rent(list.Count);
			list.CopyTo(array, 0);
			return new MemoryOwner<T>(array, list.Count);
		}

		public static MemoryOwner<T> GetMemory<T>(this IEnumerable<T> enumerable)
		{
			List<T> list = ListPool<T>.Rent();
			list.AddRange(enumerable);
			return new MemoryOwner<T>(list);
		}
	}
}
#endif // DISABLE_GAMEJOLT