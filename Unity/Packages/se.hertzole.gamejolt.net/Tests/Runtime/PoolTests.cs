#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests
{
	public sealed class PoolTests
	{
		[Test]
		public void Rent_Return_IsSameObject()
		{
			ObjectPool<object> pool = new ObjectPool<object>(() => new object());

			object obj = pool.Rent();
			pool.Return(obj);

			object obj2 = pool.Rent();

			Assert.That(obj, Is.SameAs(obj2));
		}

		[Test]
		public void Rent_Return_Handle_IsSameObject()
		{
			ObjectPool<object> pool = new ObjectPool<object>(() => new object());

			PoolHandle<object> handle = pool.Rent(out object obj);
			handle.Dispose();

			PoolHandle<object> handle2 = pool.Rent(out object obj2);

			Assert.That(obj, Is.SameAs(obj2));
			Assert.That(handle, Is.EqualTo(handle2));
		}

		[Test]
		public void Rent_OnGet_IsCalled()
		{
			bool called = false;
			ObjectPool<object> pool = new ObjectPool<object>(() => new object(), _ => called = true);

			pool.Rent();

			Assert.That(called, Is.True);
		}

		[Test]
		public void Return_OnReturn_IsCalled()
		{
			bool called = false;
			ObjectPool<object> pool = new ObjectPool<object>(() => new object(), onReturn: _ => called = true);

			object obj = pool.Rent();
			pool.Return(obj);

			Assert.That(called, Is.True);
		}
	}
}
#endif // DISABLE_GAMEJOLT