#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

#nullable enable

using System;
using System.Text;
using GameJolt.NET.Tests.Enums;
using GameJolt.NET.Tests.Extensions;
using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests.Serialization.Converters
{
	public class UsersFetchResponseConverter : BaseSerializationTest
	{
		[Test]
		public void WriteJson([Values] bool success, [Values] bool nullMessage, [Values] bool apostrophe, [Values] ArrayInitialization arrayInitialization)
		{
			string username = faker.Internet.UserName();
			string displayName = faker.Company.CompanyName();
			string website = faker.Internet.Url();
			string description = faker.Lorem.Sentence();
			
			if (apostrophe)
			{
				username = faker.Name.FirstName() + " '" + faker.Name.LastName();
				displayName = faker.Company.CompanyName() + " ' " + faker.Company.CompanyName();
				website = faker.Internet.Url() + " ' " + faker.Internet.Url();
				description = faker.Lorem.Sentence() + " ' " + faker.Lorem.Sentence();
			}
			
			User[]? users = arrayInitialization.CreateArray(f => DummyData.User(username: username, displayName: displayName, userWebsite: website, userDescription: description));
			
			string? message = nullMessage ? null : faker.Lorem.Sentence();
			UsersFetchResponse response = new UsersFetchResponse(success, message, users);
			string json = Serialize(response);
			
			string expected = WriteExpectedResponse(success, message, sb =>
			{
				sb.Append(",\"users\":");
				if (users == null || users.Length == 0)
				{
					sb.Append("[]");
				}
				else
				{
					sb.Append('[');
					for (int i = 0; i < users.Length; i++)
					{
						sb.Append("{\"id\":");
						sb.Append(users[i].id);
						sb.Append(",\"type\":\"");
						sb.Append(users[i].type);
						sb.Append("\",\"username\":\"");
						sb.Append(users[i].username.ReplaceWithUnicode());
						sb.Append("\",\"avatar_url\":\"");
						sb.Append(users[i].avatarUrl);
						sb.Append("\",\"signed_up\":\"");
						sb.Append(users[i].signedUp);
						sb.Append("\",\"signed_up_timestamp\":");
						sb.Append(users[i].signedUpTimestamp);
						sb.Append(",\"last_logged_in\":\"");
						sb.Append(users[i].lastLoggedIn);
						sb.Append("\",\"last_logged_in_timestamp\":");
						sb.Append(users[i].lastLoggedInTimestamp);
						sb.Append(",\"status\":\"");
						sb.Append(users[i].status);
						sb.Append("\",\"developer_name\":\"");
						sb.Append(users[i].displayName.ReplaceWithUnicode()); // Replace single quotes with the unicode character for single quotes.
						sb.Append("\",\"developer_website\":");
						sb.Append(users[i].userWebsite == null ? "null" : $"\"{users[i].userWebsite!.ReplaceWithUnicode()}\"");
						sb.Append(",\"developer_description\":\"");
						sb.Append(users[i].userDescription.ReplaceWithUnicode());
						sb.Append("\"}");
						if (i < users.Length - 1)
						{
							sb.Append(',');
						}
					}
					sb.Append(']');
				}
			});
			
			Assert.That(json, Is.EqualTo(expected));
		}
		
		[Test]
		public void ReadJson([Values] bool success, [Values] bool nullMessage, [Values] ArrayInitialization arrayInitialization)
		{
			User[]? users = arrayInitialization.CreateArray(f => DummyData.User());
			
			string? message = nullMessage ? null : faker.Lorem.Sentence();
			UsersFetchResponse expected = new UsersFetchResponse(success, message, users);
			string json = Serialize(expected);

			if (arrayInitialization == ArrayInitialization.NullArray)
			{
				json = json.Replace("\"users\":[]", "\"users\":null");
			}
			
			UsersFetchResponse response = Deserialize<UsersFetchResponse>(json);

			users ??= Array.Empty<User>();
			
			Assert.That(response.Success, Is.EqualTo(success));
			Assert.That(response.Message, Is.EqualTo(message));
			Assert.That(EqualityHelper.ArrayEquals(users, response.Users), Is.True);
		}

		[Test]
		public void ReadJson_TooManyFields([Values] bool success, [Values] bool nullMessage)
		{
			string? message = nullMessage ? null : faker.Lorem.Sentence();
			UsersFetchResponse expected = new UsersFetchResponse(success, message, null);
			
			string json = $"{{\"success\":{success.ToString().ToLower()},\"message\":{(nullMessage ? "null" : "\"" + message + "\"") },\"extra\":{{}},\"users\":[]}}";
			UsersFetchResponse response = Deserialize<UsersFetchResponse>(json);
			
			Assert.That(response.Success, Is.EqualTo(success));
			Assert.That(response.Message, Is.EqualTo(message));
			Assert.That(EqualityHelper.ArrayEquals(expected.Users, response.Users), Is.True);
		}
	}
}
#endif // DISABLE_GAMEJOLT