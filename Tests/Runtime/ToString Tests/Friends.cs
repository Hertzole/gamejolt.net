using Bogus;
using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests.ToString
{
	public sealed class Friends
	{
		private readonly Faker faker = new Faker();

		[Test]
		public void FetchFriendsResponse()
		{
			FriendId[] friends = new FriendId[faker.Random.Int(0, 10)];
			for (int i = 0; i < friends.Length; i++)
			{
				friends[i] = new FriendId(faker.Random.Int());
			}

			bool success = faker.Random.Bool();
			string message = faker.Random.Utf16String();

			Assert.That(new FetchFriendsResponse(success, message, friends).ToString(),
				Is.EqualTo($"FetchFriendsResponse (Success: {success}, Message: {message}, friends: {friends.ToCommaSeparatedString()})"));
		}

		[Test]
		public void FriendId()
		{
			int id = faker.Random.Int();
			Assert.That(new FriendId(id).ToString(), Is.EqualTo($"FriendId (id: {id})"));
		}
	}
}