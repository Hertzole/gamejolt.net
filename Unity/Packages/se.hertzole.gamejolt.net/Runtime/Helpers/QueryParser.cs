#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;

namespace Hertzole.GameJolt
{
	internal static class QueryParser
	{
		public static bool TryGetToken(ReadOnlySpan<char> url, ReadOnlySpan<char> tokenName, [NotNullWhen(true)] out string? token)
		{
			// First check if the url even has a query.
			int queryIndex = url.IndexOfAny('?', '&');
			if (queryIndex == -1) // No query.
			{
				token = null;
				return false;
			}

			// Get the query part of the url.
			ReadOnlySpan<char> queryUrl = url.Slice(queryIndex + 1);
			bool tokenNameEndsWithEquals = tokenName.EndsWith('=');

			// Check if the query contains the token.
			// If the token doesn't end with an equal sign, add it.
			Span<char> name = stackalloc char[tokenName.Length + (!tokenNameEndsWithEquals ? 1 : 0)];
			tokenName.CopyTo(name);
			if (!tokenNameEndsWithEquals)
			{
				name[tokenName.Length] = '=';
			}

			// Get the index of the token.
			int tokenIndex = queryUrl.IndexOf(name, StringComparison.OrdinalIgnoreCase);
			if (tokenIndex == -1) // Token not found.
			{
				token = null;
				return false;
			}

			tokenIndex += name.Length;

			// Get the rest of the query.
			ReadOnlySpan<char> theRest = queryUrl.Slice(tokenIndex);

			// Get the index of the next token.
			int tokenEnd = theRest.IndexOf('&');
			if (tokenEnd == -1) // No next token, return the token value.
			{
				token = theRest.ToString();
				return true;
			}

			// Return the token value and remove the rest of the query.
			token = theRest.Slice(0, tokenEnd).ToString();
			return true;
		}
	}
}
#endif // DISABLE_GAMEJOLT