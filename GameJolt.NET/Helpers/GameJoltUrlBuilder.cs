#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

using System.Security.Cryptography;
using System.Text;
#if NET5_0_OR_GREATER
using System;
#endif

namespace Hertzole.GameJolt
{
	internal static class GameJoltUrlBuilder
	{
		internal const string BASE_URL = "https://api.gamejolt.com/api/game/v1_2/";

		public static string BuildUrl(StringBuilder builder)
		{
			builder.Insert(0, BASE_URL);
			builder.Append(EndsWithSlash(builder) ? '?' : '&');
			builder.Append("game_id=");
			builder.Append(GameJoltAPI.GameId);
			builder.Append("&format=json");

			string baseString = builder.ToString();

			builder.Append(GameJoltAPI.PrivateKey);

			AppendHash(builder, baseString);

			return builder.ToString();
		}

		private static void AppendHash(StringBuilder builder, string baseString)
		{
#if NET5_0_OR_GREATER
			AppendHashSpan(builder, baseString);
#else
			AppendHashString(builder, baseString);
#endif
		}

#if NET5_0_OR_GREATER
		private static void AppendHashSpan(StringBuilder builder, ReadOnlySpan<char> baseString)
		{
			Span<byte> hash = stackalloc byte[16];
			Span<byte> bytes = stackalloc byte[builder.Length];
			Encoding.UTF8.GetBytes(builder.ToString(), bytes);
			int hashCount = MD5.HashData(bytes, hash);
			builder.Clear();

			builder.Append(baseString);
			builder.Append("&signature=");

			for (int i = 0; i < hashCount; i++)
			{
				builder.Append(hash[i].ToString("x2"));
			}
		}
#else
		private static void AppendHashString(StringBuilder builder, string baseString)
		{
			using (MD5 md5 = MD5.Create())
			{
				byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(builder.ToString()));
				builder.Clear();

				builder.Append(baseString);
				builder.Append("&signature=");

				for (int i = 0; i < hash.Length; i++)
				{
					builder.Append(hash[i].ToString("x2"));
				}
			}
		}
#endif

		internal static bool EndsWithSlash(StringBuilder sb)
		{
			return sb.Length > 0 && sb[sb.Length - 1] == '/';
		}
	}
}
#endif // DISABLE_GAMEJOLT