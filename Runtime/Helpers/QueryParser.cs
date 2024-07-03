#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
#define USE_SPANS
#endif
#nullable enable

using System;
#if USE_SPANS
using StringType = System.ReadOnlySpan<char>;
#else
using StringType = System.String;
#endif

namespace Hertzole.GameJolt
{
	internal static class QueryParser
	{
		public static bool TryGetToken(StringType url, StringType tokenName, out string? token)
		{
#if USE_SPANS
			return TryGetTokenSpans(url, tokenName, out token);
#else
			return TryGetTokenStrings(url, tokenName, out token);
#endif
		}

#if USE_SPANS
		private static bool TryGetTokenSpans(ReadOnlySpan<char> url, ReadOnlySpan<char> tokenName, out string? token)
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
			int tokenIndex = queryUrl.IndexOf(name, StringComparison.OrdinalIgnoreCase);
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
#else
		private static readonly char[] querySeparators = { '?', '&' };

		private static bool TryGetTokenStrings(string url, string tokenName, out string? token)
		{
			// First check if the url even has a query.
			int queryIndex = url.IndexOfAny(querySeparators);
			if (queryIndex == -1) // No query.
			{
				token = default;
				return false;
			}

			// Get the query part of the url.
			string queryUrl = url.Substring(queryIndex + 1);

			// Check if the query contains the token.
			// If the token doesn't end with an equal sign, add it.
			if (!tokenName.EndsWith("=", StringComparison.OrdinalIgnoreCase))
			{
				tokenName += '=';
			}

			// Get the index of the token.
			int tokenIndex = queryUrl.IndexOf(tokenName, StringComparison.OrdinalIgnoreCase);
			if (tokenIndex == -1) // Token not found.
			{
				token = default;
				return false;
			}

			tokenIndex += tokenName.Length;

			// Get the rest of the query.
			string theRest = queryUrl.Substring(tokenIndex);

			// Get the index of the next token.
			int tokenEnd = theRest.IndexOf('&');
			if (tokenEnd == -1) // No next token, return the token value.
			{
				token = theRest;
				return true;
			}

			// Return the token value and remove the rest of the query.
			token = theRest.Substring(0, tokenEnd);
			return true;
		}
#endif
	}
}
#endif // DISABLE_GAMEJOLT