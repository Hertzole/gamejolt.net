#nullable enable

using System;

namespace Hertzole.GameJolt
{
	/// <summary>
	///     A Game Jolt score from a <see cref="GameJoltTable">score table</see>.
	/// </summary>
	public readonly struct GameJoltScore : IEquatable<GameJoltScore>
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

		public bool Equals(GameJoltScore other)
		{
			return Sort == other.Sort && UserId == other.UserId && Stored.Equals(other.Stored) &&
			       EqualityHelper.StringEquals(Score, other.Score) &&
			       EqualityHelper.StringEquals(ExtraData, other.ExtraData) &&
			       EqualityHelper.StringEquals(Username, other.Username) &&
			       EqualityHelper.StringEquals(GuestName, other.GuestName);
		}

		public override bool Equals(object? obj)
		{
			return obj is GameJoltScore other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = Sort;
				hashCode = (hashCode * 397) ^ UserId.GetHashCode();
				hashCode = (hashCode * 397) ^ Stored.GetHashCode();
				hashCode = (hashCode * 397) ^ Score.GetHashCode();
				hashCode = (hashCode * 397) ^ (!string.IsNullOrEmpty(ExtraData) ? ExtraData.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (!string.IsNullOrEmpty(Username) ? Username.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (!string.IsNullOrEmpty(GuestName) ? GuestName.GetHashCode() : 0);
				return hashCode;
			}
		}

		public static bool operator ==(GameJoltScore left, GameJoltScore right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(GameJoltScore left, GameJoltScore right)
		{
			return !left.Equals(right);
		}

		public override string ToString()
		{
			return
				$"{nameof(GameJoltScore)} ({nameof(Sort)}: {Sort}, {nameof(Score)}: {Score}, {nameof(ExtraData)}: {ExtraData}, {nameof(Username)}: {Username}, {nameof(UserId)}: {UserId}, {nameof(GuestName)}: {GuestName}, {nameof(Stored)}: {Stored})";
		}
	}
}