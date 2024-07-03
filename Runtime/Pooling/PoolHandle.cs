#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

using System;
using System.Collections.Generic;

namespace Hertzole.GameJolt
{
	internal readonly struct PoolHandle<T> : IDisposable, IEquatable<PoolHandle<T>> where T : class
	{
		private readonly ObjectPool<T> pool;
		private readonly T item;

		public PoolHandle(ObjectPool<T> pool, T item)
		{
			this.pool = pool;
			this.item = item;
		}

		public bool Equals(PoolHandle<T> other)
		{
			return ReferenceEquals(pool, other.pool) && EqualityComparer<T>.Default.Equals(item, other.item);
		}

		public override bool Equals(object obj)
		{
			return obj is PoolHandle<T> other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((pool != null ? pool.GetHashCode() : 0) * 397) ^ (item != null ? item.GetHashCode() : 0);
			}
		}

		public static bool operator ==(PoolHandle<T> left, PoolHandle<T> right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(PoolHandle<T> left, PoolHandle<T> right)
		{
			return !left.Equals(right);
		}

		public void Dispose()
		{
			pool.Return(item);
		}
	}
}
#endif // DISABLE_GAMEJOLT