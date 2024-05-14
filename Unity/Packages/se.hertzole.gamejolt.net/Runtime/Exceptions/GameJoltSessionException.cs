namespace Hertzole.GameJolt
{
	public sealed class GameJoltSessionException : GameJoltException
	{
		internal const string MESSAGE = "Could not find an open session. You must open a new one.";
		
		public GameJoltSessionException(string message) : base(message) { }
	}
}