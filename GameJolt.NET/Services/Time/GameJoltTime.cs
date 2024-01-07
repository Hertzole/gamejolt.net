
#nullable enable

using System;
using System.Collections.ObjectModel;
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

		private const string ENDPOINT = "time/";

		public async Task<GameJoltResult<DateTime>> GetTimeAsync(CancellationToken cancellationToken = default)
		{
			string json = await webClient.GetStringAsync(ENDPOINT, cancellationToken).ConfigureAwait(false);
			FetchTimeResponse response = settings.Deserialize<FetchTimeResponse>(json);

			if (response.TryGetException(out Exception? exception))
			{
				return GameJoltResult<DateTime>.Error(exception);
			}

			Debug.Assert(response.Success, "Response was successful, but success was false.");

			DateTime time = DateTimeHelper.FromUnixTimestamp(response.timestamp);
			return GameJoltResult<DateTime>.Success(time);
		}

		private static TimeZoneInfo GetTimeZone()
		{
			try
			{
				return TimeZoneInfo.FindSystemTimeZoneById("America/New_York");
			}
			catch (TimeZoneNotFoundException e)
			{
				ReadOnlyCollection<TimeZoneInfo>? allZones = TimeZoneInfo.GetSystemTimeZones();
				
				foreach (TimeZoneInfo zone in allZones)
				{
					if (zone.Id.Contains("Eastern Standard Time"))
					{
						return zone;
					}
				}
				
				throw new TimeZoneNotFoundException("Could not find the New York time zone.", e);
			}
		}
	}
}