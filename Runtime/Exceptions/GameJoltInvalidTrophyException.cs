#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

namespace Hertzole.GameJolt
{
	public sealed class GameJoltInvalidTrophyException : GameJoltException
	{
		public GameJoltInvalidTrophyException(string message) : base(message) { }

		internal const string DOES_NOT_BELONG_MESSAGE = "The trophy returned does not belong to this game.";
		internal const string INCORRECT_ID_MESSAGE = "Incorrect trophy ID.";
	}
}
#endif // DISABLE_GAMEJOLT