#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

namespace Hertzole.GameJolt
{
	/// <summary>
	///     The exception that is thrown when no session matching the provided one could be found.
	/// </summary>
	public sealed class GameJoltSessionException : GameJoltException
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="GameJoltSessionException" /> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public GameJoltSessionException(string message) : base(message) { }

		internal const string MESSAGE = "Could not find an open session. You must open a new one.";
	}
}
#endif // DISABLE_GAMEJOLT