#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

namespace Hertzole.GameJolt
{
	/// <summary>
	///     The exception that is thrown when the game ID is invalid.
	/// </summary>
	public sealed class GameJoltInvalidGameException : GameJoltException
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="GameJoltInvalidGameException" /> class.
		/// </summary>
		public GameJoltInvalidGameException() : base(MESSAGE) { }

		internal const string MESSAGE = "The game ID you passed in does not point to a valid game.";
	}
}
#endif // DISABLE_GAMEJOLT