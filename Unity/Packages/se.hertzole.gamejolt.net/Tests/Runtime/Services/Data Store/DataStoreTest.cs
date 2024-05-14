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
	}
}