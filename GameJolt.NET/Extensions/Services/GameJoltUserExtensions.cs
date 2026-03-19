#if !DISABLE_GAMEJOLT
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hertzole.GameJolt
{
	/// <summary>
	///     Extensions for <see cref="GameJoltUsers" /> to provide additional overloads.
	/// </summary>
	public static class GameJoltUserExtensions
	{
		/// <inheritdoc cref="GameJoltUsers.AuthenticateFromCredentialsFileAsync(ReadOnlyMemory{string}, CancellationToken)" />
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="lines" /> is <see langword="null" />.</exception>
		public static async Task<GameJoltResult> AuthenticateFromCredentialsFileAsync(this GameJoltUsers users,
			IList<string> lines,
			CancellationToken cancellationToken = default)
		{
			Guard.IsNotNull(lines, nameof(lines));

			using (MemoryOwner<string> memory = lines.GetMemory())
			{
				return await users.AuthenticateFromCredentialsFileAsync(memory, cancellationToken);
			}
		}

		/// <inheritdoc cref="GameJoltUsers.AuthenticateFromCredentialsFileAsync(ReadOnlyMemory{string}, CancellationToken)" />
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="lines" /> is <see langword="null" />.</exception>
		public static async Task<GameJoltResult> AuthenticateFromCredentialsFileAsync(this GameJoltUsers users,
			IEnumerable<string> lines,
			CancellationToken cancellationToken = default)
		{
			Guard.IsNotNull(lines, nameof(lines));

			using (MemoryOwner<string> memory = lines.GetMemory())
			{
				return await users.AuthenticateFromCredentialsFileAsync(memory, cancellationToken);
			}
		}

		/// <inheritdoc cref="GameJoltUsers.GetUsersAsync(ReadOnlyMemory{string}, CancellationToken)" />
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="usernames" /> is <see langword="null" />.</exception>
		public static async Task<GameJoltResult<GameJoltUser[]>> GetUsersAsync(this GameJoltUsers users,
			IList<string> usernames,
			CancellationToken cancellationToken = default)
		{
			Guard.IsNotNull(usernames, nameof(usernames));

			using (MemoryOwner<string> memory = usernames.GetMemory())
			{
				return await users.GetUsersAsync(memory, cancellationToken);
			}
		}

		/// <inheritdoc cref="GameJoltUsers.GetUsersAsync(ReadOnlyMemory{string}, IList{GameJoltUser}, CancellationToken)" />
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="usernames" /> is <see langword="null" />.</exception>
		public static async Task<GameJoltResult> GetUsersAsync(this GameJoltUsers users,
			IList<string> usernames,
			IList<GameJoltUser> results,
			CancellationToken cancellationToken = default)
		{
			Guard.IsNotNull(usernames, nameof(usernames));

			using (MemoryOwner<string> memory = usernames.GetMemory())
			{
				return await users.GetUsersAsync(memory, results, cancellationToken);
			}
		}

		/// <inheritdoc cref="GameJoltUsers.GetUsersAsync(ReadOnlyMemory{string}, CancellationToken)" />
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="usernames" /> is <see langword="null" />.</exception>
		public static async Task<GameJoltResult<GameJoltUser[]>> GetUsersAsync(this GameJoltUsers users,
			IEnumerable<string> usernames,
			CancellationToken cancellationToken = default)
		{
			Guard.IsNotNull(usernames, nameof(usernames));

			using (MemoryOwner<string> memory = usernames.GetMemory())
			{
				return await users.GetUsersAsync(memory, cancellationToken);
			}
		}

		/// <inheritdoc cref="GameJoltUsers.GetUsersAsync(ReadOnlyMemory{string}, IList{GameJoltUser}, CancellationToken)" />
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="usernames" /> is <see langword="null" />.</exception>
		public static async Task<GameJoltResult> GetUsersAsync(this GameJoltUsers users,
			IEnumerable<string> usernames,
			IList<GameJoltUser> results,
			CancellationToken cancellationToken = default)
		{
			Guard.IsNotNull(usernames, nameof(usernames));

			using (MemoryOwner<string> memory = usernames.GetMemory())
			{
				return await users.GetUsersAsync(memory, results, cancellationToken);
			}
		}

		/// <inheritdoc cref="GameJoltUsers.GetUsersAsync(ReadOnlyMemory{string}, CancellationToken)" />
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="userIds" /> is <see langword="null" />.</exception>
		public static async Task<GameJoltResult<GameJoltUser[]>> GetUsersAsync(this GameJoltUsers users,
			IList<int> userIds,
			CancellationToken cancellationToken = default)
		{
			Guard.IsNotNull(userIds, nameof(userIds));

			using (MemoryOwner<int> memory = userIds.GetMemory())
			{
				return await users.GetUsersAsync(memory, cancellationToken);
			}
		}

		/// <inheritdoc cref="GameJoltUsers.GetUsersAsync(ReadOnlyMemory{string}, IList{GameJoltUser}, CancellationToken)" />
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="userIds" /> is <see langword="null" />.</exception>
		public static async Task<GameJoltResult> GetUsersAsync(this GameJoltUsers users,
			IList<int> userIds,
			IList<GameJoltUser> results,
			CancellationToken cancellationToken = default)
		{
			Guard.IsNotNull(userIds, nameof(userIds));

			using (MemoryOwner<int> memory = userIds.GetMemory())
			{
				return await users.GetUsersAsync(memory, results, cancellationToken);
			}
		}

		/// <inheritdoc cref="GameJoltUsers.GetUsersAsync(ReadOnlyMemory{string}, CancellationToken)" />
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="userIds" /> is <see langword="null" />.</exception>
		public static async Task<GameJoltResult<GameJoltUser[]>> GetUsersAsync(this GameJoltUsers users,
			IEnumerable<int> userIds,
			CancellationToken cancellationToken = default)
		{
			Guard.IsNotNull(userIds, nameof(userIds));

			using (MemoryOwner<int> memory = userIds.GetMemory())
			{
				return await users.GetUsersAsync(memory, cancellationToken);
			}
		}

		/// <inheritdoc cref="GameJoltUsers.GetUsersAsync(ReadOnlyMemory{string}, IList{GameJoltUser}, CancellationToken)" />
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="userIds" /> is <see langword="null" />.</exception>
		public static async Task<GameJoltResult> GetUsersAsync(this GameJoltUsers users,
			IEnumerable<int> userIds,
			IList<GameJoltUser> results,
			CancellationToken cancellationToken = default)
		{
			Guard.IsNotNull(userIds, nameof(userIds));

			using (MemoryOwner<int> memory = userIds.GetMemory())
			{
				return await users.GetUsersAsync(memory, results, cancellationToken);
			}
		}
	}
}
#endif // !DISABLE_GAMEJOLT