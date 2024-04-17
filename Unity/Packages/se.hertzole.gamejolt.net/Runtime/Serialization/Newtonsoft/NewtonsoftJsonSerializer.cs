#if !NET6_0_OR_GREATER
using Hertzole.GameJolt.Serialization.Newtonsoft;
using Hertzole.GameJolt.Serialization.Shared;
using Newtonsoft.Json;

namespace Hertzole.GameJolt
{
	internal sealed class NewtonsoftJsonSerializer : IGameJoltSerializer
	{
		private static readonly JsonConverter[] converters =
		{
			BooleanOrDateConverter.Instance,
			GameJoltBooleanConverter.Instance,
			GameJoltIntConverter.Instance,
			GameJoltLongConverter.Instance,
			StringOrNumberConverter.Instance,
			GameJoltStatusConverter.Instance,
			GameJoltTrophyDifficultyConverter.Instance,
			GameJoltUserTypeConverter.Instance,
			new DataKeyConverter(),
			new GetDataResponseConverter(),
			new GameJoltResponseConverter<GetDataResponse>(),
			new GetKeysResponseConverter(),
			new GameJoltResponseConverter<GetKeysResponse>(),
			new StoreDataResponseConverter(),
			new GameJoltResponseConverter<StoreDataResponse>(),
			new UpdateDataResponseConverter(),
			new GameJoltResponseConverter<UpdateDataResponse>(),
			new FetchFriendsResponseConverter(),
			new GameJoltResponseConverter<FetchFriendsResponse>(),
			new FriendIdConverter(),
			new GetScoreRankResponseConverter(),
			new GameJoltResponseConverter<GetScoreRankResponse>(),
			new GetScoresResponseConverter(),
			new GameJoltResponseConverter<GetScoresResponse>(),
			new GetTablesResponseConverter(),
			new GameJoltResponseConverter<GetTablesResponse>(),
			new ScoreInternalConverter(),
			new SubmitScoreResponseConverter(),
			new GameJoltResponseConverter<SubmitScoreResponse>(),
			new TableInternalConverter(),
			new SessionResponseConverter(),
			new GameJoltResponseConverter<SessionResponse>(),
			new FetchTimeResponseConverter(),
			new GameJoltResponseConverter<FetchTimeResponse>(),
			new FetchTrophiesResponseConverter(),
			new GameJoltResponseConverter<FetchTrophiesResponse>(),
			new TrophyInternalConverter(),
			new TrophyResponseConverter(),
			new GameJoltResponseConverter<TrophyResponse>(),
			new AuthResponseConverter(),
			new GameJoltResponseConverter<AuthResponse>(),
			new UserConverter(),
			new UsersFetchResponseConverter(),
			new GameJoltResponseConverter<UsersFetchResponse>()
		};

		private static readonly JsonSerializerSettings settings = new JsonSerializerSettings
		{
			Converters = converters
		};

		public string Serialize<T>(T value)
		{
			return JsonConvert.SerializeObject(new GameJoltResponse<T>(value), typeof(GameJoltResponse<T>), settings);
		}

		public T Deserialize<T>(string value)
		{
			return JsonConvert.DeserializeObject<GameJoltResponse<T>>(value, settings).response;
		}
	}
}
#endif