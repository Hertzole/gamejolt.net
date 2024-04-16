namespace Hertzole.GameJolt
{
	public sealed class GameJoltInvalidUserException : GameJoltException
	{
		public GameJoltInvalidUserException() : base(MESSAGE) { }
		
		public GameJoltInvalidUserException(string message) : base(message) { }

		internal const string MESSAGE = "No such user could be found.";
		internal const string NULL_USER_MESSAGE = "User was null when it was not supposed to be. This is a bug!";
	}
}