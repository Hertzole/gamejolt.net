#nullable enable

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER || UNITY_2021_3_OR_NEWER
using GameJoltTrophyArrayTask = System.Threading.Tasks.ValueTask<Hertzole.GameJolt.GameJoltResult<Hertzole.GameJolt.GameJoltTrophy[]>>;
#else
using GameJoltTrophyArrayTask = System.Threading.Tasks.Task<Hertzole.GameJolt.GameJoltResult<Hertzole.GameJolt.GameJoltTrophy[]>>;
#endif

namespace Hertzole.GameJolt
{
	/// <summary>
	///     Used to get and manage trophies.
	/// </summary>
	public sealed class GameJoltTrophies
	{
		private readonly IGameJoltWebClient webClient;
		private readonly IGameJoltSerializer serializer;
		private readonly GameJoltUsers users;

		private readonly ArrayPool<int> intPool;

		internal GameJoltTrophies(IGameJoltWebClient webClient, IGameJoltSerializer serializer, GameJoltUsers users)
		{
			this.webClient = webClient;
			this.serializer = serializer;
			this.users = users;

			intPool = ArrayPool<int>.Create();
		}

		internal const string ENDPOINT = "trophies/";
		internal const string ADD_ENDPOINT = ENDPOINT + "add-achieved/";
		internal const string REMOVE_ENDPOINT = ENDPOINT + "remove-achieved/";

		/// <summary>
		///     Gets all trophies for the current user. This method will get both locked and unlocked trophies. This method
		///     requires the current user to be authenticated.
		/// </summary>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the request and the trophies.</returns>
		/// <exception cref="GameJoltAuthorizedException">Returned if the user is not authenticated.</exception>
		public async Task<GameJoltResult<GameJoltTrophy[]>> GetTrophiesAsync(CancellationToken cancellationToken = default)
		{
			return await GetTrophiesInternalAsync(null, 0, null, cancellationToken);
		}

		/// <summary>
		///     Gets all trophies for the current user. This method allows you to pick whether to get locked or unlocked trophies.
		///     This method requires the current user to be authenticated.
		/// </summary>
		/// <param name="getAchieved">
		///     Pass in <c>true</c> to only get trophies that are unlocked, <c>false</c> to only get trophies
		///     that are locked.
		/// </param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the request and the trophies.</returns>
		/// <exception cref="GameJoltAuthorizedException">Returned if the user is not authenticated.</exception>
		public async Task<GameJoltResult<GameJoltTrophy[]>> GetTrophiesAsync(bool getAchieved, CancellationToken cancellationToken = default)
		{
			return await GetTrophiesInternalAsync(null, 0, getAchieved, cancellationToken);
		}

		/// <summary>
		///     Get all trophies for the current user with the specified IDs. This method requires the current user to be
		///     authenticated.
		/// </summary>
		/// <param name="trophyIds">The IDs of the trophies to get.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the request and the trophies.</returns>
		/// <exception cref="GameJoltAuthorizedException">Returned if the user is not authenticated.</exception>
		/// <exception cref="GameJoltInvalidTrophyException">Returned if any of the trophy IDs can't be found on the server.</exception>
		public async Task<GameJoltResult<GameJoltTrophy[]>> GetTrophiesAsync(IEnumerable<int> trophyIds, CancellationToken cancellationToken = default)
		{
			return await GetTrophiesInternalAsync(trophyIds, -1, null, cancellationToken);
		}

		/// <summary>
		///     Gets a trophy for the current user with the specified ID. This method requires the current user to be
		///     authenticated.
		/// </summary>
		/// <param name="trophyId"></param>
		/// <param name="cancellationToken"></param>
		/// <returns>The result of the request and the trophy.</returns>
		/// <exception cref="GameJoltAuthorizedException">Returned if the user is not authenticated.</exception>
		/// <exception cref="GameJoltInvalidTrophyException">Returned if the trophy can't be found on the server.</exception>
		public async Task<GameJoltResult<GameJoltTrophy>> GetTrophyAsync(int trophyId, CancellationToken cancellationToken = default)
		{
			int[] trophyIds = intPool.Rent(1);
			trophyIds[0] = trophyId;
			GameJoltResult<GameJoltTrophy[]> result = await GetTrophiesInternalAsync(trophyIds, 1, null, cancellationToken);

			intPool.Return(trophyIds);

			if (result.HasError)
			{
				return GameJoltResult<GameJoltTrophy>.Error(result.Exception!);
			}

			Debug.Assert(result.Value!.Length == 1, "Result length was not 1.");

			return GameJoltResult<GameJoltTrophy>.Success(result.Value[0]);
		}

		private async GameJoltTrophyArrayTask GetTrophiesInternalAsync(IEnumerable<int>? trophyIds,
			int idLength,
			bool? getAchieved,
			CancellationToken cancellationToken)
		{
			if (!users.IsAuthenticatedInternal(out GameJoltResult<GameJoltTrophy[]> result))
			{
				return result;
			}

			using (StringBuilderPool.Rent(out StringBuilder builder))
			{
				builder.Append(ENDPOINT);
				builder.Append("?username=");
				builder.Append(users.myUsername);
				builder.Append("&user_token=");
				builder.Append(users.myToken);
				if (getAchieved.HasValue)
				{
					builder.Append("&achieved=");
					builder.Append(getAchieved.Value ? "true" : "false");
				}

				if (trophyIds != null)
				{
					bool addComma = false;

					builder.Append("&trophy_id=");
					int i = 0;

					foreach (int trophyId in trophyIds)
					{
						if (addComma)
						{
							builder.Append(',');
						}
						else
						{
							addComma = true;
						}

						builder.Append(trophyId);
						i++;

						if (i >= idLength && idLength >= 0)
						{
							break;
						}
					}
				}

				string json = await webClient.GetStringAsync(GameJoltUrlBuilder.BuildUrl(builder), cancellationToken);
				FetchTrophiesResponse response = serializer.Deserialize<FetchTrophiesResponse>(json);

				if (response.TryGetException(out Exception? exception))
				{
					return GameJoltResult<GameJoltTrophy[]>.Error(exception!);
				}

				GameJoltTrophy[] trophies = response.trophies.Length > 0 ? new GameJoltTrophy[response.trophies.Length] : Array.Empty<GameJoltTrophy>();

				for (int i = 0; i < trophies.Length; i++)
				{
					trophies[i] = response.trophies[i].ToPublicTrophy();
				}

				return GameJoltResult<GameJoltTrophy[]>.Success(trophies);
			}
		}

		/// <summary>
		///     Unlocks a trophy for the current user. This method requires the current user to be authenticated.
		/// </summary>
		/// <param name="trophyId">The ID of the trophy to unlock.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the request.</returns>
		/// <exception cref="GameJoltAuthorizedException">Returned if the user is not authenticated.</exception>
		/// <exception cref="GameJoltInvalidTrophyException">Returned if the trophy can't be found on the server.</exception>
		public async Task<GameJoltResult> UnlockTrophyAsync(int trophyId, CancellationToken cancellationToken = default)
		{
			if (!users.IsAuthenticatedInternal(out GameJoltResult result))
			{
				return result;
			}

			using (StringBuilderPool.Rent(out StringBuilder builder))
			{
				builder.Append(ADD_ENDPOINT);
				builder.Append("?username=");
				builder.Append(users.myUsername);
				builder.Append("&user_token=");
				builder.Append(users.myToken);
				builder.Append("&trophy_id=");
				builder.Append(trophyId);

				string json = await webClient.GetStringAsync(GameJoltUrlBuilder.BuildUrl(builder), cancellationToken);
				TrophyResponse response = serializer.Deserialize<TrophyResponse>(json);

				if (response.TryGetException(out Exception? exception))
				{
					return GameJoltResult.Error(exception!);
				}

				return GameJoltResult.Success();
			}
		}

		/// <summary>
		///     Removes an unlocked trophy for the current user. This method requires the current user to be authenticated.
		/// </summary>
		/// <param name="trophyId">The ID of the trophy to remove.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the request.</returns>
		/// <exception cref="GameJoltAuthorizedException">Returned if the user is not authenticated.</exception>
		/// <exception cref="GameJoltInvalidTrophyException">Returned if the trophy can't be found on the server.</exception>
		public async Task<GameJoltResult> RemoveUnlockedTrophyAsync(int trophyId, CancellationToken cancellationToken = default)
		{
			if (!users.IsAuthenticatedInternal(out GameJoltResult result))
			{
				return result;
			}

			using (StringBuilderPool.Rent(out StringBuilder builder))
			{
				builder.Append(REMOVE_ENDPOINT);
				builder.Append("?username=");
				builder.Append(users.myUsername);
				builder.Append("&user_token=");
				builder.Append(users.myToken);
				builder.Append("&trophy_id=");
				builder.Append(trophyId);

				string json = await webClient.GetStringAsync(GameJoltUrlBuilder.BuildUrl(builder), cancellationToken);
				TrophyResponse response = serializer.Deserialize<TrophyResponse>(json);

				if (response.TryGetException(out Exception? exception))
				{
					return GameJoltResult.Error(exception!);
				}

				return GameJoltResult.Success();
			}
		}
	}
}