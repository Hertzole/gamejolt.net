#nullable enable

using System;

namespace Hertzole.GameJolt
{
	public readonly struct GameJoltUser
	{
		public int Id { get; }
		public UserType Type { get; }
		public string Username { get; }
		public string AvatarUrl { get; }
		public UserStatus Status { get; }
		public string DisplayName { get; }
		public string? UserWebsite { get; }
		public string UserDescription { get; }

		public DateTime SignedUp { get; }
		public DateTime LastLoggedIn { get; }
		public bool OnlineNow { get; }

		internal GameJoltUser(int id,
			UserType type,
			string username,
			string avatarUrl,
			UserStatus status,
			string displayName,
			string? userWebsite,
			string userDescription,
			DateTime signedUp,
			DateTime lastLoggedIn,
			bool onlineNow)
		{
			Id = id;
			Type = type;
			Username = username;
			AvatarUrl = avatarUrl;
			Status = status;
			DisplayName = displayName;
			UserWebsite = userWebsite;
			UserDescription = userDescription;
			SignedUp = signedUp;
			LastLoggedIn = lastLoggedIn;
			OnlineNow = onlineNow;
		}
	}
}