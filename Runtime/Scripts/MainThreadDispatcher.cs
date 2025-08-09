using System;
using System.Collections.Concurrent;
using UnityEngine;

namespace Schmiggolas.ULogger
{
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

        public static void Enqueue(Action action)
        {
            _actions.Enqueue(action);
        }

        private void Update()
        {
            var iterations = 0;
            while (_actions.TryDequeue(out var action) && iterations < _maxIterationsPerFrame)
            {
                action?.Invoke();
                iterations++;
            }
        }
    }
}