using System;

namespace Hertzole.GameJolt
{
	/// <summary>
	///     A Game Jolt score table.
	/// </summary>
	public readonly struct GameJoltTable : IEquatable<GameJoltTable>
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

		public bool Equals(GameJoltTable other)
		{
			return Id == other.Id && IsPrimary == other.IsPrimary &&
			       EqualityHelper.StringEquals(Name, other.Name) && EqualityHelper.StringEquals(Description, other.Description);
		}

		public override bool Equals(object obj)
		{
			return obj is GameJoltTable other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = Id;
				hashCode = (hashCode * 397) ^ IsPrimary.GetHashCode();
				hashCode = (hashCode * 397) ^ (!string.IsNullOrEmpty(Name) ? Name.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (!string.IsNullOrEmpty(Description) ? Description.GetHashCode() : 0);
				return hashCode;
			}
		}

		public static bool operator ==(GameJoltTable left, GameJoltTable right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(GameJoltTable left, GameJoltTable right)
		{
			return !left.Equals(right);
		}

		public override string ToString()
		{
			return
				$"{nameof(GameJoltTable)} ({nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(Description)}: {Description}, {nameof(IsPrimary)}: {IsPrimary})";
		}
	}
}