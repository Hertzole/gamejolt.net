#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

namespace Hertzole.GameJolt
{
	/// <summary>
	///     The exception that is thrown when the high score table ID is invalid for any reason.
	/// </summary>
	public sealed class GameJoltInvalidTableException : GameJoltException
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="GameJoltInvalidTableException" /> class.
		/// </summary>
		public GameJoltInvalidTableException() : base(MESSAGE) { }

		internal const string MESSAGE = "The high score table ID you passed in does not belong to this game or has been deleted.";
	}
}
#endif // DISABLE_GAMEJOLT