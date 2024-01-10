#nullable enable

namespace Hertzole.GameJolt
{
	public static class GameJoltAPI
	{
		private static GameJoltUsers? users;
		private static GameJoltSessions? sessions;
		private static GameJoltScores? scores;
		private static GameJoltTrophies? trophies;
		private static GameJoltDataStore? dataStore;
		private static GameJoltFriends? friends;
		private static GameJoltTime? time;

		internal static int GameId { get; private set; }
		internal static string? PrivateKey { get; private set; }

		internal static readonly IGameJoltSerializer serializer =
#if NET6_0_OR_GREATER
			new SystemJsonSerializer();
#else
			new NewtonsoftJsonSerializer();
#endif
		internal static IGameJoltWebClient webClient =
#if UNITY_2021_1_OR_NEWER
			new UnityWebClient();
#else
			new StandardWebClient();
#endif

		public static GameJoltUsers Users
		{
			get
			{
				ThrowIfNotInitialized();

				return users!;
			}
		}

		public static GameJoltSessions Sessions
		{
			get
			{
				ThrowIfNotInitialized();

				return sessions ??= new GameJoltSessions(webClient, serializer, users!);
			}
		}

		public static GameJoltScores Scores
		{
			get
			{
				ThrowIfNotInitialized();

				return scores ??= new GameJoltScores(webClient, serializer, users!);
			}
		}

		public static GameJoltTrophies Trophies
		{
			get
			{
				ThrowIfNotInitialized();

				return trophies ??= new GameJoltTrophies(webClient, serializer, users!);
			}
		}

		public static GameJoltDataStore DataStore
		{
			get
			{
				ThrowIfNotInitialized();

				return dataStore ??= new GameJoltDataStore(webClient, serializer, users!);
			}
		}

		public static GameJoltFriends Friends
		{
			get
			{
				ThrowIfNotInitialized();

				return friends ??= new GameJoltFriends(webClient, serializer, users!);
			}
		}

		public static GameJoltTime Time
		{
			get
			{
				ThrowIfNotInitialized();

				return time ??= new GameJoltTime(webClient, serializer);
			}
		}

		public static bool IsInitialized { get; private set; }

		public static void Initialize(int gameId, string privateKey)
		{
			GameId = gameId;
			PrivateKey = privateKey;

			users = new GameJoltUsers(webClient, serializer);

			IsInitialized = true;
		}

		public static void Shutdown()
		{
			ThrowIfNotInitialized();

			users!.Shutdown();

			webClient.Dispose();

			users = null;
			sessions = null;
			scores = null;
			trophies = null;
			dataStore = null;
			friends = null;
			time = null;

			IsInitialized = false;
		}

		private static void ThrowIfNotInitialized()
		{
			if (!IsInitialized)
			{
				throw new GameJoltInitializationException();
			}
		}
	}
}