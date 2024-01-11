#nullable enable

using System;

namespace Hertzole.GameJolt
{
	/// <summary>
	///     A Game Jolt score from a <see cref="GameJoltTable">score table</see>.
	/// </summary>
	public readonly struct GameJoltScore
	{
		/// <summary>
		///     The score's numerical sort value.
		/// </summary>
		public int Sort { get; }
		/// <summary>
		///     The score string.
		/// </summary>
		public string Score { get; }
		/// <summary>
		///     Any extra data associated with the score.
		/// </summary>
		public string? ExtraData { get; }
		/// <summary>
		///     <para>If this is a user score, this is the display name of the user. Otherwise, this is null.</para>
		///     <para>
		///         Use <see cref="DisplayName" /> if you want to get the display name regardless of whether it's a user score or
		///         a guest score.
		///     </para>
		/// </summary>
		public string? Username { get; }
		/// <summary>
		///     If this is a user score, this is the user's ID. Otherwise, this is null.
		/// </summary>
		public int? UserId { get; }
		/// <summary>
		///     <para>If this is a guest score, this is the guest's name. Otherwise, this is null.</para>
		///     <para>
		///         Use <see cref="DisplayName" /> if you want to get the display name regardless of whether it's a user score or
		///         a guest score.
		///     </para>
		/// </summary>
		public string? GuestName { get; }
		/// <summary>
		///     The date the score was stored.
		/// </summary>
		public DateTime Stored { get; }

		/// <summary>
		///     The display name of the score. If this is a user score, this is the user's username. If this is a guest score, this
		///     is the guest's name.
		/// </summary>
		public string DisplayName
		{
			get
			{
				if (!string.IsNullOrEmpty(Username))
				{
					return Username!;
				}

				return !string.IsNullOrEmpty(GuestName) ? GuestName! : string.Empty;
			}
		}

		internal GameJoltScore(int sort, string score, string? extraData, string? username, int? userId, string? guestName, DateTime stored)
		{
			Sort = sort;
			Score = score;
			ExtraData = extraData;
			Username = username;
			UserId = userId;
			GuestName = guestName;
			Stored = stored;
		}
	}
}