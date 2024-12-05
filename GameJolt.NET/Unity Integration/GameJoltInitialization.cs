#if UNITY_64 && !DISABLE_GAMEJOLT
#if GAMEJOLT_UNITY_TESTS && UNITY_INCLUDE_TESTS
#define HAS_TESTS
#endif // GAMEJOLT_UNITY_TESTS && DEBUG
using System;
using System.Threading;
using UnityEngine;
#if HAS_TESTS
using NUnit.Framework;
using NUnit.Framework.Internal;
#endif // HAS_TESTS

namespace Hertzole.GameJolt
{
	internal static class GameJoltInitialization
	{
		internal static GameJoltManager manager;
		private static readonly Action onQuitting = OnApplicationQuit;

		internal static CancellationTokenSource exitTokenSource;

#if HAS_TESTS
		internal static bool ReturnIfTestIsRunning { get; set; } = true;
#endif

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void Initialize()
		{
#if HAS_TESTS && GAMEJOLT_NET_TESTS_DONT_RUN_INITIALIZATION
			// Don't run this if we're running tests as it will interfere with the tests.
			if (ReturnIfTestIsRunning)
			{
				return;
			}
#endif // HAS_TESTS

			exitTokenSource = new CancellationTokenSource();

			manager = new GameJoltManager(exitTokenSource.Token);

			Application.quitting -= onQuitting;
			Application.quitting += onQuitting;

			InitializeGameJolt();
		}

		private static void InitializeGameJolt()
		{
			manager.InitializeGameJolt();
		}

		private static void OnApplicationQuit()
		{
			Application.quitting -= onQuitting;

			exitTokenSource.Cancel();
			manager.Dispose();
			manager = null;
		}
	}
}
#endif // UNITY_64 && !DISABLE_GAMEJOLT