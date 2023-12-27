using System;
using System.Threading;
using System.Threading.Tasks;

namespace Hertzole.GameJolt
{
	internal interface IGameJoltWebClient : IDisposable
	{
		Task<string> GetStringAsync(string url, CancellationToken cancellationToken);
	}
}