#if !DISABLE_GAMEJOLT
#nullable enable

using System;
using System.Buffers;
using System.Collections.Generic;

namespace Hertzole.GameJolt
{
	internal readonly struct MemoryOwner<T> : IDisposable
	{
		private readonly T[] array;
		private readonly List<T>? list;

		public int Length { get; }

		public T this[int index]
		{
			get { return array[index]; }
		}

		public static MemoryOwner<T> Empty
		{
			get { return new MemoryOwner<T>(Array.Empty<T>(), 0); }
		}

		public MemoryOwner(T[] array, int length)
		{
			this.array = array;
			Length = length;
			list = null;
		}

		public MemoryOwner(List<T> list)
		{
			this.list = list;
			Length = list.Count;
			if (list.Count > 0)
			{
				array = ArrayPool<T>.Shared.Rent(list.Count);
				list.CopyTo(array, 0);
			}
			else
			{
				array = Array.Empty<T>();
			}
		}

		public Memory<T> AsMemory()
		{
			return new Memory<T>(array, 0, Length);
		}

		/// <inheritdoc />
		public void Dispose()
		{
			if (array.Length > 0)
			{
				ArrayPool<T>.Shared.Return(array);
			}

			if (list != null)
			{
				ListPool<T>.Return(list);
			}
		}
	}
}
#endif // !DISABLE_GAMEJOLT