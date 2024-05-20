#nullable enable

using System;
using System.Text;

namespace Hertzole.GameJolt
{
	/// <summary>
	///     A Game Jolt user.
	/// </summary>
	public readonly struct GameJoltUser : IEquatable<GameJoltUser>
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

		public bool Equals(GameJoltUser other)
		{
			return Id == other.Id && Type == other.Type && Status == other.Status && SignedUp.Equals(other.SignedUp) &&
			       LastLoggedIn.Equals(other.LastLoggedIn) && OnlineNow == other.OnlineNow &&
			       EqualityHelper.StringEquals(Username, other.Username) &&
			       EqualityHelper.StringEquals(AvatarUrl, other.AvatarUrl) &&
			       EqualityHelper.StringEquals(DisplayName, other.DisplayName) &&
			       EqualityHelper.StringEquals(UserWebsite, other.UserWebsite) &&
			       EqualityHelper.StringEquals(UserDescription, other.UserDescription);
		}

		public override bool Equals(object? obj)
		{
			return obj is GameJoltUser other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = Id;
				hashCode = (hashCode * 397) ^ (int) Type;
				hashCode = (hashCode * 397) ^ (int) Status;
				hashCode = (hashCode * 397) ^ SignedUp.GetHashCode();
				hashCode = (hashCode * 397) ^ LastLoggedIn.GetHashCode();
				hashCode = (hashCode * 397) ^ OnlineNow.GetHashCode();
				hashCode = (hashCode * 397) ^ Username.GetHashCode();
				hashCode = (hashCode * 397) ^ AvatarUrl.GetHashCode();
				hashCode = (hashCode * 397) ^ DisplayName.GetHashCode();
				hashCode = (hashCode * 397) ^ (!string.IsNullOrEmpty(UserWebsite) ? UserWebsite!.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ UserDescription.GetHashCode();
				return hashCode;
			}
		}

		public static bool operator ==(GameJoltUser left, GameJoltUser right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(GameJoltUser left, GameJoltUser right)
		{
			return !left.Equals(right);
		}

		public override string ToString()
		{
			using (StringBuilderPool.Rent(out StringBuilder sb))
			{
				sb.Append(nameof(GameJoltUser) + " (" + nameof(Id) + ": ");
				sb.Append(Id);
				sb.Append(", " + nameof(Type) + ": ");
				sb.Append(Type);
				sb.Append(", " + nameof(Username) + ": ");
				sb.Append(Username);
				sb.Append(", " + nameof(AvatarUrl) + ": ");
				sb.Append(AvatarUrl);
				sb.Append(", " + nameof(SignedUp) + ": ");
				sb.Append(SignedUp);
				sb.Append(", " + nameof(LastLoggedIn) + ": ");
				sb.Append(LastLoggedIn);
				sb.Append(", " + nameof(OnlineNow) + ": ");
				sb.Append(OnlineNow);
				sb.Append(", " + nameof(Status) + ": ");
				sb.Append(Status);
				sb.Append(", " + nameof(DisplayName) + ": ");
				sb.Append(DisplayName);

				if (!string.IsNullOrEmpty(UserWebsite))
				{
					sb.Append(", " + nameof(UserWebsite) + ": ");
					sb.Append(UserWebsite);
				}

				sb.Append(", " + nameof(UserDescription) + ": ");
				sb.Append(UserDescription);

				sb.Append(')');

				return sb.ToString();
			}
		}
	}
}