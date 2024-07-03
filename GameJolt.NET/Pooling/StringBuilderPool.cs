#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

using System.Text;

namespace Hertzole.GameJolt
{
	internal static class StringBuilderPool
	{
		private static readonly ObjectPool<StringBuilder> pool = new ObjectPool<StringBuilder>(() => new StringBuilder(256), builder => builder.Clear());

		public static PoolHandle<StringBuilder> Rent(out StringBuilder builder)
		{
			return pool.Rent(out builder);
		}
	}
}
#endif // DISABLE_GAMEJOLT