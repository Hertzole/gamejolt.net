#if !UNITY_2021_1_OR_NEWER
#nullable enable

using System.Net.Http;
using System.Threading;
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER || UNITY_2021_3_OR_NEWER
using StringTask = System.Threading.Tasks.ValueTask<string>;
#else
using StringTask = System.Threading.Tasks.Task<string>;
#endif

namespace Hertzole.GameJolt
{
	internal partial class GameJoltWebClient
	{
		private readonly HttpClient client = new HttpClient();

		private partial async StringTask SendGetRequestAsync(string url, CancellationToken cancellationToken)
		{
			string? response = await client.GetStringAsync(url, cancellationToken).ConfigureAwait(false);
			if (string.IsNullOrEmpty(response))
			{
				throw new GameJoltException("Response was empty.");
			}

			return response!;
		}
	}
}
#endif