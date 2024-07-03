#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

using Bogus;
using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests.ToString
{
	public sealed class DataStore
	{
		private readonly Faker faker = new Faker();

		[Test]
		public void DataKey()
		{
			string data = faker.Random.Utf16String();
			Assert.That(new DataKey(data).ToString(), Is.EqualTo($"DataKey (key: {data})"));
		}

		[Test]
		public void GetDataResponse()
		{
			string data = faker.Random.Utf16String();
			bool success = faker.Random.Bool();
			string message = faker.Random.Utf16String();

			Assert.That(new GetDataResponse(success, message, data).ToString(),
				Is.EqualTo($"GetDataResponse (Success: {success}, Message: {message}, data: {data})"));
		}

		[Test]
		public void GetKeysResponse()
		{
			DataKey[] keys = new DataKey[faker.Random.Int(0, 10)];
			for (int i = 0; i < keys.Length; i++)
			{
				keys[i] = new DataKey(faker.Random.Utf16String());
			}

			bool success = faker.Random.Bool();
			string message = faker.Random.Utf16String();

			Assert.That(new GetKeysResponse(success, message, keys).ToString(),
				Is.EqualTo($"GetKeysResponse (Success: {success}, Message: {message}, keys: {keys.ToCommaSeparatedString()})"));
		}

		[Test]
		public void UpdateDataResponse()
		{
			string data = faker.Random.Utf16String();
			bool success = faker.Random.Bool();
			string message = faker.Random.Utf16String();

			Assert.That(new UpdateDataResponse(success, message, data).ToString(),
				Is.EqualTo($"UpdateDataResponse (Success: {success}, Message: {message}, data: {data})"));
		}
	}
}
#endif // DISABLE_GAMEJOLT