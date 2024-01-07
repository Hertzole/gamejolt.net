using System;

namespace Hertzole.GameJolt
{
	internal readonly struct PoolHandle<T> : IDisposable where T : class
	{
		private readonly ObjectPool<T> pool;
		private readonly T obj;

		public PoolHandle(ObjectPool<T> pool, T obj)
		{
			this.pool = pool;
			this.obj = obj;
		}

		public void Dispose()
		{
			pool.Return(obj);
		}
	}
}