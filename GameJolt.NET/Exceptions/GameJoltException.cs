using System;

namespace Hertzole.GameJolt
{
	public class GameJoltException : Exception
	{
		internal const string UNKNOWN_FATAL_ERROR = "Unknown fatal error occurred.";
        
		internal static GameJoltException UnknownFatalError { get; } = new GameJoltException(UNKNOWN_FATAL_ERROR);

		public GameJoltException(string message) : base(message) { }
	}
}