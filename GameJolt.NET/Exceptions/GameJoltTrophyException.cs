#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

namespace Hertzole.GameJolt
{
	public sealed class GameJoltTrophyException : GameJoltException
	{
		internal const string ALREADY_UNLOCKED_MESSAGE = "The user already has this trophy.";
		internal const string DOES_NOT_HAVE_MESSAGE = "The user does not have this trophy.";
		
		public GameJoltTrophyException(string message) : base(message) { }
	}
}
#endif // DISABLE_GAMEJOLT