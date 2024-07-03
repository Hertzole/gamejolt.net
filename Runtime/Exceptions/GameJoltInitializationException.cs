#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

namespace Hertzole.GameJolt
{
	public sealed class GameJoltInitializationException : GameJoltException
	{
		public GameJoltInitializationException() : base(MESSAGE) { }

		internal const string MESSAGE = "GameJolt.NET has not been initialized yet. Please call GameJoltAPI.Initialize() first.";
	}
}
#endif // DISABLE_GAMEJOLT