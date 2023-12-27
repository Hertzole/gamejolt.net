using System;

namespace Hertzole.GameJolt
{
	internal static class QueryParser
	{
		public static bool TryGetToken(ReadOnlySpan<char> url, ReadOnlySpan<char> tokenName, out string? token)
		{
			// First check if the url even has a query.
			int queryIndex = url.IndexOfAny('?', '&');
			if (queryIndex == -1) // No query.
			{
				token = default;
				return false;
			}

			// Get the query part of the url.
			ReadOnlySpan<char> queryUrl = url.Slice(queryIndex + 1);

			// Check if the query contains the token.
			// If the token doesn't end with an equal sign, add it.
			Span<char> name = stackalloc char[tokenName.Length + (!tokenName.EndsWith("=", StringComparison.OrdinalIgnoreCase) ? 1 : 0)];
			tokenName.CopyTo(name);
			if (!tokenName.EndsWith("=", StringComparison.OrdinalIgnoreCase))
			{
				name[tokenName.Length] = '=';
			}

			// Get the index of the token.
			int tokenIndex = queryUrl.IndexOf(name);
			if (tokenIndex == -1) // Token not found.
			{
				token = default;
				return false;
			}

			tokenIndex += name.Length;

			// Get the rest of the query.
			ReadOnlySpan<char> theRest = queryUrl.Slice(tokenIndex);

			// Get the index of the next token.
			int tokenEnd = theRest.IndexOf('&');
			if (tokenEnd == -1) // No next token, return the token value.
			{
				token = new string(theRest);
				return true;
			}

			// Return the token value and remove the rest of the query.
			token = new string(theRest.Slice(0, tokenEnd));
			return true;
		}
	}
}