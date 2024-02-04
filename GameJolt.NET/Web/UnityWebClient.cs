#if UNITY_2021_1_OR_NEWER

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
			return await GetStringAsyncCompletionSource(url, cancellationToken).ConfigureAwait(false);
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
				if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
				{
					tcs.SetException(new GameJoltException(request.error));
				}
				else
				{
					tcs.SetResult(request.downloadHandler.text);
				}
			};

			cancellationToken.Register(() =>
			{
				if (!tcs.Task.IsCompleted)
				{
#if NET5_0_OR_GREATER
					tcs.SetCanceled(cancellationToken);
#else
					tcs.SetCanceled();
#endif
				}
			});

			return await tcs.Task.ConfigureAwait(false);
		}
#else
		private static async Awaitable<string> GetStringAsyncAwaitable(string url, CancellationToken cancellationToken)
		{
			UnityWebRequest request = UnityWebRequest.Get(url);

			await request.SendWebRequest();
			
			cancellationToken.ThrowIfCancellationRequested();
			
			if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
			{
				throw new GameJoltException(request.error);
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