using System.Text;
using GameJolt.NET.Tests.Extensions;
using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests.Serialization.Converters
{
	public sealed class FriendIdConverter : BaseSerializationTest
	{
		[Test]
		public void WriteJson()
		{
			FriendId friendId = new FriendId(faker.Random.Int());
			string json = Serialize(friendId);

			StringBuilder sb = new StringBuilder();
			sb.Append('{');
			sb.AppendJsonPropertyName("friend_id");
			sb.Append(friendId.id);
			sb.Append('}');

			Assert.That(json, Is.EqualTo(sb.ToString()));
		}

		[Test]
		public void ReadJson([Values] bool randomCapitalize)
		{
			int id = faker.Random.Int();

			StringBuilder sb = new StringBuilder();
			sb.Append('{');
			sb.AppendJsonPropertyName("friend_id", randomCapitalize);
			sb.Append(id);
			sb.Append('}');

			string json = sb.ToString();

			FriendId friendId = Deserialize<FriendId>(json);

			Assert.That(friendId.id, Is.EqualTo(id));
		}

		[Test]
		public void ReadJson_ExtraProperties([Values] bool randomCapitalize, [Values] bool beforeId)
		{
			int id = faker.Random.Int();

			StringBuilder sb = new StringBuilder();
			sb.Append('{');
			if (beforeId)
			{
				sb.AppendJsonPropertyName("extra_property", randomCapitalize);
				sb.AppendStringValue(faker.Lorem.Sentence());
				sb.Append(',');
			}

			sb.AppendJsonPropertyName("friend_id", randomCapitalize);
			sb.Append(id);
			if (!beforeId)
			{
				sb.Append(',');
				sb.AppendJsonPropertyName("extra_property", randomCapitalize);
				sb.AppendStringValue(faker.Lorem.Sentence());
			}

			sb.Append('}');

			string json = sb.ToString();

			FriendId friendId = Deserialize<FriendId>(json);

			Assert.That(friendId.id, Is.EqualTo(id));
		}
	}
}