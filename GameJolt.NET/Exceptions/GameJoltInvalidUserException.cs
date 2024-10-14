#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

namespace Hertzole.GameJolt
{
	/// <summary>
	///     The exception that is thrown when no such user could be found.
	/// </summary>
	public sealed class GameJoltInvalidUserException : GameJoltException
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="GameJoltInvalidUserException" /> class.
		/// </summary>
		public GameJoltInvalidUserException() : base(MESSAGE) { }

		internal const string MESSAGE = "No such user could be found.";
	}
}
#endif // DISABLE_GAMEJOLT