#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

using System;
using System.Threading;
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER || UNITY_2021_3_OR_NEWER
using StringTask = System.Threading.Tasks.ValueTask<string>;
#else
using StringTask = System.Threading.Tasks.Task<string>;
#endif

namespace Hertzole.GameJolt
{
	internal interface IGameJoltWebClient : IDisposable
	{
		StringTask GetStringAsync(string url, CancellationToken cancellationToken);
	}
}
#endif // DISABLE_GAMEJOLT