﻿using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests.Equality
{
	public sealed class DataStore : EqualityTest
	{
		[Test]
		public void DataKey()
		{
			TestEquality((a, b) => a == b, (a, b) => a != b,
				new DataKey("key"),
				new DataKey("key2"),
				new DataKey(null),
				new DataKey(string.Empty));

			AssertNotEqualObject<DataKey>();
		}

		[Test]
		public void GetDataResponse()
		{
			TestEquality((a, b) => a == b, (a, b) => a != b,
				new GetDataResponse(true, "message", "data"),
				new GetDataResponse(false, "message", "data"),
				new GetDataResponse(true, "message2", "data"),
				new GetDataResponse(true, "message", "data2"),
				new GetDataResponse(true, "message", null),
				new GetDataResponse(true, "message", string.Empty));

			AssertNotEqualObject<GetDataResponse>();
		}

		[Test]
		public void GetKeysResponse()
		{
			TestEquality((a, b) => a == b, (a, b) => a != b,
				new GetKeysResponse(true, "message", new DataKey[0]),
				new GetKeysResponse(false, "message", new DataKey[0]),
				new GetKeysResponse(true, "message2", new DataKey[0]),
				new GetKeysResponse(true, "message", new[] { new DataKey("key") }),
				new GetKeysResponse(true, "message", new[] { new DataKey("key"), new DataKey("key2") }));

			AssertNotEqualObject<GetKeysResponse>();
		}

		[Test]
		public void UpdateDataResponse()
		{
			TestEquality((a, b) => a == b, (a, b) => a != b,
				new UpdateDataResponse(true, "message", "data"),
				new UpdateDataResponse(false, "message", "data"),
				new UpdateDataResponse(true, "message2", "data"),
				new UpdateDataResponse(true, "message", "data2"),
				new UpdateDataResponse(true, "message", null),
				new UpdateDataResponse(true, "message", string.Empty));

			AssertNotEqualObject<UpdateDataResponse>();
		}
	}
}