namespace Hertzole.GameJolt
{
	public sealed class GameJoltInvalidDataStoreValueException : GameJoltException
	{
		public GameJoltInvalidDataStoreValueException() { }

		public GameJoltInvalidDataStoreValueException(string message) : base(message) { }
	}
}