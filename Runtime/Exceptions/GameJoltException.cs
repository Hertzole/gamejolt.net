using System;

namespace Hertzole.GameJolt
{
	public class GameJoltException : Exception
	{
		internal static GameJoltException UnknownFatalError { get; } = new GameJoltException("Unknown fatal error occurred.");

		public GameJoltException() { }

		public GameJoltException(string message) : base(message) { }
	}
}