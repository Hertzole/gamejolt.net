#nullable enable

using System;

namespace Hertzole.GameJolt
{
	/// <summary>
	///     The main class for the Game Jolt API where you can access all the API features.
	/// </summary>
	public static class GameJoltAPI
	{
		private static GameJoltUsers? users;
		private static GameJoltSessions? sessions;
		private static GameJoltScores? scores;
		private static GameJoltTrophies? trophies;
		private static GameJoltDataStore? dataStore;
		private static GameJoltFriends? friends;
		private static GameJoltTime? time;

		internal static readonly IGameJoltSerializer serializer = GetSerializer();
		internal static IGameJoltWebClient webClient = GetWebClient();

		internal static int GameId { get; private set; }
		internal static string? PrivateKey { get; private set; }

		/// <summary>
		///     Access user-based features.
		/// </summary>
		/// <exception cref="GameJoltInitializationException">Thrown if the API has not been initialized.</exception>
		public static GameJoltUsers Users
		{
			get
			{
				ThrowIfNotInitialized();

				return users!;
			}
		}

		/// <summary>
		///     Set up sessions for your game.
		/// </summary>
		/// <exception cref="GameJoltInitializationException">Thrown if the API has not been initialized.</exception>
		public static GameJoltSessions Sessions
		{
			get
			{
				ThrowIfNotInitialized();

				return sessions ??= new GameJoltSessions(webClient, serializer, users!);
			}
		}

		/// <summary>
		///     Manipulate scores on score tables.
		/// </summary>
		/// <exception cref="GameJoltInitializationException">Thrown if the API has not been initialized.</exception>
		public static GameJoltScores Scores
		{
			get
			{
				ThrowIfNotInitialized();

				return scores ??= new GameJoltScores(webClient, serializer, users!);
			}
		}

		/// <summary>
		///     Manage trophies for your game.
		/// </summary>
		/// <exception cref="GameJoltInitializationException">Thrown if the API has not been initialized.</exception>
		public static GameJoltTrophies Trophies
		{
			get
			{
				ThrowIfNotInitialized();

				return trophies ??= new GameJoltTrophies(webClient, serializer, users!);
			}
		}

		/// <summary>
		///     Manipulate items in a cloud-based data storage.
		/// </summary>
		/// <exception cref="GameJoltInitializationException">Thrown if the API has not been initialized.</exception>
		public static GameJoltDataStore DataStore
		{
			get
			{
				ThrowIfNotInitialized();

				return dataStore ??= new GameJoltDataStore(webClient, serializer, users!);
			}
		}

		/// <summary>
		///     List a user's friends.
		/// </summary>
		/// <exception cref="GameJoltInitializationException">Thrown if the API has not been initialized.</exception>
		public static GameJoltFriends Friends
		{
			get
			{
				ThrowIfNotInitialized();

				return friends ??= new GameJoltFriends(webClient, serializer, users!);
			}
		}

		/// <summary>
		///     Get the server's time.
		/// </summary>
		/// <exception cref="GameJoltInitializationException">Thrown if the API has not been initialized.</exception>
		public static GameJoltTime Time
		{
			get
			{
				ThrowIfNotInitialized();

				return time ??= new GameJoltTime(webClient, serializer);
			}
		}

		/// <summary>
		///     Has the API been initialized?
		/// </summary>
		public static bool IsInitialized { get; private set; }

		/// <summary>
		///     Event that is invoked when the API has been initialized with a game ID and private key.
		/// </summary>
		public static event Action? OnInitialized;
		/// <summary>
		///     Event that is invoked when the API is shutting down.
		/// </summary>
		/// <remarks>
		///     The API will still be accessible when this event is invoked and <see cref="IsInitialized" /> will still be true,
		///     unlike <see cref="OnShutdownComplete" />.
		/// </remarks>
		public static event Action? OnShutdown;
		/// <summary>
		///     Event that is invoked when the API has been completely shutdown.
		/// </summary>
		/// <remarks>
		///     All the API features will be null after this event has been invoked and <see cref="IsInitialized" /> will be false.
		/// </remarks>
		public static event Action? OnShutdownComplete;

		/// <summary>
		///     Initializes the API.
		/// </summary>
		/// <param name="gameId">The ID for your game.</param>
		/// <param name="privateKey">The private key for your game.</param>
		public static void Initialize(int gameId, string privateKey)
		{
			GameId = gameId;
			PrivateKey = privateKey;

			users = new GameJoltUsers(webClient, serializer);

			IsInitialized = true;
			OnInitialized?.Invoke();
		}

		/// <summary>
		///     Shuts down the API.
		/// </summary>
		public static void Shutdown()
		{
			ThrowIfNotInitialized();

			// Call the event before cleaning up so that the event can still access the API.
			OnShutdown?.Invoke();

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
			OnShutdownComplete?.Invoke();
		}

		/// <summary>
		///     Throws an exception if the API has not been initialized.
		/// </summary>
		/// <exception cref="GameJoltInitializationException">Thrown if the API has not been initialized.</exception>
		private static void ThrowIfNotInitialized()
		{
			if (!IsInitialized)
			{
				throw new GameJoltInitializationException();
			}
		}

		internal static IGameJoltSerializer GetSerializer()
		{
#if NET6_0_OR_GREATER
			return new SystemJsonSerializer();
#else
			return new NewtonsoftJsonSerializer();
#endif
		}
		
		internal static IGameJoltWebClient GetWebClient()
		{
#if UNITY_2021_1_OR_NEWER
			return new UnityWebClient();
#else
			return new StandardWebClient();
#endif
		}
	}
}