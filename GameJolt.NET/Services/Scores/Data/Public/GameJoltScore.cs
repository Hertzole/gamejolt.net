#nullable enable

using System;

namespace Hertzole.GameJolt
{
	public readonly struct GameJoltScore
	{
		public int Sort { get; }
		public string Score { get; }
		public string? ExtraData { get; }
		public string? Username { get; }
		public int? UserId { get; }
		public string? GuestName { get; }
		public DateTime Stored { get; }

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