#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

namespace Hertzole.GameJolt
{
	/// <summary>
	///     The exception that is thrown when a value for a data store item is invalid.
	/// </summary>
	public sealed class GameJoltInvalidDataStoreValueException : GameJoltException
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="GameJoltInvalidDataStoreValueException" /> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public GameJoltInvalidDataStoreValueException(string message) : base(message) { }
	}
}
#endif // DISABLE_GAMEJOLT