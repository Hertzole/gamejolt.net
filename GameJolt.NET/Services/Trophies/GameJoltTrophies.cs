#nullable enable

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hertzole.GameJolt
{
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

		public Task<GameJoltResult<GameJoltTrophy[]>> GetTrophiesAsync(CancellationToken cancellationToken = default)
		{
			return GetTrophiesInternalAsync(null, 0, null, cancellationToken);
		}

		public Task<GameJoltResult<GameJoltTrophy[]>> GetTrophiesAsync(bool getAchieved, CancellationToken cancellationToken = default)
		{
			return GetTrophiesInternalAsync(null, 0, getAchieved, cancellationToken);
		}

		public async Task<GameJoltResult<GameJoltTrophy>> GetTrophyAsync(int trophyId, CancellationToken cancellationToken = default)
		{
			int[]? trophyIds = intPool.Rent(1);
			trophyIds[0] = trophyId;
			GameJoltResult<GameJoltTrophy[]> result = await GetTrophiesInternalAsync(trophyIds, 1, null, cancellationToken).ConfigureAwait(false);

			intPool.Return(trophyIds);

			if (result.HasError)
			{
				return GameJoltResult<GameJoltTrophy>.Error(result.Exception!);
			}

			Debug.Assert(result.Value.Length == 1, "Result length was not 1.");

			return GameJoltResult<GameJoltTrophy>.Success(result.Value[0]);
		}

		public Task<GameJoltResult<GameJoltTrophy[]>> GetTrophiesAsync(IEnumerable<int> trophyIds, CancellationToken cancellationToken = default)
		{
			return GetTrophiesInternalAsync(trophyIds, -1, null, cancellationToken);
		}

		private async Task<GameJoltResult<GameJoltTrophy[]>> GetTrophiesInternalAsync(IEnumerable<int>? trophyIds,
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

				string json = await webClient.GetStringAsync(builder.ToString(), cancellationToken).ConfigureAwait(false);
				FetchTrophiesResponse response = serializer.Deserialize<FetchTrophiesResponse>(json);

				if (response.TryGetException(out Exception? exception))
				{
					return GameJoltResult<GameJoltTrophy[]>.Error(exception);
				}

				GameJoltTrophy[] trophies = response.trophies.Length > 0 ? new GameJoltTrophy[response.trophies.Length] : Array.Empty<GameJoltTrophy>();

				for (int i = 0; i < trophies.Length; i++)
				{
					trophies[i] = response.trophies[i].ToPublicTrophy();
				}

				return GameJoltResult<GameJoltTrophy[]>.Success(trophies);
			}
		}

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

				string json = await webClient.GetStringAsync(builder.ToString(), cancellationToken).ConfigureAwait(false);
				TrophyResponse response = serializer.Deserialize<TrophyResponse>(json);

				if (response.TryGetException(out Exception? exception))
				{
					return GameJoltResult.Error(exception);
				}

				return GameJoltResult.Success();
			}
		}

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

				string json = await webClient.GetStringAsync(builder.ToString(), cancellationToken).ConfigureAwait(false);
				TrophyResponse response = serializer.Deserialize<TrophyResponse>(json);

				if (response.TryGetException(out Exception? exception))
				{
					return GameJoltResult.Error(exception);
				}

				return GameJoltResult.Success();
			}
		}
	}
}