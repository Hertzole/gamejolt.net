﻿#if UNITY_2021_1_OR_NEWER
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER || UNITY_2021_3_OR_NEWER
using StringTask = System.Threading.Tasks.ValueTask<string>;
#else
using StringTask = System.Threading.Tasks.Task<string>;
#endif
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace Hertzole.GameJolt
{
	internal sealed class UnityWebClient : IGameJoltWebClient
	{
		public async StringTask GetStringAsync(string url, CancellationToken cancellationToken)
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

		public void Dispose()
		{
			// Unity doesn't have anything to dispose.
		}
	}
}
#endif