#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

using System;
using System.Text;

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

		/// <inheritdoc />
		public bool Equals(GameJoltTable other)
		{
			return Id == other.Id && IsPrimary == other.IsPrimary &&
			       EqualityHelper.StringEquals(Name, other.Name) && EqualityHelper.StringEquals(Description, other.Description);
		}

		/// <inheritdoc />
		public override bool Equals(object obj)
		{
			return obj is GameJoltTable other && Equals(other);
		}

		/// <inheritdoc />
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

		/// <summary>
		///     Determines whether two specified instances of <see cref="GameJoltTable" /> are equal.
		/// </summary>
		/// <param name="left">The first <see cref="GameJoltTable" /> to compare.</param>
		/// <param name="right">The second <see cref="GameJoltTable" /> to compare.</param>
		/// <returns>
		///     <c>true</c> if <paramref name="left" /> and <paramref name="right" /> represent the same result; otherwise,
		///     <c>false</c>.
		/// </returns>
		public static bool operator ==(GameJoltTable left, GameJoltTable right)
		{
			return left.Equals(right);
		}

		/// <summary>
		///     Determines whether two specified instances of <see cref="GameJoltTable" /> are not equal.
		/// </summary>
		/// <param name="left">The first <see cref="GameJoltTable" /> to compare.</param>
		/// <param name="right">The second <see cref="GameJoltTable" /> to compare.</param>
		/// <returns>
		///     <c>true</c> if <paramref name="left" /> and <paramref name="right" /> do not represent the same result;
		///     otherwise, <c>false</c>.
		/// </returns>
		public static bool operator !=(GameJoltTable left, GameJoltTable right)
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
				builder.Append(nameof(GameJoltTable) + " (" + nameof(Id) + ": ");
				builder.Append(Id);
				builder.Append(", " + nameof(Name) + ": ");
				builder.Append(Name);
				builder.Append(", " + nameof(Description) + ": ");
				builder.Append(Description);
				builder.Append(", " + nameof(IsPrimary) + ": ");
				builder.Append(IsPrimary);
				builder.Append(')');

				return builder.ToString();
			}
		}
	}
}
#endif // DISABLE_GAMEJOLT