namespace Hertzole.GameJolt
{
	public sealed class GameJoltAuthorizedException : GameJoltException
	{
		public GameJoltAuthorizedException(string message) : base(message) { }
		
		public GameJoltAuthorizedException() : base(MESSAGE) { }

		internal const string MESSAGE = "You must be logged in to perform this action.";
	}
}