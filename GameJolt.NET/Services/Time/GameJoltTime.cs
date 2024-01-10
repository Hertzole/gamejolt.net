#nullable enable

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Hertzole.GameJolt
{
	public sealed class GameJoltTime
	{
		private readonly IGameJoltWebClient webClient;
		private readonly IGameJoltSerializer settings;

		public TimeZoneInfo TimeZone { get; }

		internal GameJoltTime(IGameJoltWebClient webClient, IGameJoltSerializer settings)
		{
			this.webClient = webClient;
			this.settings = settings;
			TimeZone = GetTimeZone();
		}

		internal const string ENDPOINT = "time/";

		public async Task<GameJoltResult<DateTime>> GetTimeAsync(CancellationToken cancellationToken = default)
		{
			string json = await webClient.GetStringAsync(GameJoltUrlBuilder.BASE_URL + ENDPOINT, cancellationToken).ConfigureAwait(false);
			FetchTimeResponse response = settings.Deserialize<FetchTimeResponse>(json);

			if (response.TryGetException(out Exception? exception))
			{
				return GameJoltResult<DateTime>.Error(exception!);
			}

			Debug.Assert(response.Success, "Response was successful, but success was false.");

			DateTime time = DateTimeHelper.FromUnixTimestamp(response.timestamp);
			return GameJoltResult<DateTime>.Success(time);
		}

		private static TimeZoneInfo GetTimeZone()
		{
			// Because getting the timezone is finicky at best, let's just create it.
			return TimeZoneInfo.CreateCustomTimeZone("America/New_York", TimeSpan.FromHours(-5), "Eastern Standard Time", "Eastern Standard Time");
		}
	}
}