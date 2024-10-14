#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

namespace Hertzole.GameJolt
{
	/// <summary>
	///     The exception that is thrown when GameJolt.NET API has not been initialized yet.
	/// </summary>
	public sealed class GameJoltInitializationException : GameJoltException
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="GameJoltInitializationException" /> class.
		/// </summary>
		public GameJoltInitializationException() : base(MESSAGE) { }

		internal const string MESSAGE = "GameJolt.NET has not been initialized yet. Please call GameJoltAPI.Initialize() first.";
	}
}
#endif // DISABLE_GAMEJOLT