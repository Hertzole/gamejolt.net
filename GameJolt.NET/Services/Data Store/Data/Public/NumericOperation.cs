#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

namespace Hertzole.GameJolt
{
	/// <summary>
	///     A numeric operation to perform on numeric data in <see cref="GameJoltDataStore" />.
	/// </summary>
	public enum NumericOperation
	{
		/// <summary>
		///     Adds the value to the current data store item.
		/// </summary>
		/// <example>1 + 2 = 3</example>
		Add = 0,
		/// <summary>
		///     Subtracts the value from the current data store item.
		/// </summary>
		/// <example>3 - 1 = 2</example>
		Subtract = 1,
		/// <summary>
		///     Multiplies the value with the current data store item.
		/// </summary>
		/// <example>2 * 3 = 6</example>
		Multiply = 2,
		/// <summary>
		///     Divides the value with the current data store item.
		/// </summary>
		/// <example>6 / 2 = 3</example>
		Divide = 3,
		/// <summary>
		///     Appends the value to the current data store item.
		/// </summary>
		/// <example>1 (append) 2 = 12</example>
		Append = 4,
		/// <summary>
		///     Prepends the value to the current data store item.
		/// </summary>
		/// <example>1 (prepend) 2 = 21</example>
		Prepend = 5
	}
}
#endif // DISABLE_GAMEJOLT