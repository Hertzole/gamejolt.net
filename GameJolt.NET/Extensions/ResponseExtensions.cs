#if NETSTANDARD2_1_OR_GREATER || UNITY_2021_3_OR_NEWER || NET5_0_OR_GREATER
#define NULLABLE_ATTRIBUTES
#endif
#nullable enable

using System;
#if NULLABLE_ATTRIBUTES
using System.Diagnostics.CodeAnalysis;
#endif

namespace Hertzole.GameJolt
{
	internal static class ResponseExtensions
	{
		public static bool TryGetException<T>(this T response,
#if NULLABLE_ATTRIBUTES
			[NotNullWhen(true)]
#endif
			out Exception? exception) where T : struct, IResponse
		{
			exception = null;

			if (response.Success || string.IsNullOrEmpty(response.Message))
			{
				return false;
			}

			string message = response.Message!;

			if (TryGetDataStoreException(message, out exception))
			{
				return true;
			}

			if (TryGetScoresException(message, out exception))
			{
				return true;
			}

			if (TryGetSessionsException(message, out exception))
			{
				return true;
			}

			if (TryGetTrophiesException(message, out exception))
			{
				return true;
			}

			if (TryGetUsersException(message, out exception))
			{
				return true;
			}
			
			if (TryGetGlobalException(message, out exception))
			{
				return true;
			}

			exception = new GameJoltException(response.Message);
			return true;
		}

		private static bool TryGetDataStoreException(string message,
#if NULLABLE_ATTRIBUTES
			[NotNullWhen(true)]
#endif
			out Exception? exception)
		{
			if (message.Equals("You must enter the key for the item you would like to retrieve data for.", StringComparison.OrdinalIgnoreCase) ||
			    message.Equals("No item with that key could be found.", StringComparison.OrdinalIgnoreCase))
			{
				exception = new GameJoltInvalidDataStoreKeyException(message);
				return true;
			}

			if (message.Equals("You must enter an value with the request.", StringComparison.OrdinalIgnoreCase) ||
			    message.Equals("You must enter data with the request.", StringComparison.OrdinalIgnoreCase) ||
			    message.Equals("Mathematical operations require the pre-existing data stored to also be numeric.", StringComparison.OrdinalIgnoreCase) ||
			    message.Equals("Value must be numeric if operation is mathematical.", StringComparison.OrdinalIgnoreCase) ||
			    message.Equals("GAME JOLT STOP: 0x00000019 (0x00000000, 0xC00E0FF0, 0xFFFFEFD4, 0xC0000000) UNIVERSAL_COLLAPSE",
				    StringComparison.OrdinalIgnoreCase))
			{
				exception = new GameJoltInvalidDataStoreValueException(message);
				return true;
			}

			if (message.StartsWith("There is no item with the key passed in:", StringComparison.OrdinalIgnoreCase))
			{
				exception = new GameJoltInvalidDataStoreKeyException(message);
				return true;
			}

			exception = null;
			return false;
		}

		private static bool TryGetScoresException(string message,
#if NULLABLE_ATTRIBUTES
			[NotNullWhen(true)]
#endif
			out Exception? exception)
		{
			if (message.Equals(GameJoltInvalidTableException.MESSAGE, StringComparison.OrdinalIgnoreCase))
			{
				exception = new GameJoltInvalidTableException();
				return true;
			}

			if (message.Equals("Guests are not allowed to enter scores for this game.", StringComparison.OrdinalIgnoreCase))
			{
				exception = new GameJoltAuthorizedException(message);
				return true;
			}

			exception = null;
			return false;
		}

		private static bool TryGetSessionsException(string message,
#if NULLABLE_ATTRIBUTES
			[NotNullWhen(true)]
#endif
			out Exception? exception)
		{
			if (message.Equals(GameJoltSessionException.MESSAGE, StringComparison.OrdinalIgnoreCase))
			{
				exception = new GameJoltSessionException(message);
				return true;
			}

			exception = null;
			return false;
		}

		private static bool TryGetTrophiesException(string message,
#if NULLABLE_ATTRIBUTES
			[NotNullWhen(true)]
#endif
			out Exception? exception)
		{
			if (message.Equals(GameJoltInvalidTrophyException.DOES_NOT_BELONG_MESSAGE, StringComparison.OrdinalIgnoreCase) ||
			    message.Equals(GameJoltInvalidTrophyException.INCORRECT_ID_MESSAGE, StringComparison.OrdinalIgnoreCase))
			{
				exception = new GameJoltInvalidTrophyException(message);
				return true;
			}

			if (message.Equals(GameJoltTrophyException.ALREADY_UNLOCKED_MESSAGE, StringComparison.OrdinalIgnoreCase) ||
			    message.Equals(GameJoltTrophyException.DOES_NOT_HAVE_MESSAGE, StringComparison.OrdinalIgnoreCase))
			{
				exception = new GameJoltTrophyException(message);
				return true;
			}

			exception = null;
			return false;
		}

		private static bool TryGetUsersException(string message,
#if NULLABLE_ATTRIBUTES
			[NotNullWhen(true)]
#endif
			out Exception? exception)
		{
			if (message.Equals(GameJoltInvalidUserException.MESSAGE, StringComparison.OrdinalIgnoreCase))
			{
				exception = new GameJoltInvalidUserException();
				return true;
			}

			if (message.Equals(GameJoltAuthenticationException.MESSAGE, StringComparison.OrdinalIgnoreCase))
			{
				exception = new GameJoltAuthenticationException();
				return true;
			}

			exception = null;
			return false;
		}
		
		private static bool TryGetGlobalException(string message,
#if NULLABLE_ATTRIBUTES
			[NotNullWhen(true)]
#endif
			out Exception? exception)
		{
			if (message.Equals(GameJoltInvalidGameException.MESSAGE, StringComparison.OrdinalIgnoreCase))
			{
				exception = new GameJoltInvalidGameException();
				return true;
			}

			exception = null;
			return false;
		}
	}
}