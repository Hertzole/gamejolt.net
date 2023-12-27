namespace Hertzole.GameJolt
{
	public sealed class GameJoltAuthenticationException : GameJoltException
	{
		public GameJoltAuthenticationException() : base(MESSAGE) { }

		internal const string MESSAGE = "No such user with the credentials passed in could be found.";
	}
}