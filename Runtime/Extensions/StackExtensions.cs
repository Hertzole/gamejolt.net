#if !NETSTANDARD2_1_OR_GREATER && !NET5_0_OR_GREATER
#nullable enable
using System.Collections.Generic;

namespace Hertzole.GameJolt
{
	internal static class StackExtensions
	{
		public static bool TryPop<T>(this Stack<T> stack, out T item)
		{
			if (stack.Count > 0)
			{
				item = stack.Pop();
				return true;
			}

			item = default!;
			return false;
		}
	}
}
#endif