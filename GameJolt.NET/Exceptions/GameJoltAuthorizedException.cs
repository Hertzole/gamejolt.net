#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

namespace Hertzole.GameJolt
{
	/// <summary>
	///     The exception that is thrown when a GameJolt user could not perform an action because they are not authorized.
	/// </summary>
	public sealed class GameJoltAuthorizedException : GameJoltException
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="GameJoltAuthorizedException" /> class with a specified error message.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public GameJoltAuthorizedException(string message) : base(message) { }

		/// <summary>
		///     Initializes a new instance of the <see cref="GameJoltAuthorizedException" /> class.
		/// </summary>
		public GameJoltAuthorizedException() : base(MESSAGE) { }

		internal const string MESSAGE = "You must be logged in to perform this action.";
	}
}
#endif // DISABLE_GAMEJOLT