#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

#if UNITY_2021_1_OR_NEWER
using System.Net.Http;
using System.Threading;
using UnityEngine.Networking;
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER || UNITY_2021_3_OR_NEWER
using StringTask = System.Threading.Tasks.ValueTask<string>;
#else
using StringTask = System.Threading.Tasks.Task<string>;
#endif
#if !UNITY_2023_1_OR_NEWER
using System.Threading.Tasks;
#else
using UnityEngine;
#endif

namespace Hertzole.GameJolt
{
	internal sealed class UnityWebClient : IGameJoltWebClient
	{
		public async StringTask GetStringAsync(string url, CancellationToken cancellationToken)
		{
#if UNITY_2023_1_OR_NEWER
			return await GetStringAsyncAwaitable(url, cancellationToken);
#else
			return await GetStringAsyncCompletionSource(url, cancellationToken);
#endif
		}

#if !UNITY_2023_1_OR_NEWER
		private static async StringTask GetStringAsyncCompletionSource(string url, CancellationToken cancellationToken)
		{
			TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();

			UnityWebRequest request = UnityWebRequest.Get(url);
			UnityWebRequestAsyncOperation operation = request.SendWebRequest();
			
			operation.completed += _ =>
			{
				if (cancellationToken.IsCancellationRequested)
				{
					tcs.SetCanceled();
					return;
				}
				
				if (request.result != UnityWebRequest.Result.Success)
				{
					tcs.SetException(new HttpRequestException($"The request to '{url}' failed with status code {request.responseCode}."));
					return;
				}
				
				tcs.SetResult(request.downloadHandler.text);
			};

			return await tcs.Task;
		}
#else
		private static async Awaitable<string> GetStringAsyncAwaitable(string url, CancellationToken cancellationToken)
		{
			await Awaitable.MainThreadAsync();
			
			UnityWebRequest request = UnityWebRequest.Get(url);

			await request.SendWebRequest();
			
			cancellationToken.ThrowIfCancellationRequested();

			if (request.result != UnityWebRequest.Result.Success)
			{
				throw new HttpRequestException($"The request to '{url}' failed with status code {request.responseCode}.");
			}
			
			return request.downloadHandler.text;
		}
#endif

		public void Dispose()
		{
			// Unity doesn't have anything to dispose.
		}
	}
}
#endif
#endif // DISABLE_GAMEJOLT