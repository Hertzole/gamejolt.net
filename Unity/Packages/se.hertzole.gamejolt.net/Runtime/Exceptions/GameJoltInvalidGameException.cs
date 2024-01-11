namespace Hertzole.GameJolt
{
	public sealed class GameJoltInvalidGameException : GameJoltException
	{
		public GameJoltInvalidGameException() : base(MESSAGE) { }

		internal const string MESSAGE = "The game ID you passed in does not point to a valid game.";
	}
}