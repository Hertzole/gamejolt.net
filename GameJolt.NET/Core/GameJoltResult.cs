#nullable enable

using System;

namespace Hertzole.GameJolt
{
	/// <summary>
	///     A result that can either be a value or an exception.
	/// </summary>
	/// <typeparam name="T">The type of the value</typeparam>
	public readonly struct GameJoltResult<T>
	{
		/// <summary>
		///     Returns true if the result has an error.
		/// </summary>
		public bool HasError { get; }
		/// <summary>
		///     Returns the exception if the result has an error. Otherwise, returns null.
		/// </summary>
		public Exception? Exception { get; }
		/// <summary>
		///     Returns the value if the result has no error. Otherwise, returns the default value of {T}.
		/// </summary>
		public T? Value { get; }

		/// <summary>
		///     Error constructor.
		/// </summary>
		/// <param name="exception">The exception in the result.</param>
		private GameJoltResult(Exception exception)
		{
			HasError = true;
			Exception = exception;
			Value = default;
		}

		/// <summary>
		///     Success constructor.
		/// </summary>
		/// <param name="value">The value in the result</param>
		private GameJoltResult(T value)
		{
			HasError = false;
			Exception = null;
			Value = value;
		}

		/// <summary>
		///     Creates a new error result.
		/// </summary>
		/// <param name="exception">The exception in the result.</param>
		/// <returns>The error result.</returns>
		public static GameJoltResult<T> Error(Exception exception)
		{
			return new GameJoltResult<T>(exception);
		}

		/// <summary>
		///     Creates a new success result.
		/// </summary>
		/// <param name="result">The value in the result.</param>
		/// <returns>The success result.</returns>
		public static GameJoltResult<T> Success(T result)
		{
			return new GameJoltResult<T>(result);
		}
	}

	/// <summary>
	///     A result that can contain an exception.
	/// </summary>
	public readonly struct GameJoltResult
	{
		/// <summary>
		///     Returns true if the result has an error.
		/// </summary>
		public bool HasError { get; }
		/// <summary>
		///     Returns the exception if the result has an error. Otherwise, returns null.
		/// </summary>
		public Exception? Exception { get; }

		/// <summary>
		///     Error constructor.
		/// </summary>
		/// <param name="exception">The exception in the result.</param>
		private GameJoltResult(Exception exception)
		{
			HasError = true;
			Exception = exception;
		}

		/// <summary>
		///     Success constructor.
		/// </summary>
		/// <param name="hasError">True if the result has an error.</param>
		private GameJoltResult(bool hasError)
		{
			HasError = hasError;
			Exception = null;
		}

		/// <summary>
		///     Creates a new error result.
		/// </summary>
		/// <param name="exception">The exception in the result.</param>
		/// <returns>The error result.</returns>
		public static GameJoltResult Error(Exception exception)
		{
			return new GameJoltResult(exception);
		}

		/// <summary>
		///     Creates a new success result.
		/// </summary>
		/// <returns>The success result.</returns>
		public static GameJoltResult Success()
		{
			return new GameJoltResult(false);
		}
	}
}