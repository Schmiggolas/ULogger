using Schmiggolas.ULogger;
using UnityEngine;

public static class ULogger
{
    public static LogLevel LogLevel { get; set; } = LogLevel.Debug;

    static ULogger()
    {
        if (Application.isPlaying)
        {
            
        }
    }

    public static bool IsLogLevelEnabled(LogLevel level)
    {
        return level <= LogLevel;
    }

    private static string FormatMessage(object message, object context)
    {
        var messageStr = message as string ?? message?.ToString() ?? "- no message provided -";
        return context != null ? $"[{context.GetType().Name}] {messageStr}" : messageStr;
    }

    public static void Log(object message, LogLevel level, object context = null)
    {
        if (!IsLogLevelEnabled(level))
        {
            return;
        }

        string formattedMessage = FormatMessage(message, context);
        Object unityContext = context as Object;

        switch (level)
        {
            case LogLevel.Error:
                MainThreadDispatcher.Enqueue(() => Debug.LogError(formattedMessage, unityContext));
                break;
            case LogLevel.Warning:
                MainThreadDispatcher.Enqueue(() => Debug.LogWarning(formattedMessage, unityContext));
                break;
            case LogLevel.Info:
            case LogLevel.Debug:
            default:
                MainThreadDispatcher.Enqueue(() => Debug.Log(formattedMessage, unityContext));
                break;
        }
    }

    public static void LogError(object message, object context = null)
    {
        Log(message, LogLevel.Error, context);
    }

    public static void LogWarning(object message, object context = null)
    {
        Log(message, LogLevel.Warning, context);
    }

    public static void LogInfo(object message, object context = null)
    {
        Log(message, LogLevel.Info, context);
    }

    public static void LogDebug(object message, object context = null)
    {
        Log(message, LogLevel.Debug, context);
    }
}
