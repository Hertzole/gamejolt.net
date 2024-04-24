using Bogus;
using Hertzole.GameJolt;

namespace GameJolt.NET.Tests.Serialization.Converters
{
	public class BaseSerializationTest
	{
		protected readonly Faker faker = new Faker();

		protected string Serialize<T>(T data)
		{
			return GameJoltAPI.serializer.SerializeResponse(data);
		}
		
		protected T Deserialize<T>(string json)
		{
			return GameJoltAPI.serializer.DeserializeResponse<T>(json);
		}
	}
}