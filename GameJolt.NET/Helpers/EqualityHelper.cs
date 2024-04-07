#nullable enable

using System;
using System.Collections.Generic;

namespace Hertzole.GameJolt
{
	internal static class EqualityHelper
	{
		public static bool ArrayEquals<T>(T[]? a, T[]? b)
		{
			if (a == null && b == null)
			{
				return true;
			}

			if (a == null || b == null)
			{
				return false;
			}

			if (a.Length != b.Length)
			{
				return false;
			}

			for (int i = 0; i < a.Length; i++)
			{
				if (!EqualityComparer<T>.Default.Equals(a[i], b[i]))
				{
					return false;
				}
			}

			return true;
		}

		public static bool StringEquals(string? a, string? b)
		{
			if (a == null && b == null)
			{
				return true;
			}

			if (a == null || b == null)
			{
				return false;
			}

			return string.Equals(a, b, StringComparison.Ordinal);
		}
	}
}