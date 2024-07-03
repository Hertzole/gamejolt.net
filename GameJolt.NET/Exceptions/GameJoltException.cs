#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

using System;

namespace Hertzole.GameJolt
{
	public class GameJoltException : Exception
	{
		internal const string UNKNOWN_FATAL_ERROR = "Unknown fatal error occurred.";

		public GameJoltException(string message) : base(message) { }
	}
}
#endif // DISABLE_GAMEJOLT