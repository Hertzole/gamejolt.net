using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests.Exceptions
{
	public sealed class DataStore : BaseExceptionTest
	{
		[Test]
		public void InvalidKey()
		{
			AssertError<GameJoltInvalidDataStoreKeyException>("You must enter the key for the item you would like to retrieve data for.");
		}

		[Test]
		public void InvalidData()
		{
			AssertError<GameJoltInvalidDataStoreValueException>("You must enter data with the request.");
		}

		[Test]
		public void NoDataStoreItem()
		{
			AssertError<GameJoltInvalidDataStoreKeyException>("There is no item with the key passed in:");
		}

		[Test]
		public void NoOperation()
		{
			AssertError<GameJoltException>("You must enter an operation with the request.");
		}

		[Test]
		public void InvalidOperation()
		{
			AssertError<GameJoltException>("Operation must be add, subtract, multiply, divide, append or prepend.");
		}

		[Test]
		public void NoValue()
		{
			AssertError<GameJoltInvalidDataStoreValueException>("You must enter an value with the request.");
		}

		[Test]
		public void InvalidMathOperation()
		{
			AssertError<GameJoltInvalidDataStoreValueException>("Mathematical operations require the pre-existing data stored to also be numeric.");
		}
		
		[Test]
		public void MathOperationNoValue()
		{
			AssertError<GameJoltInvalidDataStoreValueException>("Value must be numeric if operation is mathematical.");
		}
		
		[Test]
		public void DivideByZero()
		{
			AssertError<GameJoltInvalidDataStoreValueException>("GAME JOLT STOP: 0x00000019 (0x00000000, 0xC00E0FF0, 0xFFFFEFD4, 0xC0000000) UNIVERSAL_COLLAPSE");
		}

		[Test]
		public void NoData()
		{
			AssertError<GameJoltInvalidDataStoreKeyException>("No item with that key could be found.");
		}
	}
}