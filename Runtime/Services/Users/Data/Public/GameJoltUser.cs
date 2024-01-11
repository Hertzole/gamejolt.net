#nullable enable

using System;

namespace Hertzole.GameJolt
{
	/// <summary>
	///     A Game Jolt user.
	/// </summary>
	public readonly struct GameJoltUser
	{
		/// <summary>
		///     The ID of the user.
		/// </summary>
		public int Id { get; }
		/// <summary>
		///     The type of the user.
		/// </summary>
		public UserType Type { get; }
		/// <summary>
		///     The username of the user.
		/// </summary>
		public string Username { get; }
		/// <summary>
		///     The URL to the user's avatar.
		/// </summary>
		public string AvatarUrl { get; }
		/// <summary>
		///     The status of the user.
		/// </summary>
		public UserStatus Status { get; }
		/// <summary>
		///     The display name of the user.
		/// </summary>
		public string DisplayName { get; }
		/// <summary>
		///     The website of the user.
		/// </summary>
		public string? UserWebsite { get; }
		/// <summary>
		///     The description of the user.
		/// </summary>
		public string UserDescription { get; }

		/// <summary>
		///     When the user signed up.
		/// </summary>
		public DateTime SignedUp { get; }
		/// <summary>
		///     When the user last logged in.
		/// </summary>
		public DateTime LastLoggedIn { get; }
		/// <summary>
		///     Whether or not the user is online now.
		/// </summary>
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