#nullable enable

using System;
using GameJolt.NET.Tests.Enums;
using GameJolt.NET.Tests.Extensions;
using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests.Serialization.Converters
{
	public sealed class FetchFriendsResponseConverter : BaseSerializationTest
	{
		[Test]
		public void WriteJson([Values] bool success, [Values] bool nullMessage, [Values] ArrayInitialization arrayInitialization)
		{
			FriendId[]? friends = arrayInitialization.CreateArray(f => new FriendId(f.Random.Int()));
			string? message = nullMessage ? null : faker.Lorem.Sentence();

			FetchFriendsResponse response = new FetchFriendsResponse(success, message, friends);
			string json = Serialize(response);

			string expected = WriteExpectedResponse(success, message, sb =>
			{
				sb.Append(",");
				sb.AppendJsonPropertyName("friends");
				sb.AppendArray(friends, (builder, friend) =>
				{
					builder.Append("{");
					builder.AppendJsonPropertyName("friend_id");
					builder.Append(friend.id);
					builder.Append("}");
				}, true); // Serializers write [] instead of null for null arrays.
			});

			Assert.That(json, Is.EqualTo(expected));
		}

		[Test]
		public void ReadJson([Values] bool success,
			[Values] bool nullMessage,
			[Values] bool randomCapitalize,
			[Values] ArrayInitialization friendsInitialization)
		{
			FriendId[]? friends = friendsInitialization.CreateArray(f => new FriendId(f.Random.Int()));
			string? message = nullMessage ? null : faker.Lorem.Sentence();

			FriendId[]? friends1 = friends;
			string json = WriteExpectedResponse(success, message, sb =>
			{
				sb.Append(",");
				sb.AppendJsonPropertyName(randomCapitalize ? "friends".RandomCapitalize() : "friends");
				sb.AppendArray(friends1, (builder, friend) =>
				{
					builder.Append("{");
					builder.AppendJsonPropertyName("friend_id");
					builder.Append(friend.id);
					builder.Append("}");
				});
			});

			FetchFriendsResponse response = Deserialize<FetchFriendsResponse>(json);

			// Arrays should never be null once they are deserialized.
			friends ??= Array.Empty<FriendId>();

			Assert.That(response.Success, Is.EqualTo(success));
			Assert.That(response.Message, Is.EqualTo(message));
			Assert.That(response.friends, Is.EqualTo(friends));
		}

		[Test]
		public void ReadJson_TooManyFields([Values] bool success,
			[Values] bool nullMessage,
			[Values] bool randomCapitalize,
			[Values] bool beforeData,
			[Values] ArrayInitialization friendsInitialization)
		{
			FriendId[]? friends = friendsInitialization.CreateArray(f => new FriendId(f.Random.Int()));
			string? message = nullMessage ? null : faker.Lorem.Sentence();

			FriendId[]? friends1 = friends;
			string json = WriteExpectedResponse(success, message, sb =>
			{
				if (beforeData)
				{
					sb.Append(",");
					sb.AppendJsonPropertyName("extra");
					sb.AppendStringValue(faker.Lorem.Word());
				}

				sb.Append(",");
				sb.AppendJsonPropertyName(randomCapitalize ? "friends".RandomCapitalize() : "friends");
				sb.AppendArray(friends1, (builder, friend) =>
				{
					builder.Append("{");
					builder.AppendJsonPropertyName("friend_id");
					builder.Append(friend.id);
					builder.Append("}");
				});

				if (!beforeData)
				{
					sb.Append(",");
					sb.AppendJsonPropertyName("extra");
					sb.AppendStringValue(faker.Lorem.Word());
				}
			});

			FetchFriendsResponse response = Deserialize<FetchFriendsResponse>(json);

			// Arrays should never be null once they are deserialized.
			friends ??= Array.Empty<FriendId>();

			Assert.That(response.Success, Is.EqualTo(success));
			Assert.That(response.Message, Is.EqualTo(message));
			Assert.That(response.friends, Is.EqualTo(friends));
		}
	}
}