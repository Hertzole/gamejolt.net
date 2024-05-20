#nullable enable

using System;
#if NET6_0_OR_GREATER
using BaseConverter = System.Text.Json.Serialization.JsonConverter<bool>;
using JsonSerializationException = System.Text.Json.JsonException;
#else
using BaseConverter = Newtonsoft.Json.JsonConverter<bool>;
using JsonSerializationException = Newtonsoft.Json.JsonSerializationException;
#endif

namespace Hertzole.GameJolt.Serialization.Shared
{
	/// <summary>
	///     Base class for boolean converters. You never know what Game Jolt will throw at you.
	/// </summary>
	internal abstract class BaseBooleanConverter : BaseConverter
	{
		private static readonly string[] trueValues =
		{
			"true",
			"1",
			"yes"
		};

		private static readonly string[] falseValues =
		{
			"false",
			"0",
			"no"
		};
		
		protected const string INVALID_STRING = "Invalid string value. Expected 'true', 'false', '1', or '0'.";

		/// <summary>
		///     Reads a number and converts it to a boolean, 0 is false, 1 is true. Otherwise throws an exception.
		/// </summary>
		/// <param name="number">The number to convert.</param>
		/// <returns>True if the value is 1, false is it 0.</returns>
		/// <exception cref="JsonSerializationException">If the numbers is more not between the ranges of 0-1.</exception>
		protected static bool ReadNumber(long number)
		{
			switch (number)
			{
				case 0:
					return false;
				case 1:
					return true;
				default:
					throw new JsonSerializationException($"Can't convert to boolean from {number}");
			}
		}

		/// <summary>
		///     Reads a string and converts it to a boolean. Throws an exception if the value is not a valid boolean.
		/// </summary>
		/// <param name="value">The string to convert.</param>
		/// <param name="result">True if the string is a valid boolean, otherwise false.</param>
		/// <returns>True if the value is "true", false if the value is "false".</returns>
		/// <exception cref="JsonSerializationException">If the value is not a valid boolean.</exception>
		protected static bool TryReadString(string? value, out bool result)
		{
			// This should never happen, but just in case.
			if (string.IsNullOrEmpty(value))
			{
				throw new JsonSerializationException("Value cannot be null or empty.");
			}

			// Check if the value is a valid boolean.
			if (IsTrue(value!))
			{
				result = true;
				return true;
			}

			// Check if the value is a valid boolean.
			if (IsFalse(value!))
			{
				result = false;
				return true;
			}

			// If the value is not a valid boolean, throw an exception.
			result = default;
			return false;
		}

		private static bool IsTrue(string value)
		{
			// Loop through all the true values and check if the value is one of them.
			int count = trueValues.Length;
			for (int i = 0; i < count; i++)
			{
				if (value.Equals(trueValues[i], StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}

			return false;
		}

		private static bool IsFalse(string value)
		{
			// Loop through all the false values and check if the value is one of them.
			int count = falseValues.Length;
			for (int i = 0; i < count; i++)
			{
				if (value.Equals(falseValues[i], StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}

			return false;
		}
	}
}