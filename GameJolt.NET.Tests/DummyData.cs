using System;
using Bogus;
using Hertzole.GameJolt;

namespace GameJolt.NET.Tests
{
	internal static class DummyData
	{
		internal static readonly Randomizer randomizer = new Randomizer();
		internal static readonly Faker faker = new Faker();
		
		internal static User User()
		{
			int id = randomizer.Int(1, (int) (int.MaxValue * 0.5f));
			UserType type = (UserType) randomizer.Int(0, 3);
			string username = faker.Internet.UserName();
			string avatarUrl = faker.Internet.Avatar();
			DateTime signedUp = faker.Date.Past();
			DateTime lastLoggedIn = faker.Date.Past();
			UserStatus status = randomizer.Enum<UserStatus>();
			string displayName = faker.Name.FullName();
			string? userWebsite = faker.Internet.Url();
			string userDescription = faker.Lorem.Sentence();

			string signedUpString = signedUp.ToString("MMMM d, yyyy");
			string lastLoggedInString = lastLoggedIn.ToString("MMMM d, yyyy");

			int signedUpTimestamp = (int) (signedUp - new DateTime(1970, 1, 1)).TotalSeconds;
			int lastLoggedInTimestamp = (int) (lastLoggedIn - new DateTime(1970, 1, 1)).TotalSeconds;

			return new User(id, type, username, avatarUrl, signedUpString, signedUpTimestamp, lastLoggedInString, lastLoggedInTimestamp, status,
				displayName, userWebsite, userDescription);
		}

		internal static TableInternal Table()
		{
			int id = randomizer.Int(1, (int) (int.MaxValue * 0.5f));
			string name = faker.Lorem.Word();
			string description = faker.Lorem.Sentence();
			bool isPrimary = randomizer.Bool();

			return new TableInternal(id, name, description, isPrimary);
		}

		internal static ScoreInternal Score()
		{
			int sort = randomizer.Int(1, (int) (int.MaxValue * 0.5f));
			string score = sort.ToString();
			string extraData = faker.Lorem.Sentence();
			int userId = randomizer.Int(1, (int) (int.MaxValue * 0.5f));
			string username = faker.Internet.UserName();
			string guestName = faker.Name.FullName();
			DateTime stored = faker.Date.Past();

			return new ScoreInternal(sort, score, extraData, username, userId, guestName, stored.ToString("MMMM d, yyyy"),
				(int) (stored - new DateTime(1970, 1, 1)).TotalSeconds);
		}

		internal static TrophyInternal Trophy()
		{
			int id = randomizer.Int(1, (int) (int.MaxValue * 0.5f));
			string title = faker.Lorem.Word();
			string description = faker.Lorem.Sentence();
			TrophyDifficulty difficulty = randomizer.Enum<TrophyDifficulty>();
			string imageUrl = faker.Internet.Avatar();
			bool achieved = randomizer.Bool();

			return new TrophyInternal(id, title, description, difficulty, imageUrl, achieved);
		}

		internal static byte[] Bytes()
		{
			return randomizer.Bytes(1024);
		}
	}
}