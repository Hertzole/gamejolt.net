namespace Hertzole.GameJolt
{
	public sealed class GameJoltInvalidDataStoreKeyException : GameJoltException
	{
		internal const string NO_KEY_MESSAGE = "You must enter the key for the item you would like to retrieve data for.";
		
		public GameJoltInvalidDataStoreKeyException(string message) : base(message) { }
	}
}