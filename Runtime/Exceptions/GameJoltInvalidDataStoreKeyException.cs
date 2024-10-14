#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

namespace Hertzole.GameJolt
{
	/// <summary>
	///     The exception that is thrown when a value key for a data store item is not provided.
	/// </summary>
	public sealed class GameJoltInvalidDataStoreKeyException : GameJoltException
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="GameJoltInvalidDataStoreKeyException" /> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public GameJoltInvalidDataStoreKeyException(string message) : base(message) { }

		internal const string NO_KEY_MESSAGE = "You must enter the key for the item you would like to retrieve data for.";
	}
}
#endif // DISABLE_GAMEJOLT