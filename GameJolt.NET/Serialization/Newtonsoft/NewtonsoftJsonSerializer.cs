#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

#if !NET6_0_OR_GREATER && !FORCE_SYSTEM_JSON
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
			new GameJoltResponseConverter<Response>(),
			new ResponseConverter(),
			new DataKeyConverter(),
			new GetDataResponseConverter(),
			new GameJoltResponseConverter<GetDataResponse>(),
			new GetKeysResponseConverter(),
			new GameJoltResponseConverter<GetKeysResponse>(),
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
			new TableInternalConverter(),
			new FetchTimeResponseConverter(),
			new GameJoltResponseConverter<FetchTimeResponse>(),
			new FetchTrophiesResponseConverter(),
			new GameJoltResponseConverter<FetchTrophiesResponse>(),
			new TrophyInternalConverter(),
			new UserConverter(),
			new UsersFetchResponseConverter(),
			new GameJoltResponseConverter<UsersFetchResponse>()
		};

		private static readonly JsonSerializerSettings settings = new JsonSerializerSettings
		{
			Converters = converters,
			StringEscapeHandling = StringEscapeHandling.EscapeHtml
		};

		public string SerializeResponse<T>(T value)
		{
			return JsonConvert.SerializeObject(new GameJoltResponse<T>(value), typeof(GameJoltResponse<T>), settings);
		}

		public string Serialize<T>(T value)
		{
			return JsonConvert.SerializeObject(value, typeof(T), settings);
		}

		public T DeserializeResponse<T>(string value)
		{
			return JsonConvert.DeserializeObject<GameJoltResponse<T>>(value, settings).response;
		}

		public T Deserialize<T>(string value)
		{
			return JsonConvert.DeserializeObject<T>(value, settings);
		}
	}
}
#endif
#endif // DISABLE_GAMEJOLT