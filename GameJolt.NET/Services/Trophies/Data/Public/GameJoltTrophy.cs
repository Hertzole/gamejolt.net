namespace Hertzole.GameJolt
{
	/// <summary>
	///     A Game Jolt trophy.
	/// </summary>
	public readonly struct GameJoltTrophy
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
	}
}