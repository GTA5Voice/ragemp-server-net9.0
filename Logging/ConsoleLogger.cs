using System;
using GTANetworkAPI;
using GTA5Voice.Definitions;
using GTA5Voice.Services;

namespace GTA5Voice.Logging;

public static class ConsoleLogger
{
    private const string TagBase = "GTA5Voice";
    private static SettingsService? _settingsService;

    public static void Configure(SettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    public static void Info(string message, string type = "Server")
        => Write(message, type, ConsoleColor.Cyan);

    public static void Success(string message, string type = "Server")
        => Write(message, type, ConsoleColor.Green);

    public static void Warning(string message, string type = "Server")
        => Write(message, type, ConsoleColor.Yellow);
    
    public static void Error(string message, string type = "Server")
        => Write(message, type, ConsoleColor.Red);

    public static void Debug(string message, string type = "Server")
    {
        var debuggingEnabled = _settingsService?.Get(Settings.DebuggingEnabled.Key, false) ?? false;
        if (!debuggingEnabled)
            return;

        Write(message, type, ConsoleColor.DarkGray);
    }
    
    private static void Write(string message, string type, ConsoleColor tagColor)
    {
        var timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
        var tag = $"[{TagBase}-{type}] ";

        Console.ForegroundColor = tagColor;
        Console.Write(tag);
        Console.ResetColor();

        NAPI.Util.ConsoleOutput($"{timestamp} | {message}");
    }
}
