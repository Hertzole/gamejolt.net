#if !DISABLE_GAMEJOLT
#nullable enable

using System.Collections.Generic;

namespace Hertzole.GameJolt
{
	/// <summary>
	///     A pool for lists. Rents a list and clears it when returned to the pool.
	/// </summary>
	/// <typeparam name="T">The type of the list.</typeparam>
	internal static class ListPool<T>
	{
		private static readonly ObjectPool<List<T>> pool = new ObjectPool<List<T>>(static () => new List<T>(), onReturn: static list => list.Clear());

		public static PoolHandle<List<T>> Rent(out List<T> list)
		{
			return pool.Rent(out list);
		}

		public static List<T> Rent()
		{
			return pool.Rent();
		}

		public static void Return(List<T> list)
		{
			pool.Return(list);
		}
	}
}
#endif