#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

using System;
using System.Text;

namespace Hertzole.GameJolt
{
	/// <summary>
	///     A Game Jolt trophy.
	/// </summary>
	public readonly struct GameJoltTrophy : IEquatable<GameJoltTrophy>
	{
		/// <summary>
		///     The ID of the trophy.
		/// </summary>
		public int Id { get; }
		/// <summary>
		///     The title of the trophy on the site.
		/// </summary>
		public string Title { get; }
		/// <summary>
		///     The trophy description text.
		/// </summary>
		public string Description { get; }
		/// <summary>
		///     The difficulty of the trophy.
		/// </summary>
		public TrophyDifficulty Difficulty { get; }
		/// <summary>
		///     The URL to the trophy's image.
		/// </summary>
		public string ImageUrl { get; }
		/// <summary>
		///     Whether or not the user has achieved the trophy.
		/// </summary>
		public bool HasAchieved { get; }

		internal GameJoltTrophy(int id, string title, string description, TrophyDifficulty difficulty, string imageUrl, bool hasAchieved)
		{
			Id = id;
			Title = title;
			Description = description;
			Difficulty = difficulty;
			ImageUrl = imageUrl;
			HasAchieved = hasAchieved;
		}

		/// <inheritdoc />
		public bool Equals(GameJoltTrophy other)
		{
			return Id == other.Id && Difficulty == other.Difficulty && HasAchieved == other.HasAchieved &&
			       EqualityHelper.StringEquals(Title, other.Title) &&
			       EqualityHelper.StringEquals(Description, other.Description) &&
			       EqualityHelper.StringEquals(ImageUrl, other.ImageUrl);
		}

		/// <inheritdoc />
		public override bool Equals(object obj)
		{
			return obj is GameJoltTrophy other && Equals(other);
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = Id;
				hashCode = (hashCode * 397) ^ (int) Difficulty;
				hashCode = (hashCode * 397) ^ HasAchieved.GetHashCode();
				hashCode = (hashCode * 397) ^ (!string.IsNullOrEmpty(Title) ? Title.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (!string.IsNullOrEmpty(Description) ? Description.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (!string.IsNullOrEmpty(ImageUrl) ? ImageUrl.GetHashCode() : 0);
				return hashCode;
			}
		}

		/// <summary>
		///     Determines whether two specified instances of <see cref="GameJoltTrophy" /> are equal.
		/// </summary>
		/// <param name="left">The first <see cref="GameJoltTrophy" /> to compare.</param>
		/// <param name="right">The second <see cref="GameJoltTrophy" /> to compare.</param>
		/// <returns>
		///     <c>true</c> if <paramref name="left" /> and <paramref name="right" /> represent the same result; otherwise,
		///     <c>false</c>.
		/// </returns>
		public static bool operator ==(GameJoltTrophy left, GameJoltTrophy right)
		{
			return left.Equals(right);
		}

		/// <summary>
		///     Determines whether two specified instances of <see cref="GameJoltTrophy" /> are not equal.
		/// </summary>
		/// <param name="left">The first <see cref="GameJoltTrophy" /> to compare.</param>
		/// <param name="right">The second <see cref="GameJoltTrophy" /> to compare.</param>
		/// <returns>
		///     <c>true</c> if <paramref name="left" /> and <paramref name="right" /> do not represent the same result;
		///     otherwise, <c>false</c>.
		/// </returns>
		public static bool operator !=(GameJoltTrophy left, GameJoltTrophy right)
		{
			return !left.Equals(right);
		}

		/// <summary>
		///     Returns a string that represents the current object.
		/// </summary>
		/// <returns>A string that represents the current object.</returns>
		public override string ToString()
		{
			using (StringBuilderPool.Rent(out StringBuilder builder))
			{
				builder.Append(nameof(GameJoltTrophy) + " (" + nameof(Id) + ": ");
				builder.Append(Id);
				builder.Append(", " + nameof(Title) + ": ");
				builder.Append(Title);
				builder.Append(", " + nameof(Description) + ": ");
				builder.Append(Description);
				builder.Append(", " + nameof(Difficulty) + ": ");
				builder.Append(Difficulty);
				builder.Append(", " + nameof(ImageUrl) + ": ");
				builder.Append(ImageUrl);
				builder.Append(", " + nameof(HasAchieved) + ": ");
				builder.Append(HasAchieved);
				builder.Append(')');

				return builder.ToString();
			}
		}
	}
}
#endif // DISABLE_GAMEJOLT