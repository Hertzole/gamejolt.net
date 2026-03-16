using System;
using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests
{
	public class Base64Tests
	{
		[Test]
		public void ValidBase64_ReturnsTrue()
		{
			// Run this test many times to make sure all types of base64 strings work.
			const int iterations = 1000;

			for (int i = 0; i < iterations; i++)
			{
				// Arrange
				byte[] data = DummyData.Bytes(i + 1); // +1 to not be empty data.
				string base64 = Convert.ToBase64String(data);
				bool result = false;

				// Act
				result = Base64.TryConvertBase64ToBytes(base64, out MemoryOwner<byte> resultData);

				// Assert
				Assert.That(result, Is.True);
				Assert.That(resultData, Has.Length.EqualTo(data.Length));
				for (int j = 0; j < data.Length; j++)
				{
					Assert.That(resultData[j], Is.EqualTo(data[j]));
				}

				resultData.Dispose();
			}
		}

		[Test]
		public void InvalidBase64_ReturnsFalse()
		{
			// Arrange
			string base64 = "InvalidString";
			bool result = false;

			// Act
			result = Base64.TryConvertBase64ToBytes(base64, out MemoryOwner<byte> resultData);

			Assert.That(result, Is.False);
			Assert.That(resultData, Has.Length.EqualTo(0));
			resultData.Dispose();
		}

		[Test]
		public void EmptyBase64_ReturnsFalse()
		{
			// Arrange
			string base64 = "";
			bool result = false;

			// Act
			result = Base64.TryConvertBase64ToBytes(base64, out MemoryOwner<byte> resultData);

			// Assert
			Assert.That(result, Is.False);
			Assert.That(resultData, Has.Length.EqualTo(0));
			resultData.Dispose();
		}
	}
}