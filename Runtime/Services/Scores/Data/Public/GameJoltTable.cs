namespace Hertzole.GameJolt
{
	/// <summary>
	///     A Game Jolt score table.
	/// </summary>
	public readonly struct GameJoltTable
	{
		/// <summary>
		///     The ID of the score table.
		/// </summary>
		public int Id { get; }
		/// <summary>
		///     The developer-defined name of the score table.
		/// </summary>
		public string Name { get; }
		/// <summary>
		///     The developer-defined description of the score table.
		/// </summary>
		public string Description { get; }
		/// <summary>
		///     Whether or not this is the primary score table. Scores are submitted to the primary score table by default.
		/// </summary>
		public bool IsPrimary { get; }

		internal GameJoltTable(int id, string name, string description, bool isPrimary)
		{
			Id = id;
			Name = name;
			Description = description;
			IsPrimary = isPrimary;
		}

		public override string ToString()
		{
			return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(Description)}: {Description}, {nameof(IsPrimary)}: {IsPrimary}";
		}
	}
}