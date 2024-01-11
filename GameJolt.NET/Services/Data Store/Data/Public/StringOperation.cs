namespace Hertzole.GameJolt
{
	/// <summary>
	///     A string operation to perform on string data in <see cref="GameJoltDataStore" />.
	/// </summary>
	public enum StringOperation
	{
		/// <summary>
		///     Appends the value to the current data store item.
		/// </summary>
		/// <example>1 (append) 2 = 12</example>
		Append = 0,
		/// <summary>
		///     Prepends the value to the current data store item.
		/// </summary>
		/// <example>1 (prepend) 2 = 21</example>
		Prepend = 1
	}
}