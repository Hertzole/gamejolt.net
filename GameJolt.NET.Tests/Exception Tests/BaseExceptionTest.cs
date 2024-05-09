#nullable enable
using System;
using Hertzole.GameJolt;
using NUnit.Framework;

namespace GameJolt.NET.Tests.Exceptions
{
	public abstract class BaseExceptionTest
	{
		protected static void AssertError<T>(string message) where T : Exception
		{
			Response response = new Response(false, message);
			bool hasError = response.TryGetException(out Exception? exception);
			
			Assert.That(hasError, Is.True);
			Assert.That(exception, Is.Not.Null);
			Assert.That(exception, Is.TypeOf<T>());
		}
	}
}