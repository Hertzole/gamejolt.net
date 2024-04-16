#nullable enable

using GameJolt.NET.Tests.Enums;
using GameJolt.NET.Tests.Extensions;
using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests.ToString
{
	public sealed class Trophies : BaseToStringTest
	{
		[Test]
		public void FetchTrophiesResponse([Values] ArrayInitialization arrayInitialization, [Values] bool nullMessage)
		{
			TrophyInternal[]? trophies = CreateArray(arrayInitialization, f => new TrophyInternal(f.Random.Int(), f.Lorem.Sentence(), f.Lorem.Sentences(),
				f.PickRandom<TrophyDifficulty>(), f.Internet.Avatar(),
				f.Random.Bool()));

			bool success = faker.Random.Bool();
			string? message = nullMessage ? null : faker.Lorem.Sentence();

			FetchTrophiesResponse response = new FetchTrophiesResponse(success, message, trophies);

			Assert.That(response.ToString(),
				Is.EqualTo($"{nameof(FetchTrophiesResponse)} (Success: {success}, Message: {message}, trophies: {trophies.GetExpectedString()})"));
		}

		[Test]
		public void TrophyInternal()
		{
			int id = faker.Random.Int();
			string title = faker.Lorem.Sentence();
			string description = faker.Lorem.Sentences();
			TrophyDifficulty difficulty = faker.PickRandom<TrophyDifficulty>();
			string imageUrl = faker.Internet.Avatar();
			bool achieved = faker.Random.Bool();

			TrophyInternal trophy = new TrophyInternal(id, title, description, difficulty, imageUrl, achieved);

			Assert.That(trophy.ToString(),
				Is.EqualTo(
					$"{nameof(TrophyInternal)} (id: {id}, title: {title}, description: {description}, difficulty: {difficulty}, imageUrl: {imageUrl}, achieved: {achieved})"));
		}

		[Test]
		public void TrophyResponse([Values] bool nullMessage)
		{
			bool success = faker.Random.Bool();
			string? message = nullMessage ? null : faker.Lorem.Sentence();

			TrophyResponse response = new TrophyResponse(success, message);

			Assert.That(response.ToString(), Is.EqualTo($"{nameof(TrophyResponse)} (Success: {success}, Message: {message})"));
		}

		[Test]
		public void GameJoltTrophy()
		{
			int id = faker.Random.Int();
			string title = faker.Lorem.Sentence();
			string description = faker.Lorem.Sentences();
			TrophyDifficulty difficulty = faker.PickRandom<TrophyDifficulty>();
			string imageUrl = faker.Internet.Avatar();
			bool hasAchieved = faker.Random.Bool();

			GameJoltTrophy trophy = new GameJoltTrophy(id, title, description, difficulty, imageUrl, hasAchieved);

			Assert.That(trophy.ToString(),
				Is.EqualTo(
					$"{nameof(GameJoltTrophy)} (Id: {id}, Title: {title}, Description: {description}, Difficulty: {difficulty}, ImageUrl: {imageUrl}, HasAchieved: {hasAchieved})"));
		}
	}
}