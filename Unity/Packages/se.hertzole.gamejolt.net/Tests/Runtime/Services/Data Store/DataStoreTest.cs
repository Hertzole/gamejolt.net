#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

#nullable enable

using System;
using System.Threading.Tasks;
using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests
{
	public sealed partial class DataStoreTest : BaseTest
	{
		// Most of all the tests were moved to the partial classes as this file was getting too big.
		
		[Test]
		public async Task UpdateString_InvalidOperation_Throws()
		{
			bool caught = false;

			try
			{
				await GameJoltAPI.DataStore.UpdateAsync("key", "value", (StringOperation) 10);
			}
			catch (ArgumentOutOfRangeException)
			{
				caught = true;
			}

			Assert.That(caught, Is.True);
		}

		[Test]
		public async Task UpdateInt_InvalidOperation_Throws()
		{
			bool caught = false;

			try
			{
				await GameJoltAPI.DataStore.UpdateAsync("key", 1, (NumericOperation) 10);
			}
			catch (ArgumentOutOfRangeException)
			{
				caught = true;
			}

			Assert.That(caught, Is.True);
		}
        
        [Test]
        public void GetStringOperationString()
		{
			Assert.That(GameJoltDataStore.GetStringOperation(StringOperation.Append), Is.EqualTo("append"));
			Assert.That(GameJoltDataStore.GetStringOperation(StringOperation.Prepend), Is.EqualTo("prepend"));
		}

		[Test]
		public void GetNumberOperation()
		{
			Assert.That(GameJoltDataStore.GetNumberOperation(NumericOperation.Add), Is.EqualTo("add"));
			Assert.That(GameJoltDataStore.GetNumberOperation(NumericOperation.Subtract), Is.EqualTo("subtract"));
			Assert.That(GameJoltDataStore.GetNumberOperation(NumericOperation.Multiply), Is.EqualTo("multiply"));
			Assert.That(GameJoltDataStore.GetNumberOperation(NumericOperation.Divide), Is.EqualTo("divide"));
			Assert.That(GameJoltDataStore.GetNumberOperation(NumericOperation.Append), Is.EqualTo("append"));
			Assert.That(GameJoltDataStore.GetNumberOperation(NumericOperation.Prepend), Is.EqualTo("prepend"));
		}
	}
}
#endif // DISABLE_GAMEJOLT