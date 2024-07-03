#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

namespace Hertzole.GameJolt
{
	public sealed class GameJoltInvalidUserException : GameJoltException
	{
		public GameJoltInvalidUserException() : base(MESSAGE) { }

		internal const string MESSAGE = "No such user could be found.";
	}
}
#endif // DISABLE_GAMEJOLT