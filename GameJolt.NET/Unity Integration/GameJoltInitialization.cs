#if GAMEJOLT_UNITY && !DISABLE_GAMEJOLT
using System;
using System.Threading;
using Hertzole.GameJolt.PlayerLoop;
using UnityEngine;

namespace Hertzole.GameJolt
{
    internal static class GameJoltInitialization
    {
        internal static GameJoltManager manager;

        internal static CancellationTokenSource exitTokenSource;

        private static readonly Action onQuit = OnQuit;

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

            GameJoltPlayerLoop.RegisterPlayerLoops();
            GameJoltPlayerLoop.OnDecommissioning -= onQuit;
            GameJoltPlayerLoop.OnDecommissioning += onQuit;

            exitTokenSource = new CancellationTokenSource();

            manager = new GameJoltManager(exitTokenSource.Token);

            InitializeGameJolt();
        }

        private static void InitializeGameJolt()
        {
            manager.InitializeGameJolt();
        }

        private static void OnQuit()
        {
            GameJoltPlayerLoop.OnDecommissioning -= onQuit;

            exitTokenSource.Cancel();
            manager.Dispose();
            manager = null;
        }
    }
}
#endif // GAMEJOLT_UNITY && !DISABLE_GAMEJOLT