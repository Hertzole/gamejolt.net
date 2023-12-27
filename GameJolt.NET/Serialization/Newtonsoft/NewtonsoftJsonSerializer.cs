#if !NET6_0_OR_GREATER
using Newtonsoft.Json;

namespace Hertzole.GameJolt
{
	internal sealed class NewtonsoftJsonSerializer : IGameJoltSerializer
	{
		private static readonly JsonConverter[] converters =
		{
			new GameJoltUserConverter(),
			new AuthResponseConverter(),
			new UserFetchResponseConverter(),
			new UsersFetchResponseConverter(),
			new SessionOpenResponseConverter(),
			new SessionCloseResponseConverter(),
			new SessionPingResponseConverter(),
			new SessionCheckResponseConverter(),
			new GameJoltLeaderboardConverter(),
			new GameJoltScoreConverter(),
			new SubmitScoresResponseConverter(),
			new GetScoreRankResponseConverter(),
			new GetLeaderboardsResponseConverter(),
			new GetScoresResponseConverter()
		};

		private static readonly JsonSerializerSettings settings = new JsonSerializerSettings
		{
			Converters = converters
		};

		public T Deserialize<T>(string value)
		{
			return JsonConvert.DeserializeObject<T>(value, settings);
		}
	}
}
#endif