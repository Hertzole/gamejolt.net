using System;
using System.Text;
using Bogus;
using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests.Equality
{
	public sealed class GameJoltResultTests : EqualityTest
	{
		private readonly Faker faker = new Faker();

		[Test]
		public void GameJoltResult_Success_Equality()
		{
			TestEquality((a, b) => a == b, (a, b) => a != b, GameJoltResult.Success(), GameJoltResult.Error(new Exception()));
		}

		[Test]
		public void GameJoltResult_Failure_Equality()
		{
			TestEquality((a, b) => a == b, (a, b) => a != b,
				GameJoltResult.Error(new Exception()),
				GameJoltResult.Error(new NotSupportedException("Test")),
				GameJoltResult.Error(null!));
		}

		[Test]
		public void GameJoltResult_DifferentObject_Equality()
		{
			AssertNotEqualObject<GameJoltResult>();
		}

		[Test]
		public void GenericGameJoltResult_Success_Equality()
		{
			TestEquality((a, b) => a == b, (a, b) => a != b,
				GameJoltResult<int>.Success(faker.Random.Int()));
		}

		[Test]
		public void GenericGameJoltResult_Failure_Equality()
		{
			TestEquality((a, b) => a == b, (a, b) => a != b,
				GameJoltResult<int>.Error(new Exception()),
				GameJoltResult<int>.Error(new NotSupportedException("Test")));

			TestEquality((a, b) => a == b, (a, b) => a != b,
				GameJoltResult<StringBuilder>.Error(new Exception()),
				GameJoltResult<StringBuilder>.Error(new NotSupportedException("Test")));
		}

		[Test]
		public void GenericGameJoltResult_DifferentObject_Equality()
		{
			AssertNotEqualObject<GameJoltResult<int>>();
		}
	}
}