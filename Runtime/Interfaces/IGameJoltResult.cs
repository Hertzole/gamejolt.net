#nullable enable

using System;

namespace Hertzole.GameJolt
{
	/// <summary>
	///     A result that can either be an error or not. This is used for methods that don't return a value but can still fail.
	/// </summary>
	public interface IGameJoltResult
	{
		/// <summary>
		///     Returns <see langword="true" /> if the result has an error; otherwise <see langword="false" />.
		/// </summary>
		bool HasError { get; }
		/// <summary>
		///     If <see cref="HasError" /> is <see langword="true" />, returns the result exception; otherwise returns
		///     <see langword="null" />.
		/// </summary>
		Exception? Exception { get; }
	}

	/// <summary>
	///     A result that can either be a value or an error. This is used for methods that return a value but can still fail.
	/// </summary>
	/// <typeparam name="T">The type of the value in the result.</typeparam>
	public interface IGameJoltResult<out T> : IGameJoltResult
	{
		/// <summary>
		///     If <see cref="IGameJoltResult.HasError" /> is <see langword="false" />, returns the result value; otherwise returns
		///     the default value of <typeparamref name="T" />.
		/// </summary>
		T? Value { get; }
	}
}