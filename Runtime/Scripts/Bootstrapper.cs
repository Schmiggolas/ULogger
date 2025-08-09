using UnityEngine;

namespace Schmiggolas.ULogger
{
    public static class Bootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Initialize()
        {
            if (!MainThreadDispatcher.IsInitialized)
            {
                var gameObject = new GameObject("MainThreadDispatcher");
                gameObject.AddComponent<MainThreadDispatcher>();
            }
        }
    }
}