#nullable enable

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER || UNITY_2021_3_OR_NEWER
using GameJoltResultTask = System.Threading.Tasks.ValueTask<Hertzole.GameJolt.GameJoltResult>;
using GameJoltScoreArrayTask = System.Threading.Tasks.ValueTask<Hertzole.GameJolt.GameJoltResult<Hertzole.GameJolt.GameJoltScore[]>>;
#else
using GameJoltResultTask = System.Threading.Tasks.Task<Hertzole.GameJolt.GameJoltResult>;
using GameJoltScoreArrayTask = System.Threading.Tasks.Task<Hertzole.GameJolt.GameJoltResult<Hertzole.GameJolt.GameJoltScore[]>>;
#endif
using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hertzole.GameJolt
{
	public sealed class GameJoltScores
	{
		private readonly IGameJoltWebClient webClient;
		private readonly IGameJoltSerializer serializer;
		private readonly GameJoltUsers users;

		internal GameJoltScores(IGameJoltWebClient webClient, IGameJoltSerializer serializer, GameJoltUsers users)
		{
			this.webClient = webClient;
			this.serializer = serializer;
			this.users = users;
		}

		internal const string ENDPOINT = "scores/";
		internal const string ADD_ENDPOINT = ENDPOINT + "add/";
		internal const string GET_RANK_ENDPOINT = ENDPOINT + "get-rank/";
		internal const string GET_TABLES_ENDPOINT = ENDPOINT + "tables/";

		public async Task<GameJoltResult> SubmitScoreAsync(int tableId,
			int sort,
			string score,
			string extraData = "",
			CancellationToken cancellationToken = default)
		{
			if (!users.IsAuthenticatedInternal(out GameJoltResult result))
			{
				return result;
			}

			return await SubmitScoreInternalAsync(tableId, users.myUsername, users.myToken, null, sort, score, extraData, cancellationToken)
				.ConfigureAwait(false);
		}

		public async Task<GameJoltResult> SubmitScoreAsGuestAsync(int tableId,
			string guestName,
			int sort,
			string score,
			string extraData = "",
			CancellationToken cancellationToken = default)
		{
			return await SubmitScoreInternalAsync(tableId, null, null, guestName, sort, score, extraData, cancellationToken).ConfigureAwait(false);
		}

		private async GameJoltResultTask SubmitScoreInternalAsync(int? tableId,
			string? username,
			string? token,
			string? guestName,
			int sort,
			string score,
			string extraData,
			CancellationToken cancellationToken)
		{
			using (StringBuilderPool.Rent(out StringBuilder builder))
			{
				builder.Append(ADD_ENDPOINT);

				if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(token))
				{
					builder.Append("?username=");
					builder.Append(users.myUsername);
					builder.Append("&user_token=");
					builder.Append(users.myToken);
				}

				if (!string.IsNullOrEmpty(guestName))
				{
					builder.Append("?guest=");
					builder.Append(guestName);
				}

				builder.Append("&score=");
				builder.Append(score);
				builder.Append("&sort=");
				builder.Append(sort);

				if (!string.IsNullOrEmpty(extraData))
				{
					builder.Append("&extra_data=");
					builder.Append(extraData);
				}

				if (tableId.HasValue)
				{
					builder.Append("&table_id=");
					builder.Append(tableId.Value);
				}

				string? json = await webClient.GetStringAsync(GameJoltUrlBuilder.BuildUrl(builder), cancellationToken).ConfigureAwait(false);
				SubmitScoreResponse response = serializer.Deserialize<SubmitScoreResponse>(json);

				if (response.TryGetException(out Exception? exception))
				{
					return GameJoltResult.Error(exception!);
				}

				Debug.Assert(response.Success, "Response was successful but success was false.");

				return GameJoltResult.Success();
			}
		}

		public async Task<GameJoltResult<int>> GetRankAsync(int tableId, int score, CancellationToken cancellationToken = default)
		{
			using (StringBuilderPool.Rent(out StringBuilder builder))
			{
				builder.Append(GET_RANK_ENDPOINT);
				builder.Append("?sort=");
				builder.Append(score);
				builder.Append("&table_id=");
				builder.Append(tableId);

				string? json = await webClient.GetStringAsync(GameJoltUrlBuilder.BuildUrl(builder), cancellationToken).ConfigureAwait(false);
				GetScoreRankResponse response = serializer.Deserialize<GetScoreRankResponse>(json);

				if (response.TryGetException(out Exception? exception))
				{
					return GameJoltResult<int>.Error(exception!);
				}

				Debug.Assert(response.success, "Response was successful but success was false.");

				return GameJoltResult<int>.Success(response.rank);
			}
		}

		public async Task<GameJoltResult<GameJoltTable[]>> GetTablesAsync(CancellationToken cancellationToken = default)
		{
			string? json = await webClient.GetStringAsync(GameJoltUrlBuilder.BASE_URL + GET_TABLES_ENDPOINT, cancellationToken).ConfigureAwait(false);
			GetTablesResponse response = serializer.Deserialize<GetTablesResponse>(json);

			if (response.TryGetException(out Exception? exception))
			{
				return GameJoltResult<GameJoltTable[]>.Error(exception!);
			}

			Debug.Assert(response.success, "Response was successful but success was false.");

			GameJoltTable[] tables = new GameJoltTable[response.tables.Length];

			for (int i = 0; i < response.tables.Length; i++)
			{
				tables[i] = response.tables[i].ToPublicTable();
			}

			return GameJoltResult<GameJoltTable[]>.Success(tables);
		}

		public GetScoresQuery QueryScores()
		{
			return new GetScoresQuery(this);
		}

		internal async GameJoltScoreArrayTask GetScoresAsync(GetScoresQuery query, CancellationToken cancellationToken)
		{
			using (StringBuilderPool.Rent(out StringBuilder builder))
			{
				builder.Append(ENDPOINT);
				if (query.tableId.HasValue)
				{
					builder.Append("?table_id=");
					builder.Append(query.tableId.Value);
				}

				builder.Append("&limit=");
				builder.Append(query.limit);

				if (!string.IsNullOrEmpty(query.username))
				{
					builder.Append("&username=");
					builder.Append(query.username);
					builder.Append("&user_token=");
					builder.Append(query.userToken);
				}
				else if (!string.IsNullOrEmpty(query.guest))
				{
					builder.Append("&guest=");
					builder.Append(query.guest);
				}

				if (query.betterThan != null)
				{
					builder.Append("&better_than=");
					builder.Append(query.betterThan.Value);
				}

				if (query.worseThan != null)
				{
					builder.Append("&worse_than=");
					builder.Append(query.worseThan.Value);
				}

				string? json = await webClient.GetStringAsync(GameJoltUrlBuilder.BuildUrl(builder), cancellationToken).ConfigureAwait(false);
				GetScoresResponse response = serializer.Deserialize<GetScoresResponse>(json);

				if (response.TryGetException(out Exception? exception))
				{
					return GameJoltResult<GameJoltScore[]>.Error(exception!);
				}

				Debug.Assert(response.success, "Response was successful but success was false.");

				GameJoltScore[] scores = new GameJoltScore[response.scores.Length];

				for (int i = 0; i < response.scores.Length; i++)
				{
					scores[i] = response.scores[i].ToPublicScore();
				}

				return GameJoltResult<GameJoltScore[]>.Success(scores);
			}
		}
	}
}