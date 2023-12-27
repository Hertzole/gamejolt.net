namespace Hertzole.GameJolt
{
	public sealed class GameJoltLockedTrophyException : GameJoltException
	{
		internal const string MESSAGE = "The user does not have this trophy.";

		public GameJoltLockedTrophyException() : base(MESSAGE) { }
	}
}