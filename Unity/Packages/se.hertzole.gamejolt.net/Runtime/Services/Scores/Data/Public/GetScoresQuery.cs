#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

#nullable enable

using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hertzole.GameJolt
{
	/// <summary>
	///     A query to get scores from a <see cref="GameJoltTable">score table</see>.
	/// </summary>
	public readonly struct GetScoresQuery : IEquatable<GetScoresQuery>
	{
		private readonly GameJoltScores? scores;
		internal readonly int? tableId;
		internal readonly int limit;
		internal readonly string? username;
		internal readonly string? userToken;
		internal readonly string? guest;
		internal readonly int? betterThan;
		internal readonly int? worseThan;

		internal GetScoresQuery(GameJoltScores? scores)
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

		internal GetScoresQuery(GameJoltScores? scores,
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

		public bool Equals(GetScoresQuery other)
		{
			return tableId == other.tableId && limit == other.limit && betterThan == other.betterThan && worseThan == other.worseThan &&
			       EqualityHelper.StringEquals(username, other.username) &&
			       EqualityHelper.StringEquals(userToken, other.userToken) &&
			       EqualityHelper.StringEquals(guest, other.guest) &&
			       ReferenceEquals(scores, other.scores);
		}

		public override bool Equals(object? obj)
		{
			return obj is GetScoresQuery other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = tableId.GetHashCode();
				hashCode = (hashCode * 397) ^ limit;
				hashCode = (hashCode * 397) ^ betterThan.GetHashCode();
				hashCode = (hashCode * 397) ^ worseThan.GetHashCode();
				hashCode = (hashCode * 397) ^ (!string.IsNullOrEmpty(username) ? username!.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (!string.IsNullOrEmpty(userToken) ? userToken!.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (!string.IsNullOrEmpty(guest) ? guest!.GetHashCode() : 0);
				return hashCode;
			}
		}

		public static bool operator ==(GetScoresQuery left, GetScoresQuery right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(GetScoresQuery left, GetScoresQuery right)
		{
			return !left.Equals(right);
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
			return await scores!.GetScoresAsync(this, cancellationToken);
		}

		public override string ToString()
		{
			using (StringBuilderPool.Rent(out StringBuilder? builder))
			{
				builder.Append(nameof(GetScoresQuery) + " (" + nameof(limit) + ": ");
				builder.Append(limit);

				if (tableId.HasValue)
				{
					builder.Append(", " + nameof(tableId) + ": ");
					builder.Append(tableId.Value);
				}

				if (!string.IsNullOrEmpty(username))
				{
					builder.Append(", " + nameof(username) + ": ");
					builder.Append(username);
				}

				if (!string.IsNullOrEmpty(userToken))
				{
					builder.Append(", " + nameof(userToken) + ": ");
					builder.Append(userToken);
				}

				if (!string.IsNullOrEmpty(guest))
				{
					builder.Append(", " + nameof(guest) + ": ");
					builder.Append(guest);
				}

				if (betterThan.HasValue)
				{
					builder.Append(", " + nameof(betterThan) + ": ");
					builder.Append(betterThan.Value);
				}

				if (worseThan.HasValue)
				{
					builder.Append(", " + nameof(worseThan) + ": ");
					builder.Append(worseThan.Value);
				}

				builder.Append(')');

				return builder.ToString();
			}
		}
	}
}
#endif // DISABLE_GAMEJOLT