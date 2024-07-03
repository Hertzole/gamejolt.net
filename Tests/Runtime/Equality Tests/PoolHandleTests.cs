#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

using System.Text;
using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests.Equality
{
	public sealed class PoolHandleTests : EqualityTest
	{
		[Test]
		public void Success()
		{
			ObjectPool<StringBuilder> pool1 = new ObjectPool<StringBuilder>(() => new StringBuilder());
			StringBuilder obj1 = pool1.Rent();
			ObjectPool<StringBuilder> pool2 = new ObjectPool<StringBuilder>(() => new StringBuilder());
			StringBuilder obj2 = pool2.Rent();

			PoolHandle<StringBuilder> handle1 = new PoolHandle<StringBuilder>(pool1, obj1);
			PoolHandle<StringBuilder> handle2 = new PoolHandle<StringBuilder>(pool2, obj2);

			TestEquality((a, b) => a == b, (a, b) => a != b, handle1, handle2);
		}

		[Test]
		public void DifferentPools_Fail()
		{
			ObjectPool<StringBuilder> pool1 = new ObjectPool<StringBuilder>(() => new StringBuilder());
			StringBuilder obj1 = pool1.Rent();
			ObjectPool<StringBuilder> pool2 = new ObjectPool<StringBuilder>(() => new StringBuilder());

			PoolHandle<StringBuilder> handle1 = new PoolHandle<StringBuilder>(pool1, obj1);
			PoolHandle<StringBuilder> handle2 = new PoolHandle<StringBuilder>(pool2, obj1);

			TestEquality((a, b) => a == b, (a, b) => a != b, handle1, handle2);
		}

		[Test]
		public void NullValues_Fail()
		{
			ObjectPool<StringBuilder> pool1 = new ObjectPool<StringBuilder>(() => new StringBuilder());
			StringBuilder obj1 = pool1.Rent();

			PoolHandle<StringBuilder> handle1 = new PoolHandle<StringBuilder>(pool1, obj1);
			PoolHandle<StringBuilder> handle2 = new PoolHandle<StringBuilder>(null, obj1);
			PoolHandle<StringBuilder> handle3 = new PoolHandle<StringBuilder>(pool1, null);

			TestEquality((a, b) => a == b, (a, b) => a != b, handle1, handle2, handle3);
		}

		[Test]
		public void DifferentObject_Fail()
		{
			AssertNotEqualObject<PoolHandle<StringBuilder>>();
		}
	}
}
#endif // DISABLE_GAMEJOLT