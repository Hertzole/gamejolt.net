#nullable enable

using System.Threading;
using System.Threading.Tasks;

namespace Hertzole.GameJolt
{
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

		private GetScoresQuery(GameJoltScores scores, int? tableId, int limit, string? username, string? userToken, string? guest, int? betterThan, int? worseThan)
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
		
		public GetScoresQuery ForTable(int tableId)
		{
			return new GetScoresQuery(scores, tableId, limit, username, userToken, guest, betterThan, worseThan);
		}

		public GetScoresQuery Limit(int limit)
		{
			return new GetScoresQuery(scores, tableId, limit, username, userToken, guest, betterThan, worseThan);
		}

		public GetScoresQuery ForUser(string username, string userToken)
		{
			return new GetScoresQuery(scores, tableId, limit, username, userToken, null, betterThan, worseThan);
		}

		public GetScoresQuery ForGuest(string guest)
		{
			return new GetScoresQuery(scores, tableId, limit, null, null, guest, betterThan, worseThan);
		}

		public GetScoresQuery BetterThan(int score)
		{
			return new GetScoresQuery(scores, tableId, limit, username, userToken, guest, score, worseThan);
		}

		public GetScoresQuery WorseThan(int score)
		{
			return new GetScoresQuery(scores, tableId, limit, username, userToken, guest, betterThan, score);
		}

		public async Task<GameJoltResult<GameJoltScore[]>> GetAsync(CancellationToken cancellationToken = default)
		{
			return await scores.GetScoresAsync(this, cancellationToken).ConfigureAwait(false);
		}
	}
}