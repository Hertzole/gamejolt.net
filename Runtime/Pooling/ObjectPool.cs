#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

#nullable enable
using System;
using System.Collections.Generic;
#if NET9_0_OR_GREATER
using System.Threading;
#endif

namespace Hertzole.GameJolt
{
	internal sealed class ObjectPool<T> where T : class
	{
		private readonly Stack<T> pool = new Stack<T>();
		private readonly Func<T> createFunc;
		private readonly Action<T>? onGet;
		private readonly Action<T>? onReturn;
#if NET9_0_OR_GREATER
		private readonly Lock lockObject = new Lock();
#else
		private readonly object lockObject = new object();
#endif

		public ObjectPool(Func<T> createFunc, Action<T>? onGet = null, Action<T>? onReturn = null)
		{
			this.createFunc = createFunc;
			this.onGet = onGet;
			this.onReturn = onReturn;
		}

		public T Rent()
		{
			lock (lockObject)
			{
				if (!pool.TryPop(out T? item))
				{
					item = createFunc();
				}

				onGet?.Invoke(item);
				return item;
			}
		}

		public PoolHandle<T> Rent(out T item)
		{
			lock (lockObject)
			{
				item = Rent();
				return new PoolHandle<T>(this, item);
			}
		}

		public void Return(T obj)
		{
			lock (lockObject)
			{
				onReturn?.Invoke(obj);
				pool.Push(obj);
			}
		}
	}
}
#endif // DISABLE_GAMEJOLT