#if NETSTANDARD2_1_OR_GREATER || UNITY_2021_3_OR_NEWER || NET5_0_OR_GREATER
#define NULLABLE_ATTRIBUTES
#endif

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

			if (response.Message.StartsWith("No item with that key could be found.", StringComparison.OrdinalIgnoreCase) ||
			    response.Message.StartsWith("There is no item with the key passed in:", StringComparison.OrdinalIgnoreCase))
			{
				exception = new GameJoltInvalidDataStoreKeyException(response.Message);
				return true;
			}

			switch (response.Message)
			{
				case GameJoltAuthenticationException.MESSAGE:
					exception = new GameJoltAuthenticationException();
					return true;
				case GameJoltInvalidGameException.MESSAGE:
					exception = new GameJoltInvalidGameException();
					return true;
				case GameJoltInvalidUserException.MESSAGE:
					exception = new GameJoltInvalidUserException();
					return true;
				case GameJoltInvalidTrophyException.MESSAGE:
					exception = new GameJoltInvalidTrophyException();
					return true;
				case "Incorrect trophy ID.":
					exception = new GameJoltInvalidTrophyException("Incorrect trophy ID.");
					return true;
				case GameJoltLockedTrophyException.MESSAGE:
					exception = new GameJoltLockedTrophyException();
					return true;
				case "Mathematical operations require the pre-existing data stored to also be numeric.":
					exception = new GameJoltInvalidDataStoreValueException("Mathematical operations require the pre-existing data stored to also be numeric.");
					return true;
				default:
					exception = new GameJoltException(response.Message);
					return true;
			}
		}
	}
}