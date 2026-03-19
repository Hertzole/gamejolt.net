#if !DISABLE_GAMEJOLT
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hertzole.GameJolt
{
	/// <summary>
	///     Extensions for <see cref="GameJoltTrophies" /> to provide additional overloads.
	/// </summary>
	public static class GameJoltTrophiesExtensions
	{
		/// <inheritdoc cref="GameJoltTrophies.GetTrophiesAsync(ReadOnlyMemory{int}, CancellationToken)" />
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="trophyIds" /> is <see langword="null" />.</exception>
		public static async Task<GameJoltResult<GameJoltTrophy[]>> GetTrophiesAsync(this GameJoltTrophies trophies,
			IList<int> trophyIds,
			CancellationToken cancellationToken = default)
		{
			Guard.IsNotNull(trophyIds, nameof(trophyIds));

			using (MemoryOwner<int> memory = trophyIds.GetMemory())
			{
				return await trophies.GetTrophiesAsync(memory, cancellationToken);
			}
		}

		/// <inheritdoc cref="GameJoltTrophies.GetTrophiesAsync(ReadOnlyMemory{int}, IList{GameJoltTrophy}, CancellationToken)" />
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="trophyIds" /> is <see langword="null" />.</exception>
		public static async Task<GameJoltResult> GetTrophiesAsync(this GameJoltTrophies trophies,
			IList<int> trophyIds,
			IList<GameJoltTrophy> results,
			CancellationToken cancellationToken = default)
		{
			Guard.IsNotNull(trophyIds, nameof(trophyIds));

			using (MemoryOwner<int> memory = trophyIds.GetMemory())
			{
				return await trophies.GetTrophiesAsync(memory, results, cancellationToken);
			}
		}

		/// <inheritdoc cref="GameJoltTrophies.GetTrophiesAsync(ReadOnlyMemory{int}, CancellationToken)" />
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="trophyIds" /> is <see langword="null" />.</exception>
		public static async Task<GameJoltResult<GameJoltTrophy[]>> GetTrophiesAsync(this GameJoltTrophies trophies,
			IEnumerable<int> trophyIds,
			CancellationToken cancellationToken = default)
		{
			Guard.IsNotNull(trophyIds, nameof(trophyIds));

			using (MemoryOwner<int> memory = trophyIds.GetMemory())
			{
				return await trophies.GetTrophiesAsync(memory, cancellationToken);
			}
		}

		/// <inheritdoc cref="GameJoltTrophies.GetTrophiesAsync(ReadOnlyMemory{int}, IList{GameJoltTrophy}, CancellationToken)" />
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="trophyIds" /> is <see langword="null" />.</exception>
		public static async Task<GameJoltResult> GetTrophiesAsync(this GameJoltTrophies trophies,
			IEnumerable<int> trophyIds,
			IList<GameJoltTrophy> results,
			CancellationToken cancellationToken = default)
		{
			Guard.IsNotNull(trophyIds, nameof(trophyIds));

			using (MemoryOwner<int> memory = trophyIds.GetMemory())
			{
				return await trophies.GetTrophiesAsync(memory, results, cancellationToken);
			}
		}
	}
}
#endif // !DISABLE_GAMEJOLT