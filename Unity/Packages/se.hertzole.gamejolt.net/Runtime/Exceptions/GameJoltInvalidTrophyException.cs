#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

namespace Hertzole.GameJolt
{
	/// <summary>
	///     The exception that is thrown when a trophy is invalid.
	/// </summary>
	public sealed class GameJoltInvalidTrophyException : GameJoltException
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="GameJoltInvalidTrophyException" /> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public GameJoltInvalidTrophyException(string message) : base(message) { }

		internal const string DOES_NOT_BELONG_MESSAGE = "The trophy returned does not belong to this game.";
		internal const string INCORRECT_ID_MESSAGE = "Incorrect trophy ID.";
	}
}
#endif // DISABLE_GAMEJOLT