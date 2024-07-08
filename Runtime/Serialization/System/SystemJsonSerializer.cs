#if !DISABLE_GAMEJOLT // Disables all GameJolt-related code

#if NET6_0_OR_GREATER || FORCE_SYSTEM_JSON
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hertzole.GameJolt
{
	[UnconditionalSuppressMessage("AOT", "IL3050:Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.",
		Justification = "Options are provided at runtime which includes (de)serializers for all types..")]
	[UnconditionalSuppressMessage("Trimming",
		"IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code",
		Justification = "Options are provided at runtime which includes (de)serializers for all types.")]
	internal sealed partial class SystemJsonSerializer : IGameJoltSerializer
	{
		private static readonly JsonSerializerOptions options = CreateOptions();

		[JsonSerializable(typeof(GameJoltResponse<GetDataResponse>))]
		[JsonSerializable(typeof(GameJoltResponse<GetKeysResponse>))]
		[JsonSerializable(typeof(GameJoltResponse<Response>))]
		[JsonSerializable(typeof(GameJoltResponse<UpdateDataResponse>))]
		[JsonSerializable(typeof(GameJoltResponse<FetchFriendsResponse>))]
		[JsonSerializable(typeof(GameJoltResponse<GetScoreRankResponse>))]
		[JsonSerializable(typeof(GameJoltResponse<GetScoresResponse>))]
		[JsonSerializable(typeof(GameJoltResponse<GetTablesResponse>))]
		[JsonSerializable(typeof(GameJoltResponse<FetchTimeResponse>))]
		[JsonSerializable(typeof(GameJoltResponse<FetchTrophiesResponse>))]
		[JsonSerializable(typeof(GameJoltResponse<UsersFetchResponse>))]
		[JsonSerializable(typeof(GameJoltResponse<FriendId>))]
		[JsonSerializable(typeof(GameJoltResponse<ScoreInternal>))]
		[JsonSerializable(typeof(GameJoltResponse<TableInternal>))]
		[JsonSerializable(typeof(GameJoltResponse<TrophyInternal>))]
		[JsonSerializable(typeof(GameJoltResponse<User>))]
		[ExcludeFromCodeCoverage]
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

		public string SerializeResponse<T>(T value)
		{
			return JsonSerializer.Serialize(new GameJoltResponse<T>(value), options);
		}

		public string Serialize<T>(T value)
		{
			return JsonSerializer.Serialize(value, options);
		}

		public T DeserializeResponse<T>(string value)
		{
			return JsonSerializer.Deserialize<GameJoltResponse<T>>(value, options).response;
		}

		public T Deserialize<T>(string value)
		{
			return JsonSerializer.Deserialize<T>(value, options);
		}
	}
}
#endif
#endif // DISABLE_GAMEJOLT