#if !UNITY_2021_1_OR_NEWER

using System.Net.Http;
using System.Threading;
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
using StringTask = System.Threading.Tasks.ValueTask<string>;
#else
using StringTask = System.Threading.Tasks.Task<string>;
#endif

namespace Hertzole.GameJolt
{
	internal sealed class StandardWebClient : IGameJoltWebClient
	{
		private readonly HttpClient client = new HttpClient();

		public async StringTask GetStringAsync(string url, CancellationToken cancellationToken)
		{
			HttpResponseMessage response = await client.GetAsync(url, cancellationToken);

			if (!response.IsSuccessStatusCode)
			{
				throw new HttpRequestException($"The request to '{url}' failed with status code {response.StatusCode}.");
			}

#if NET5_0_OR_GREATER
			return await response.Content.ReadAsStringAsync(cancellationToken);
#else
			return await response.Content.ReadAsStringAsync();
#endif
		}

		public void Dispose()
		{
			client.Dispose();
		}
	}
}
#endif