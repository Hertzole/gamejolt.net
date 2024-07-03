#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

namespace Hertzole.GameJolt
{
	public sealed class GameJoltInvalidDataStoreValueException : GameJoltException
	{
		public GameJoltInvalidDataStoreValueException(string message) : base(message) { }
	}
}
#endif // DISABLE_GAMEJOLT