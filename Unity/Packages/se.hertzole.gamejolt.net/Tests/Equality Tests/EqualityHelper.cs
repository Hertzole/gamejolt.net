using System;
using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests.Equality
{
	public sealed class EqualityHelper
	{
		[Test]
		public void Array_BothNull()
		{
			int[]? a = null;
			int[]? b = null;

			Assert.That(Hertzole.GameJolt.EqualityHelper.ArrayEquals(a, b), Is.True);
		}

		[Test]
		public void Array_OneNull([Values] bool flip)
		{
			int[]? a = null;
			int[] b = Array.Empty<int>();

			Assert.That(flip ? Hertzole.GameJolt.EqualityHelper.ArrayEquals(a, b) : Hertzole.GameJolt.EqualityHelper.ArrayEquals(b, a), Is.False);
		}

		[Test]
		public void Array_LengthMismatch()
		{
			int[] a = new int[5];
			int[] b = new int[10];

			Assert.That(Hertzole.GameJolt.EqualityHelper.ArrayEquals(a, b), Is.False);
		}

		[Test]
		public void Array_BothEmpty()
		{
			int[] a = Array.Empty<int>();
			int[] b = Array.Empty<int>();

			Assert.That(Hertzole.GameJolt.EqualityHelper.ArrayEquals(a, b), Is.True);
		}

		[Test]
		public void Array_BothSame()
		{
			int[] a = { 1, 2, 3, 4, 5 };
			int[] b = { 1, 2, 3, 4, 5 };

			Assert.That(Hertzole.GameJolt.EqualityHelper.ArrayEquals(a, b), Is.True);
		}

		[Test]
		public void Array_BothDifferent()
		{
			int[] a = { 1, 2, 3, 4, 5 };
			int[] b = { 5, 4, 3, 2, 1 };

			Assert.That(Hertzole.GameJolt.EqualityHelper.ArrayEquals(a, b), Is.False);
		}

		[Test]
		public void String_BothNull()
		{
			string? a = null;
			string? b = null;

			Assert.That(Hertzole.GameJolt.EqualityHelper.StringEquals(a, b), Is.True);
		}

		[Test]
		public void String_OneNull([Values] bool flip)
		{
			string? a = null;
			string b = string.Empty;

			Assert.That(flip ? Hertzole.GameJolt.EqualityHelper.StringEquals(a, b) : Hertzole.GameJolt.EqualityHelper.StringEquals(b, a), Is.False);
		}

		[Test]
		public void String_BothEmpty()
		{
			string a = string.Empty;
			string b = string.Empty;

			Assert.That(Hertzole.GameJolt.EqualityHelper.StringEquals(a, b), Is.True);
		}

		[Test]
		public void String_BothSame()
		{
			string a = "Hello, World!";
			string b = "Hello, World!";

			Assert.That(Hertzole.GameJolt.EqualityHelper.StringEquals(a, b), Is.True);
		}

		[Test]
		public void String_BothDifferent()
		{
			string a = "Hello, World!";
			string b = "Goodbye, World!";

			Assert.That(Hertzole.GameJolt.EqualityHelper.StringEquals(a, b), Is.False);
		}

		[Test]
		public void String_BothSame_WithCase()
		{
			string a = "Hello, World!";
			string b = "hello, world!";

			Assert.That(Hertzole.GameJolt.EqualityHelper.StringEquals(a, b), Is.False);
		}

		[Test]
		public void ResponseEquals_Same_Success()
		{
			TestResponse a = new TestResponse(true, "Hello, World!");
			TestResponse b = new TestResponse(true, "Hello, World!");

			Assert.That(Hertzole.GameJolt.EqualityHelper.ResponseEquals(a, b), Is.True);
		}

		[Test]
		public void ResponseEquals_DifferentSuccess_Fail()
		{
			TestResponse a = new TestResponse(true, "Hello, World!");
			TestResponse b = new TestResponse(false, "Hello, World!");

			Assert.That(Hertzole.GameJolt.EqualityHelper.ResponseEquals(a, b), Is.False);
		}

		[Test]
		public void ResponseEquals_DifferentMessage_Fail()
		{
			TestResponse a = new TestResponse(true, "Hello, World!");
			TestResponse b = new TestResponse(true, "Goodbye, World!");

			Assert.That(Hertzole.GameJolt.EqualityHelper.ResponseEquals(a, b), Is.False);
		}

		[Test]
		public void ResponseEquals_BothNullMessage_Success()
		{
			TestResponse a = new TestResponse(true, null);
			TestResponse b = new TestResponse(true, null);

			Assert.That(Hertzole.GameJolt.EqualityHelper.ResponseEquals(a, b), Is.True);
		}

		[Test]
		public void ResponseEquals_OneNullMessage_Fail([Values] bool flip)
		{
			TestResponse a = new TestResponse(true, null);
			TestResponse b = new TestResponse(true, "Hello, World!");

			Assert.That(flip ? Hertzole.GameJolt.EqualityHelper.ResponseEquals(a, b) : Hertzole.GameJolt.EqualityHelper.ResponseEquals(b, a), Is.False);
		}

		[Test]
		public void ResponseHashCode_Same_Success()
		{
			TestResponse a = new TestResponse(true, "Hello, World!");
			TestResponse b = new TestResponse(true, "Hello, World!");

			Assert.That(Hertzole.GameJolt.EqualityHelper.ResponseHashCode(0, a), Is.EqualTo(Hertzole.GameJolt.EqualityHelper.ResponseHashCode(0, b)));
		}

		[Test]
		public void ResponseHashCode_DifferentSuccess_Fail()
		{
			TestResponse a = new TestResponse(true, "Hello, World!");
			TestResponse b = new TestResponse(false, "Hello, World!");

			Assert.That(Hertzole.GameJolt.EqualityHelper.ResponseHashCode(0, a), Is.Not.EqualTo(Hertzole.GameJolt.EqualityHelper.ResponseHashCode(0, b)));
		}

		[Test]
		public void ResponseHashCode_DifferentMessage_Fail()
		{
			TestResponse a = new TestResponse(true, "Hello, World!");
			TestResponse b = new TestResponse(true, "Goodbye, World!");

			Assert.That(Hertzole.GameJolt.EqualityHelper.ResponseHashCode(0, a), Is.Not.EqualTo(Hertzole.GameJolt.EqualityHelper.ResponseHashCode(0, b)));
		}

		[Test]
		public void ResponseHashCode_BothNullMessage_Success()
		{
			TestResponse a = new TestResponse(true, null);
			TestResponse b = new TestResponse(true, null);

			Assert.That(Hertzole.GameJolt.EqualityHelper.ResponseHashCode(0, a), Is.EqualTo(Hertzole.GameJolt.EqualityHelper.ResponseHashCode(0, b)));
		}

		[Test]
		public void ResponseHashCode_OneNullMessage_Fail()
		{
			TestResponse a = new TestResponse(true, null);
			TestResponse b = new TestResponse(true, "Hello, World!");

			Assert.That(Hertzole.GameJolt.EqualityHelper.ResponseHashCode(0, a), Is.Not.EqualTo(Hertzole.GameJolt.EqualityHelper.ResponseHashCode(0, b)));
		}

		private struct TestResponse : IResponse
		{
			public bool Success { get; }
			public string? Message { get; }

			public TestResponse(bool success, string? message)
			{
				Success = success;
				Message = message;
			}
		}
	}
}