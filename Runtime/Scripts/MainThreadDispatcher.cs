using System;
using System.Collections.Concurrent;
using UnityEngine;

namespace Schmiggolas.ULogger
{
    /// <summary>
    /// Dispatches actions to be executed on the main Unity thread.
    /// Useful for calling Unity APIs from background threads.
    /// </summary>
    public class MainThreadDispatcher : MonoBehaviour
    {
        public static bool IsInitialized { get; private set; }
        private static readonly ConcurrentQueue<Action> _actions = new();
        private static readonly int _maxIterationsPerFrame = 10;

        private void Awake()
        {
            if (IsInitialized)
            {
                Destroy(gameObject);
                return;
            }
            IsInitialized = true;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            var iterations = 0;
            while (_actions.TryDequeue(out var action) && iterations < _maxIterationsPerFrame)
            {
                try
                {
                    action?.Invoke();
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex, this);
                }
                iterations++;
            }
        }

        /// <summary>
        /// Enqueues an action to be executed on the main thread.
        /// Thread-safe and can be called from any thread.
        /// </summary>
        /// <param name="action">The action to execute on the main thread</param>
        public static void Enqueue(Action action)
        {
            _actions.Enqueue(action);
        }
    }
}