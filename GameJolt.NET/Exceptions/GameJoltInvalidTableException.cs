namespace Hertzole.GameJolt
{
	public sealed class GameJoltInvalidTableException : GameJoltException
	{
		public GameJoltInvalidTableException() : base(MESSAGE) { }

		internal const string MESSAGE = "The high score table ID you passed in does not belong to this game or has been deleted.";
	}
}