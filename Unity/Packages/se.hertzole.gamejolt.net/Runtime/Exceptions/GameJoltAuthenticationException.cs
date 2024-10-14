#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

namespace Hertzole.GameJolt
{
	/// <summary>
	///     The exception that is thrown when a GameJolt user could not be authenticated.
	/// </summary>
	public sealed class GameJoltAuthenticationException : GameJoltException
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="GameJoltAuthenticationException" /> class.
		/// </summary>
		public GameJoltAuthenticationException() : base(MESSAGE) { }

		internal const string MESSAGE = "No such user with the credentials passed in could be found.";
	}
}
#endif // DISABLE_GAMEJOLT