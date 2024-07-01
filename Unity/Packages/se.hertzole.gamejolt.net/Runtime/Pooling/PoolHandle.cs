using System;
using System.Collections.Generic;

namespace Hertzole.GameJolt
{
	internal readonly struct PoolHandle<T> : IDisposable, IEquatable<PoolHandle<T>> where T : class
	{
		private readonly ObjectPool<T> pool;
		private readonly T obj;

		public PoolHandle(ObjectPool<T> pool, T obj)
		{
			this.pool = pool;
			this.obj = obj;
		}

		public bool Equals(PoolHandle<T> other)
		{
			return ReferenceEquals(pool, other.pool) && EqualityComparer<T>.Default.Equals(obj, other.obj);
		}

		public override bool Equals(object otherObj)
		{
			return otherObj is PoolHandle<T> other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((pool != null ? pool.GetHashCode() : 0) * 397) ^ (obj != null ? obj.GetHashCode() : 0);
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
			pool.Return(obj);
		}
	}
}