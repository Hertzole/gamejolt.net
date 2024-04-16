#nullable enable

using System;
using Bogus;
using Hertzole.GameJolt;

namespace GameJolt.NET.Tests
{
	internal static class DummyData
	{
		internal static readonly Randomizer randomizer = new Randomizer();
		internal static readonly Faker faker = new Faker();

		internal static User User(int? id = null, UserType? type = null, string? username = null, string? avatarUrl = null, DateTime? signedUp = null,
			DateTime? lastLoggedIn = null, UserStatus? status = null, string? displayName = null, string? userWebsite = null, string? userDescription = null)
		{
			id ??= randomizer.Int(1, (int) (int.MaxValue * 0.5f));
			type ??= (UserType) randomizer.Int(0, 3);
			username ??= faker.Internet.UserName();
			avatarUrl ??= faker.Internet.Avatar();
			signedUp ??= faker.Date.Past();
			lastLoggedIn ??= faker.Date.Past();
			status ??= randomizer.Enum<UserStatus>();
			displayName ??= faker.Name.FullName();
			userWebsite ??= faker.Internet.Url();
			userDescription ??= faker.Lorem.Sentence();

			string signedUpString = signedUp.Value.ToString("MMMM d, yyyy");
			string lastLoggedInString = lastLoggedIn.Value.ToString("MMMM d, yyyy");

			int signedUpTimestamp = (int) (signedUp.Value - new DateTime(1970, 1, 1)).TotalSeconds;
			int lastLoggedInTimestamp = (int) (lastLoggedIn.Value - new DateTime(1970, 1, 1)).TotalSeconds;

			return new User(id.Value, type.Value, username, avatarUrl, signedUpString, signedUpTimestamp, lastLoggedInString, lastLoggedInTimestamp, status.Value,
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