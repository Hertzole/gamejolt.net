namespace Hertzole.GameJolt
{
	public sealed class GameJoltInvalidUserException : GameJoltException
	{
		public GameJoltInvalidUserException() : base(MESSAGE) { }

		internal const string MESSAGE = "No such user could be found.";
	}
}