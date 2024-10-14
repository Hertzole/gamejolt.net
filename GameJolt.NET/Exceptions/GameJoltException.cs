#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

using System;

namespace Hertzole.GameJolt
{
	/// <summary>
	///     Base exception for all GameJolt exceptions.
	/// </summary>
	public class GameJoltException : Exception
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="GameJoltException" /> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public GameJoltException(string message) : base(message) { }

		internal const string UNKNOWN_FATAL_ERROR = "Unknown fatal error occurred.";
	}
}
#endif // DISABLE_GAMEJOLT