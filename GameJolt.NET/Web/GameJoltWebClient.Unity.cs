#if UNITY_2021_1_OR_NEWER
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Hertzole.GameJolt
{
	partial class GameJoltWebClient
	{
		private partial async Task<string> SendGetRequestAsync(string url, CancellationToken cancellationToken)
		{
			UnityWebRequest request = UnityWebRequest.Get(url);
			await request.SendWebRequest();
			cancellationToken.ThrowIfCancellationRequested();
			return Serializer.Deserialize<T>(request.downloadHandler.text);
		}
	}
}
#endif
