namespace Hertzole.GameJolt
{
	internal interface IGameJoltSerializer
	{
		string Serialize<T>(T value);

		T Deserialize<T>(string value);
	}
}