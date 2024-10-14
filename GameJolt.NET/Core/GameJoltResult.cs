#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

#nullable enable

using System;
using System.Collections.Generic;
using System.Text;

namespace Hertzole.GameJolt
{
	/// <summary>
	///     A result that can either be a value or an exception.
	/// </summary>
	/// <typeparam name="T">The type of the value</typeparam>
	public readonly struct GameJoltResult<T> : IEquatable<GameJoltResult<T>>
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

		/// <summary>
		///     Determines whether the specified object instances are considered equal.
		/// </summary>
		/// <param name="other">The object to compare with the current instance.</param>
		/// <returns><c>true</c> if the objects are considered equal; otherwise, <c>false</c>.</returns>
		public bool Equals(GameJoltResult<T> other)
		{
			return HasError == other.HasError && Equals(Exception, other.Exception) && EqualityComparer<T?>.Default.Equals(Value, other.Value);
		}

		/// <summary>
		///     Determines whether the specified object instances are considered equal.
		/// </summary>
		/// <param name="obj">The object to compare with the current instance.</param>
		/// <returns><c>true</c> if the objects are considered equal; otherwise, <c>false</c>.</returns>
		public override bool Equals(object? obj)
		{
			return obj is GameJoltResult<T> other && Equals(other);
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = HasError.GetHashCode();
				hashCode = (hashCode * 397) ^ (Exception != null ? Exception.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (Value != null ? EqualityComparer<T>.Default.GetHashCode(Value) : 0);
				return hashCode;
			}
		}

		/// <summary>
		///     Determines whether two specified instances of <see cref="GameJoltResult{T}" /> are equal.
		/// </summary>
		/// <param name="left">The first <see cref="GameJoltResult{T}" /> to compare.</param>
		/// <param name="right">The second <see cref="GameJoltResult{T}" /> to compare.</param>
		/// <returns>
		///     <c>true</c> if <paramref name="left" /> and <paramref name="right" /> represent the same result; otherwise,
		///     <c>false</c>.
		/// </returns>
		public static bool operator ==(GameJoltResult<T> left, GameJoltResult<T> right)
		{
			return left.Equals(right);
		}

		/// <summary>
		///     Determines whether two specified instances of <see cref="GameJoltResult{T}" /> are not equal.
		/// </summary>
		/// <param name="left">The first <see cref="GameJoltResult{T}" /> to compare.</param>
		/// <param name="right">The second <see cref="GameJoltResult{T}" /> to compare.</param>
		/// <returns>
		///     <c>true</c> if <paramref name="left" /> and <paramref name="right" /> do not represent the same result;
		///     otherwise, <c>false</c>.
		/// </returns>
		public static bool operator !=(GameJoltResult<T> left, GameJoltResult<T> right)
		{
			return !left.Equals(right);
		}

		/// <summary>
		///     Returns a string that represents the current object.
		/// </summary>
		/// <returns>A string that represents the current object.</returns>
		public override string ToString()
		{
			using (StringBuilderPool.Rent(out StringBuilder? builder))
			{
				builder.Append(nameof(GameJoltResult<T>) + " (" + nameof(HasError) + ": ");
				builder.Append(HasError);
				builder.Append(", " + nameof(Exception) + ": ");
				builder.Append(Exception);
				builder.Append(", " + nameof(Value) + ": ");
				builder.Append(Value);
				builder.Append(')');

				return builder.ToString();
			}
		}
	}

	/// <summary>
	///     A result that can contain an exception.
	/// </summary>
	public readonly struct GameJoltResult : IEquatable<GameJoltResult>
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

		/// <summary>
		///     Determines whether the specified object instances are considered equal.
		/// </summary>
		/// <param name="other">The object to compare with the current instance.</param>
		/// <returns><c>true</c> if the objects are considered equal; otherwise, <c>false</c>.</returns>
		public bool Equals(GameJoltResult other)
		{
			return HasError == other.HasError && Equals(Exception, other.Exception);
		}

		/// <summary>
		///     Determines whether the specified object instances are considered equal.
		/// </summary>
		/// <param name="obj">The object to compare with the current instance.</param>
		/// <returns><c>true</c> if the objects are considered equal; otherwise, <c>false</c>.</returns>
		public override bool Equals(object? obj)
		{
			return obj is GameJoltResult other && Equals(other);
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			unchecked
			{
				return (HasError.GetHashCode() * 397) ^ (Exception != null ? Exception.GetHashCode() : 0);
			}
		}

		/// <summary>
		///     Determines whether two specified instances of <see cref="GameJoltResult{T}" /> are equal.
		/// </summary>
		/// <param name="left">The first <see cref="GameJoltResult{T}" /> to compare.</param>
		/// <param name="right">The second <see cref="GameJoltResult{T}" /> to compare.</param>
		/// <returns>
		///     <c>true</c> if <paramref name="left" /> and <paramref name="right" /> represent the same result; otherwise,
		///     <c>false</c>.
		/// </returns>
		public static bool operator ==(GameJoltResult left, GameJoltResult right)
		{
			return left.Equals(right);
		}

		/// <summary>
		///     Determines whether two specified instances of <see cref="GameJoltResult{T}" /> are not equal.
		/// </summary>
		/// <param name="left">The first <see cref="GameJoltResult{T}" /> to compare.</param>
		/// <param name="right">The second <see cref="GameJoltResult{T}" /> to compare.</param>
		/// <returns>
		///     <c>true</c> if <paramref name="left" /> and <paramref name="right" /> do not represent the same result;
		///     otherwise, <c>false</c>.
		/// </returns>
		public static bool operator !=(GameJoltResult left, GameJoltResult right)
		{
			return !left.Equals(right);
		}

		/// <summary>
		///     Returns a string that represents the current object.
		/// </summary>
		/// <returns>A string that represents the current object.</returns>
		public override string ToString()
		{
			using (StringBuilderPool.Rent(out StringBuilder? builder))
			{
				builder.Append(nameof(GameJoltResult) + " (" + nameof(HasError) + ": ");
				builder.Append(HasError);
				builder.Append(", " + nameof(Exception) + ": ");
				builder.Append(Exception);
				builder.Append(')');

				return builder.ToString();
			}
		}
	}
}
#endif // DISABLE_GAMEJOLT