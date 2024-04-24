namespace Hertzole.GameJolt
{
	internal interface IGameJoltSerializer
	{
		string SerializeResponse<T>(T value);

		T DeserializeResponse<T>(string value);
	}
}