#nullable enable

using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER || UNITY_2021_3_OR_NEWER
using GameJoltResultTask = System.Threading.Tasks.ValueTask<Hertzole.GameJolt.GameJoltResult>;
using GameJoltScoreArrayTask = System.Threading.Tasks.ValueTask<Hertzole.GameJolt.GameJoltResult<Hertzole.GameJolt.GameJoltScore[]>>;

#else
using GameJoltResultTask = System.Threading.Tasks.Task<Hertzole.GameJolt.GameJoltResult>;
using GameJoltScoreArrayTask = System.Threading.Tasks.Task<Hertzole.GameJolt.GameJoltResult<Hertzole.GameJolt.GameJoltScore[]>>;
#endif

namespace Hertzole.GameJolt
{
	/// <summary>
	///     Used to submit, get and manage scores.
	/// </summary>
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

		/// <summary>
		///     Submits a score for the current user. This method requires the current user to be authenticated.
		/// </summary>
		/// <param name="tableId">The ID of the score table to submit to.</param>
		/// <param name="sort">The numerical sorting value associated with the score. All sorting will be based on this number.</param>
		/// <param name="score">This is a string value associated with the score.</param>
		/// <param name="extraData">If there's any extra data you would like to store as a string, you can use this field.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the request.</returns>
		/// <exception cref="GameJoltAuthorizedException">Returned if the user is not authenticated.</exception>
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

			return await SubmitScoreInternalAsync(tableId, users.myUsername, users.myToken, null, sort, score, extraData, cancellationToken);
		}

		/// <summary>
		///     Submits a score for the current user. This method requires the current user to be authenticated.
		/// </summary>
		/// <param name="tableId">The ID of the score table to submit to.</param>
		/// <param name="guestName">The name of the guest to submit the score for.</param>
		/// <param name="sort">The numerical sorting value associated with the score. All sorting will be based on this number.</param>
		/// <param name="score">This is a string value associated with the score.</param>
		/// <param name="extraData">If there's any extra data you would like to store as a string, you can use this field.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the request.</returns>
		public async Task<GameJoltResult> SubmitScoreAsGuestAsync(int tableId,
			string guestName,
			int sort,
			string score,
			string extraData = "",
			CancellationToken cancellationToken = default)
		{
			return await SubmitScoreInternalAsync(tableId, null, null, guestName, sort, score, extraData, cancellationToken);
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

				string? json = await webClient.GetStringAsync(GameJoltUrlBuilder.BuildUrl(builder), cancellationToken);
				SubmitScoreResponse response = serializer.Deserialize<SubmitScoreResponse>(json);

				if (response.TryGetException(out Exception? exception))
				{
					return GameJoltResult.Error(exception!);
				}

				Debug.Assert(response.Success, "Response was successful but success was false.");

				return GameJoltResult.Success();
			}
		}

		/// <summary>
		///     Returns the rank of a score in a score table.
		/// </summary>
		/// <param name="tableId">The ID of the score table from which you want to get the rank.</param>
		/// <param name="score">This is a numerical sorting value that is represented by a rank on the score table.</param>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the request and the rank of the score.</returns>
		public async Task<GameJoltResult<int>> GetRankAsync(int tableId, int score, CancellationToken cancellationToken = default)
		{
			using (StringBuilderPool.Rent(out StringBuilder builder))
			{
				builder.Append(GET_RANK_ENDPOINT);
				builder.Append("?sort=");
				builder.Append(score);
				builder.Append("&table_id=");
				builder.Append(tableId);

				string? json = await webClient.GetStringAsync(GameJoltUrlBuilder.BuildUrl(builder), cancellationToken);
				GetScoreRankResponse response = serializer.Deserialize<GetScoreRankResponse>(json);

				if (response.TryGetException(out Exception? exception))
				{
					return GameJoltResult<int>.Error(exception!);
				}

				Debug.Assert(response.success, "Response was successful but success was false.");

				return GameJoltResult<int>.Success(response.rank);
			}
		}

		/// <summary>
		///     Returns a list of all the score tables for your game.
		/// </summary>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the request and a list of score tables.</returns>
		public async Task<GameJoltResult<GameJoltTable[]>> GetTablesAsync(CancellationToken cancellationToken = default)
		{
			using (StringBuilderPool.Rent(out StringBuilder builder))
			{
				builder.Append(GET_TABLES_ENDPOINT);

				string? json = await webClient.GetStringAsync(GameJoltUrlBuilder.BuildUrl(builder), cancellationToken);
				GetTablesResponse response = serializer.Deserialize<GetTablesResponse>(json);

				if (response.TryGetException(out Exception? exception))
				{
					return GameJoltResult<GameJoltTable[]>.Error(exception!);
				}

				Debug.Assert(response.success, "Response was successful but success was false.");

				if (response.tables == null)
				{
					return GameJoltResult<GameJoltTable[]>.Error(new NullReferenceException("Tables array was null. This is a bug!"));
				}

				GameJoltTable[] tables = new GameJoltTable[response.tables.Length];

				for (int i = 0; i < response.tables.Length; i++)
				{
					tables[i] = response.tables[i].ToPublicTable();
				}

				return GameJoltResult<GameJoltTable[]>.Success(tables);
			}
		}

		/// <summary>
		///     Used to get scores from a score table.
		/// </summary>
		/// <returns>A query where you can set the parameters for the request.</returns>
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

				string? json = await webClient.GetStringAsync(GameJoltUrlBuilder.BuildUrl(builder), cancellationToken);
				GetScoresResponse response = serializer.Deserialize<GetScoresResponse>(json);

				if (response.TryGetException(out Exception? exception))
				{
					return GameJoltResult<GameJoltScore[]>.Error(exception!);
				}

				Debug.Assert(response.success, "Response was successful but success was false.");

				if (response.scores == null)
				{
					return GameJoltResult<GameJoltScore[]>.Error(new NullReferenceException("Scores array was null. This is a bug!"));
				}

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