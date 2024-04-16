using System;

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

		public bool Equals(GameJoltTrophy other)
		{
			return Id == other.Id && Difficulty == other.Difficulty && HasAchieved == other.HasAchieved &&
			       EqualityHelper.StringEquals(Title, other.Title) &&
			       EqualityHelper.StringEquals(Description, other.Description) &&
			       EqualityHelper.StringEquals(ImageUrl, other.ImageUrl);
		}

		public override bool Equals(object obj)
		{
			return obj is GameJoltTrophy other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = Id;
				hashCode = (hashCode * 397) ^ (int) Difficulty;
				hashCode = (hashCode * 397) ^ HasAchieved.GetHashCode();
				hashCode = (hashCode * 397) ^ (Title != null ? Title.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (Description != null ? Description.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (ImageUrl != null ? ImageUrl.GetHashCode() : 0);
				return hashCode;
			}
		}

		public static bool operator ==(GameJoltTrophy left, GameJoltTrophy right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(GameJoltTrophy left, GameJoltTrophy right)
		{
			return !left.Equals(right);
		}

		public override string ToString()
		{
			return $"{nameof(GameJoltTrophy)} ({nameof(Id)}: {Id}, {nameof(Title)}: {Title}, {nameof(Description)}: {Description}, {nameof(Difficulty)}: {Difficulty}, {nameof(ImageUrl)}: {ImageUrl}, {nameof(HasAchieved)}: {HasAchieved})";
		}
	}
}