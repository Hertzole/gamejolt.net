#if NET6_0_OR_GREATER
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hertzole.GameJolt
{
	internal sealed partial class SystemJsonSerializer : IGameJoltSerializer
	{
		private static readonly JsonSerializerOptions options = CreateOptions();

		[JsonSerializable(typeof(GameJoltResponse<GetDataResponse>))]
		[JsonSerializable(typeof(GameJoltResponse<GetKeysResponse>))]
		[JsonSerializable(typeof(GameJoltResponse<StoreDataResponse>))]
		[JsonSerializable(typeof(GameJoltResponse<UpdateDataResponse>))]
		[JsonSerializable(typeof(GameJoltResponse<FetchFriendsResponse>))]
		[JsonSerializable(typeof(GameJoltResponse<GetScoreRankResponse>))]
		[JsonSerializable(typeof(GameJoltResponse<GetScoresResponse>))]
		[JsonSerializable(typeof(GameJoltResponse<GetTablesResponse>))]
		[JsonSerializable(typeof(GameJoltResponse<SubmitScoreResponse>))]
		[JsonSerializable(typeof(GameJoltResponse<SessionResponse>))]
		[JsonSerializable(typeof(GameJoltResponse<FetchTimeResponse>))]
		[JsonSerializable(typeof(GameJoltResponse<FetchTrophiesResponse>))]
		[JsonSerializable(typeof(GameJoltResponse<TrophyResponse>))]
		[JsonSerializable(typeof(GameJoltResponse<AuthResponse>))]
		[JsonSerializable(typeof(GameJoltResponse<UsersFetchResponse>))]
		[JsonSerializable(typeof(GameJoltResponse<FriendId>))]
		[JsonSerializable(typeof(GameJoltResponse<ScoreInternal>))]
		[JsonSerializable(typeof(GameJoltResponse<TableInternal>))]
		[JsonSerializable(typeof(GameJoltResponse<TrophyInternal>))]
		[JsonSerializable(typeof(GameJoltResponse<User>))]
		public sealed partial class JsonContext : JsonSerializerContext { }

		private static JsonSerializerOptions CreateOptions()
		{
			JsonSerializerOptions o = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
				IncludeFields = true
			};
#if NET8_0_OR_GREATER
			o.TypeInfoResolverChain.Insert(0, JsonContext.Default);
#else
			o.AddContext<JsonContext>();
#endif
			return o;
		}

		public string Serialize<T>(T value)
		{
			return JsonSerializer.Serialize(new GameJoltResponse<T>(value), options);
		}

		public T Deserialize<T>(string value)
		{
			return JsonSerializer.Deserialize<GameJoltResponse<T>>(value, options).response;
		}
	}
}
#endif