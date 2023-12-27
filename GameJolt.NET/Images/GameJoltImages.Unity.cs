#if UNITY_2021_1_OR_NEWER
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
#endif

namespace Hertzole.GameJolt
{
	partial class GameJoltImages
	{
		internal static void ClearCache()
		{
#if UNITY_2021_1_OR_NEWER
			foreach (Texture2D value in avatarCache.Values)
			{
				if (value != null)
				{
					Object.Destroy(value);
				}
			}

			avatarCache.Clear();
#endif
		}

#if UNITY_2021_1_OR_NEWER
		private static readonly Dictionary<int, Texture2D> avatarCache = new Dictionary<int, Texture2D>();

		public static async Task<Texture2D> GetAvatarAsync(int userId, CancellationToken cancellationToken = default)
		{
			if (avatarCache.TryGetValue(userId, out Texture2D cached))
			{
				return cached;
			}

			UserFetchResponse fetchResponse = await GameJoltUsers.FetchUserAsync(userId, cancellationToken);
			if (!fetchResponse.Success || fetchResponse.User == null)
			{
				return null;
			}

			Texture2D texture = await WebHelper.GetImageAsync(fetchResponse.User.Value.AvatarUrl, cancellationToken);
			if (texture != null)
			{
				avatarCache.Add(userId, texture);
			}

			return texture;
		}
#endif
	}
}