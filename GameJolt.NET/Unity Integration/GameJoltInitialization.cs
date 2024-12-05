#if UNITY_64 && !DISABLE_GAMEJOLT
using System;
using System.Threading;
using UnityEngine;
#if UNITY_INCLUDE_TESTS
using NUnit.Framework;
using NUnit.Framework.Internal;
#endif // UNITY_INCLUDE_TESTS

namespace Hertzole.GameJolt
{
	internal static class GameJoltInitialization
	{
		internal static GameJoltManager manager;
		private static readonly Action onQuitting = OnApplicationQuit;

		internal static CancellationTokenSource exitTokenSource;

#if UNITY_INCLUDE_TESTS
		internal static bool ReturnIfTestIsRunning { get; set; } = true;
#endif

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void Initialize()
		{
#if UNITY_INCLUDE_TESTS && GAMEJOLT_NET_TESTS_DONT_RUN_INITIALIZATION
			// Don't run this if we're running tests as it will interfere with the tests.
			if (ReturnIfTestIsRunning)
			{
				return;
			}
#endif // UNITY_INCLUDE_TESTS

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