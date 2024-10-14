#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

namespace Hertzole.GameJolt
{
	/// <summary>
	///     The exception that is thrown when the user already has the trophy or does not have the trophy.
	/// </summary>
	public sealed class GameJoltTrophyException : GameJoltException
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="GameJoltTrophyException" /> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public GameJoltTrophyException(string message) : base(message) { }

		internal const string ALREADY_UNLOCKED_MESSAGE = "The user already has this trophy.";
		internal const string DOES_NOT_HAVE_MESSAGE = "The user does not have this trophy.";
	}
}
#endif // DISABLE_GAMEJOLT