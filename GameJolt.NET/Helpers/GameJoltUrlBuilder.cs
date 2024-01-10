using System.Security.Cryptography;
using System.Text;

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

			return builder.ToString();
		}

		private static bool EndsWithSlash(StringBuilder sb)
		{
			return sb.Length > 0 && sb[sb.Length - 1] == '/';
		}
	}
}