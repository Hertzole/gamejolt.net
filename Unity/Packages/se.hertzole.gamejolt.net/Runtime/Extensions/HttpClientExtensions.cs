#if !UNITY_2021_1_OR_NEWER && !NET5_0_OR_GREATER
#nullable enable
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Hertzole.GameJolt
{
	internal static class HttpClientExtensions
	{
		public static Task<string?> GetStringAsync(this HttpClient client, string url, CancellationToken cancellationToken)
		{
			return client.GetStringAsync(url);
		}
	}
}
#endif