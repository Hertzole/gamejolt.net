namespace Hertzole.GameJolt
{
	public readonly struct GameJoltTrophy
	{
		public int Id { get; }
		public string Title { get; }
		public string Description { get; }
		public TrophyDifficulty Difficulty { get; }
		public string ImageUrl { get; }
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