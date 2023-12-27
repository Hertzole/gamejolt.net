#if !UNITY_2021_1_OR_NEWER
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Hertzole.GameJolt
{
	internal partial class GameJoltWebClient
	{
		private readonly HttpClient client = new HttpClient();

		private partial async Task<string> SendGetRequestAsync(string url, CancellationToken cancellationToken)
		{
			string? response = await client.GetStringAsync(url, cancellationToken);
			return response;
		}
	}
}
#endif