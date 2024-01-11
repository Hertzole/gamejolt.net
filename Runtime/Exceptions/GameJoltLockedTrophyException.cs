namespace Hertzole.GameJolt
{
	public sealed class GameJoltLockedTrophyException : GameJoltException
	{
		public GameJoltLockedTrophyException() : base(MESSAGE) { }

		internal const string MESSAGE = "The user does not have this trophy.";
	}
}