#nullable enable

using System.Threading;
using System.Threading.Tasks;

namespace Hertzole.GameJolt
{
	/// <summary>
	///     A query to get scores from a <see cref="GameJoltTable">score table</see>.
	/// </summary>
	public readonly struct GetScoresQuery
	{
		private readonly GameJoltScores scores;
		internal readonly int? tableId;
		internal readonly int limit;
		internal readonly string? username;
		internal readonly string? userToken;
		internal readonly string? guest;
		internal readonly int? betterThan;
		internal readonly int? worseThan;

		internal GetScoresQuery(GameJoltScores scores)
		{
			this.scores = scores;
			tableId = null;
			limit = 10;
			username = null;
			userToken = null;
			guest = null;
			betterThan = null;
			worseThan = null;
		}

		private GetScoresQuery(GameJoltScores scores,
			int? tableId,
			int limit,
			string? username,
			string? userToken,
			string? guest,
			int? betterThan,
			int? worseThan)
		{
			this.scores = scores;
			this.tableId = tableId;
			this.limit = limit;
			this.username = username;
			this.userToken = userToken;
			this.guest = guest;
			this.betterThan = betterThan;
			this.worseThan = worseThan;
		}

		/// <summary>
		///     Gets scores for a specific table. Default is the primary score table.
		/// </summary>
		/// <param name="tableId">The ID of the score table.</param>
		/// <returns>The query.</returns>
		public GetScoresQuery ForTable(int tableId)
		{
			return new GetScoresQuery(scores, tableId, limit, username, userToken, guest, betterThan, worseThan);
		}

		/// <summary>
		///     The number of scores to return. Default is 10. The maximum is 100.
		/// </summary>
		/// <param name="limit">The number of scores you'd like to return.</param>
		/// <returns>The query.</returns>
		public GetScoresQuery Limit(int limit)
		{
			return new GetScoresQuery(scores, tableId, limit, username, userToken, guest, betterThan, worseThan);
		}

		/// <summary>
		///     Only return scores for the current user.
		/// </summary>
		/// <returns>The query.</returns>
		public GetScoresQuery ForCurrentUser()
		{
			return new GetScoresQuery(scores, tableId, limit, GameJoltAPI.Users.myUsername, GameJoltAPI.Users.myToken, null, betterThan, worseThan);
		}

		/// <summary>
		///     Only return scores for a specific guest.
		/// </summary>
		/// <param name="guestName">A guest's name</param>
		/// <returns>The query.</returns>
		public GetScoresQuery ForGuest(string guestName)
		{
			return new GetScoresQuery(scores, tableId, limit, null, null, guestName, betterThan, worseThan);
		}

		/// <summary>
		///     Only return scores better than the specified score.
		/// </summary>
		/// <param name="score">Fetch only scores better than this score sort value.</param>
		/// <returns>The query.</returns>
		public GetScoresQuery BetterThan(int score)
		{
			return new GetScoresQuery(scores, tableId, limit, username, userToken, guest, score, worseThan);
		}

		/// <summary>
		///     Only return scores worse than the specified score.
		/// </summary>
		/// <param name="score">Fetch only scores worse than this score sort value.</param>
		/// <returns>The query.</returns>
		public GetScoresQuery WorseThan(int score)
		{
			return new GetScoresQuery(scores, tableId, limit, username, userToken, guest, betterThan, score);
		}

		/// <summary>
		///     Gets the scores from the specified query.
		/// </summary>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the request and a list of scores.</returns>
		public async Task<GameJoltResult<GameJoltScore[]>> GetAsync(CancellationToken cancellationToken = default)
		{
			return await scores.GetScoresAsync(this, cancellationToken);
		}
	}
}