namespace Hertzole.GameJolt
{
	public readonly struct GameJoltTable
	{
		public int Id { get; }
		public string Name { get; }
		public string Description { get; }
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