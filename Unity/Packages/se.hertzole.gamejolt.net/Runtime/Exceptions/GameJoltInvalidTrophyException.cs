namespace Hertzole.GameJolt
{
	public sealed class GameJoltInvalidTrophyException : GameJoltException
	{
		public GameJoltInvalidTrophyException() : base(MESSAGE) { }

		public GameJoltInvalidTrophyException(string message) : base(message) { }

		internal const string MESSAGE = "The trophy returned does not belong to this game.";
	}
}