#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

#nullable enable

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests
{
	partial class DataStoreTest
	{
		private static readonly Predicate<ArgumentNullException> keyNullPredicate = exception =>
			exception.Message == "Parameter key (string) must not be null. (Parameter 'key')";
		private static readonly Predicate<ArgumentException> keyEmptyPredicate = exception =>
			exception.Message == "Parameter key (string) must not be empty or whitespace. (Parameter 'key')";
		private static readonly Predicate<ArgumentNullException> stringValueNullPredicate = e => MustNotBeNullPredicate<string>(e, "data");
		private static readonly Predicate<ArgumentException> stringValueEmptyPredicate =
			exception => exception.Message == "Parameter data (string) must not be empty or whitespace. (Parameter 'data')";
		private static readonly Predicate<ArgumentNullException> bytesValueNullPredicate = e => MustNotBeNullPredicate<IReadOnlyCollection<byte>>(e, "data");
		private static readonly Predicate<ArgumentException> bytesValueEmptyPredicate =
			exception => exception.Message == $"Parameter data ({typeof(IReadOnlyCollection<byte>).ToTypeString()}) must not be empty. (Parameter 'data')";

		[Test]
		public async Task SetAsync_String_Guards()
		{
			// Null key
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.SetAsync(null!, "value"), keyNullPredicate);
			// Empty key
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.SetAsync(string.Empty, "value"), keyEmptyPredicate);
			// Null value
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.SetAsync("key", (string) null!), stringValueNullPredicate);
			// Empty value
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.SetAsync("key", string.Empty), stringValueEmptyPredicate);
		}

		[Test]
		public async Task SetAsync_Int_Guards()
		{
			// Null key
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.SetAsync(null!, 123), keyNullPredicate);
			// Empty key
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.SetAsync(string.Empty, 123), keyEmptyPredicate);
		}

		[Test]
		public async Task SetAsync_Bytes_Guards()
		{
			// Null key
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.SetAsync(null!, new byte[] { 1, 2, 3 }), keyNullPredicate);
			// Empty key
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.SetAsync(string.Empty, new byte[] { 1, 2, 3 }), keyEmptyPredicate);
			// Null data
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.SetAsync("key", (byte[]) null!), bytesValueNullPredicate);
			// Empty data
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.SetAsync("key", Array.Empty<byte>()), bytesValueEmptyPredicate);
		}

		[Test]
		public async Task SetAsync_Bool_Guards()
		{
			// Null key
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.SetAsync(null!, true), keyNullPredicate);
			// Empty key
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.SetAsync(string.Empty, true), keyEmptyPredicate);
		}

		[Test]
		public async Task SetAsCurrentUserAsync_String_Guards()
		{
			// Null key
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.SetAsCurrentUserAsync(null!, "value"), keyNullPredicate);
			// Empty key
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.SetAsCurrentUserAsync(string.Empty, "value"), keyEmptyPredicate);
			// Null value
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.SetAsCurrentUserAsync("key", (string) null!), stringValueNullPredicate);
			// Empty value
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.SetAsCurrentUserAsync("key", string.Empty), stringValueEmptyPredicate);
		}

		[Test]
		public async Task SetAsCurrentUserAsync_Int_Guards()
		{
			// Null key
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.SetAsCurrentUserAsync(null!, 123), keyNullPredicate);
			// Empty key
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.SetAsCurrentUserAsync(string.Empty, 123), keyEmptyPredicate);
		}

		[Test]
		public async Task SetAsCurrentUserAsync_Bytes_Guards()
		{
			// Null key
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.SetAsCurrentUserAsync(null!, new byte[] { 1, 2, 3 }), keyNullPredicate);
			// Empty key
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.SetAsCurrentUserAsync(string.Empty, new byte[] { 1, 2, 3 }), keyEmptyPredicate);

			// Null data
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.SetAsCurrentUserAsync("key", (byte[]) null!), bytesValueNullPredicate);
			// Empty data
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.SetAsCurrentUserAsync("key", Array.Empty<byte>()), bytesValueEmptyPredicate);
		}

		[Test]
		public async Task SetAsCurrentUserAsync_Bool_Guards()
		{
			// Null key
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.SetAsCurrentUserAsync(null!, true), keyNullPredicate);
			// Empty key
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.SetAsCurrentUserAsync(string.Empty, true), keyEmptyPredicate);
		}

		[Test]
		public async Task RemoveAsync_Guards()
		{
			// Null key
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.RemoveAsync(null!), keyNullPredicate);
			// Empty key
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.RemoveAsync(string.Empty), keyEmptyPredicate);
		}

		[Test]
		public async Task RemoveAsCurrentUserAsync_Guards()
		{
			// Null key
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.RemoveAsCurrentUserAsync(null!), keyNullPredicate);
			// Empty key
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.RemoveAsCurrentUserAsync(string.Empty), keyEmptyPredicate);
		}

		[Test]
		public async Task UpdateAsync_String_Guards()
		{
			// Null key
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.UpdateAsync(null!, "value", StringOperation.Append), keyNullPredicate);
			// Empty key
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.UpdateAsync(string.Empty, "value", StringOperation.Append), keyEmptyPredicate);

			// Null data
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.UpdateAsync("key", null!, StringOperation.Append), stringValueNullPredicate);

			// Empty data
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.UpdateAsync("key", string.Empty, StringOperation.Append), stringValueEmptyPredicate);
		}

		[Test]
		public async Task UpdateAsync_Int_Guards()
		{
			// Null key
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.UpdateAsync(null!, 123, NumericOperation.Add), keyNullPredicate);
			// Empty key
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.UpdateAsync(string.Empty, 123, NumericOperation.Add), keyEmptyPredicate);
		}

		[Test]
		public async Task UpdateAsCurrentUserAsync_String_Guards()
		{
			// Null key
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.UpdateAsCurrentUserAsync(null!, "value", StringOperation.Append), keyNullPredicate);

			// Empty key
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.UpdateAsCurrentUserAsync(string.Empty, "value", StringOperation.Append), keyEmptyPredicate);

			// Null data
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.UpdateAsCurrentUserAsync("key", null!, StringOperation.Append), stringValueNullPredicate);

			// Empty data
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.UpdateAsCurrentUserAsync("key", string.Empty, StringOperation.Append),
				stringValueEmptyPredicate);
		}

		[Test]
		public async Task UpdateAsCurrentUserAsync_Int_Guards()
		{
			// Null key
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.UpdateAsCurrentUserAsync(null!, 123, NumericOperation.Add), keyNullPredicate);

			// Empty key
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.UpdateAsCurrentUserAsync(string.Empty, 123, NumericOperation.Add), keyEmptyPredicate);
		}

		[Test]
		public async Task GetValueAsStringAsync_Guards()
		{
			// Null key
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.GetValueAsStringAsync(null!), keyNullPredicate);
			// Empty key
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.GetValueAsStringAsync(string.Empty), keyEmptyPredicate);
		}

		[Test]
		public async Task GetValueAsIntAsync_Guards()
		{
			// Null key
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.GetValueAsIntAsync(null!), keyNullPredicate);
			// Empty key
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.GetValueAsIntAsync(string.Empty), keyEmptyPredicate);
		}

		[Test]
		public async Task GetValueAsBytesAsync_ReturnArray_Guards()
		{
			// Null key
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.GetValueAsBytesAsync(null!), keyNullPredicate);
			// Empty key
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.GetValueAsBytesAsync(string.Empty), keyEmptyPredicate);
		}

		[Test]
		public async Task GetValueAsBytesAsync_WithList_Guards()
		{
			// Null key
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.GetValueAsBytesAsync(null!, new List<byte>()), keyNullPredicate);
			// Empty key
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.GetValueAsBytesAsync(string.Empty, new List<byte>()), keyEmptyPredicate);
			// Null list
			await AssertThrowsAsync<ArgumentNullException>(() => GameJoltAPI.DataStore.GetValueAsBytesAsync("key", null!),
				e => MustNotBeNullPredicate<IList<byte>>(e, "result"));
		}

		[Test]
		public async Task GetValueAsBoolAsync_Guards()
		{
			// Null key
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.GetValueAsBoolAsync(null!), keyNullPredicate);
			// Empty key
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.GetValueAsBoolAsync(string.Empty), keyEmptyPredicate);
		}

		[Test]
		public async Task GetValueAsStringAsCurrentUserAsync_Guards()
		{
			// Null key
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.GetValueAsStringAsCurrentUserAsync(null!), keyNullPredicate);
			// Empty key
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.GetValueAsStringAsCurrentUserAsync(string.Empty), keyEmptyPredicate);
		}

		[Test]
		public async Task GetValueAsIntAsCurrentUserAsync_Guards()
		{
			// Null key
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.GetValueAsIntAsCurrentUserAsync(null!), keyNullPredicate);
			// Empty key
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.GetValueAsIntAsCurrentUserAsync(string.Empty), keyEmptyPredicate);
		}

		[Test]
		public async Task GetValueAsBytesAsCurrentUserAsync_ReturnArray_Guards()
		{
			// Null key
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.GetValueAsBytesAsCurrentUserAsync(null!), keyNullPredicate);
			// Empty key
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.GetValueAsBytesAsCurrentUserAsync(string.Empty), keyEmptyPredicate);
		}

		[Test]
		public async Task GetValueAsBytesAsCurrentUserAsync_WithList_Guards()
		{
			// Null key
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.GetValueAsBytesAsCurrentUserAsync(null!, new List<byte>()), keyNullPredicate);

			// Empty key
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.GetValueAsBytesAsCurrentUserAsync(string.Empty, new List<byte>()), keyEmptyPredicate);

			// Null list
			await AssertThrowsAsync<ArgumentNullException>(() => GameJoltAPI.DataStore.GetValueAsBytesAsCurrentUserAsync("key", null!),
				e => MustNotBeNullPredicate<IList<byte>>(e, "result"));
		}

		[Test]
		public async Task GetValueAsBoolAsCurrentUserAsync_Guards()
		{
			// Null key
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.GetValueAsBoolAsCurrentUserAsync(null!), keyNullPredicate);
			// Empty key
			await AssertThrowsAsync(() => GameJoltAPI.DataStore.GetValueAsBoolAsCurrentUserAsync(string.Empty), keyEmptyPredicate);
		}

		[Test]
		public async Task GetKeysAsync_WithList_Guards()
		{
			// Null list
			await AssertThrowsAsync<ArgumentNullException>(() => GameJoltAPI.DataStore.GetKeysAsync((List<string>) null!),
				e => MustNotBeNullPredicate<IList<string>>(e, "result"));
		}

		[Test]
		public async Task GetKeysAsCurrentUserAsync_WithList_Guards()
		{
			// Null list
			await AssertThrowsAsync<ArgumentNullException>(() => GameJoltAPI.DataStore.GetKeysAsCurrentUserAsync((List<string>) null!),
				e => MustNotBeNullPredicate<IList<string>>(e, "result"));
		}

		private static bool MustNotBeNullPredicate<T>(Exception e, string paramName)
		{
			return e.Message == $"Parameter {paramName} ({typeof(T).ToTypeString()}) must not be null. (Parameter '{paramName}')";
		}
	}
}

#endif // DISABLE_GAMEJOLT