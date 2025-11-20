#if GAMEJOLT_UNITY && !DISABLE_GAMEJOLT
#nullable enable

using System;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;
using UnityPlayerLoop = UnityEngine.LowLevel.PlayerLoop;

namespace Hertzole.GameJolt.PlayerLoop
{
    public struct GameJoltDecommissioning { }

    internal static class GameJoltPlayerLoop
    {
        private static bool isRegistered = false;
        private static bool isQuitting = false;
        private static bool hasDecommissioned = false;

        internal static event Action? OnDecommissioning;

        internal static void RegisterPlayerLoops()
        {
            isQuitting = false;
            hasDecommissioned = false;

            if (isRegistered)
            {
                return;
            }

            PlayerLoopSystem playerLoop = UnityPlayerLoop.GetCurrentPlayerLoop();

            InsertInto<PostLateUpdate>(ref playerLoop, new PlayerLoopSystem
            {
                type = typeof(GameJoltDecommissioning),
                updateDelegate = OnGameJoltDecommissioning
            });

            UnityPlayerLoop.SetPlayerLoop(playerLoop);

            isRegistered = true;

            Application.quitting += OnApplicationQuitting;
        }

        private static void OnApplicationQuitting()
        {
            isQuitting = true;
        }

        private static void OnGameJoltDecommissioning()
        {
            if (!isQuitting || hasDecommissioned)
            {
                return;
            }

            OnDecommissioning?.Invoke();
            hasDecommissioned = true;
        }

        private static void InsertInto<T>(ref PlayerLoopSystem parent, PlayerLoopSystem toInsert)
        {
            int index = -1;
            for (int i = 0; i < parent.subSystemList.Length; i++)
            {
                if (parent.subSystemList[i].type == typeof(T))
                {
                    index = i;
                    break;
                }
            }

            if (index == -1)
            {
                Debug.LogError($"Could not find {typeof(T)} in the player loop.");
                return;
            }

            PlayerLoopSystem[] list = new PlayerLoopSystem[parent.subSystemList.Length + 1];
            for (int i = 0, j = 0; i < list.Length; i++)
            {
                if (i == index)
                {
                    list[i] = toInsert;
                }
                else
                {
                    list[i] = parent.subSystemList[j];
                    j++;
                }
            }

            parent.subSystemList = list;
        }
    }
}
#endif // GAMEJOLT_UNITY && !DISABLE_GAMEJOLT