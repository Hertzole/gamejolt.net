#if !DISABLE_GAMEJOLT
#nullable enable

using System;
using System.Buffers;

namespace Hertzole.GameJolt
{
	internal readonly struct MemoryOwner<T> : IDisposable
	{
		private readonly T[] array;
		private readonly ArrayPool<T> pool;

		public int Length { get; }

		public T this[int index]
		{
			get { return array[index]; }
		}

		public static MemoryOwner<T> Empty
		{
			get { return new MemoryOwner<T>(Array.Empty<T>(), 0); }
		}

		public MemoryOwner(T[] array, int length, ArrayPool<T>? pool = null)
		{
			this.array = array;
			Length = length;
			this.pool = pool ?? ArrayPool<T>.Shared;
		}

		/// <inheritdoc />
		public void Dispose()
		{
			if (array.Length > 0)
			{
				pool.Return(array);
			}
		}
	}
}
#endif // !DISABLE_GAMEJOLT