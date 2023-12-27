using System;

namespace Hertzole.GameJolt
{
	public readonly struct GameJoltResult<T>
	{
		public bool HasError { get; }
		public Exception? Exception { get; }
		public T Value { get; }

		private GameJoltResult(Exception exception)
		{
			HasError = true;
			Exception = exception;
			Value = default;
		}

		private GameJoltResult(T value)
		{
			HasError = false;
			Exception = null;
			Value = value;
		}

		public static GameJoltResult<T> Error(Exception exception)
		{
			return new GameJoltResult<T>(exception);
		}

		public static GameJoltResult<T> Success(T result)
		{
			return new GameJoltResult<T>(result);
		}
	}

	public readonly struct GameJoltResult
	{
		public bool HasError { get; }
		public Exception? Exception { get; }

		private GameJoltResult(Exception exception)
		{
			HasError = true;
			Exception = exception;
		}

		private GameJoltResult(bool hasError)
		{
			HasError = hasError;
			Exception = null;
		}

		public static GameJoltResult Error(Exception exception)
		{
			return new GameJoltResult(exception);
		}

		public static GameJoltResult Success()
		{
			return new GameJoltResult(false);
		}
	}
}