#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

#nullable enable

using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hertzole.GameJolt
{
	/// <summary>
	///     Time is used to get the current time from the Game Jolt servers.
	/// </summary>
	public sealed class GameJoltTime
	{
		private readonly IGameJoltWebClient webClient;
		private readonly IGameJoltSerializer settings;

		/// <summary>
		///     The timezone of the Game Jolt servers.
		/// </summary>
		public TimeZoneInfo TimeZone { get; }

		internal GameJoltTime(IGameJoltWebClient webClient, IGameJoltSerializer settings)
		{
			this.webClient = webClient;
			this.settings = settings;
			TimeZone = GetTimeZone();
		}

		internal const string ENDPOINT = "time/";

		/// <summary>
		///     Fetches the current time from the Game Jolt servers.
		/// </summary>
		/// <param name="cancellationToken">Optional cancellation token for stopping this task.</param>
		/// <returns>The result of the request and the current server time.</returns>
		public async Task<GameJoltResult<DateTime>> GetTimeAsync(CancellationToken cancellationToken = default)
		{
			using (StringBuilderPool.Rent(out StringBuilder builder))
			{
				builder.Append(ENDPOINT);

				string json = await webClient.GetStringAsync(GameJoltUrlBuilder.BuildUrl(builder), cancellationToken);
				FetchTimeResponse response = settings.DeserializeResponse<FetchTimeResponse>(json);

				if (response.TryGetException(out Exception? exception))
				{
					return GameJoltResult<DateTime>.Error(exception!);
				}

				Debug.Assert(response.Success, "Response was successful, but success was false.");

				DateTime time = DateTimeHelper.FromUnixTimestamp(response.timestamp);
				return GameJoltResult<DateTime>.Success(time);
			}
		}

		private static TimeZoneInfo GetTimeZone()
		{
			// Because getting the timezone is finicky at best, let's just create it.
			return TimeZoneInfo.CreateCustomTimeZone("America/New_York", TimeSpan.FromHours(-5), "Eastern Standard Time", "Eastern Standard Time");
		}
	}
}
#endif // DISABLE_GAMEJOLT