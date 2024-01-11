#nullable enable
using System;
using System.Collections.Generic;

namespace Hertzole.GameJolt
{
	internal sealed class ObjectPool<T> where T : class
	{
		private readonly Stack<T> pool = new Stack<T>();
		private readonly Func<T> createFunc;
		private readonly Action<T>? onGet;
		private readonly Action<T>? onReturn;

		public ObjectPool(Func<T> createFunc, Action<T>? onGet = null, Action<T>? onReturn = null)
		{
			this.createFunc = createFunc;
			this.onGet = onGet;
			this.onReturn = onReturn;
		}

		public T Rent()
		{
			if (!pool.TryPop(out T? item))
			{
				item = createFunc();
			}

			onGet?.Invoke(item);
			return item;
		}

		public PoolHandle<T> Rent(out T item)
		{
			item = Rent();
			return new PoolHandle<T>(this, item);
		}

		public void Return(T obj)
		{
			onReturn?.Invoke(obj);
			pool.Push(obj);
		}
	}
}