using GTANetworkAPI;
using GTA5Voice.Definitions;
using GTA5Voice.Logging;

namespace GTA5Voice.Services;

public class SettingsService(Script script)
{
    private Script Script { get; } = script;
    private readonly Dictionary<string, string?> _settings = new();

    public void Initialize()
    {
        foreach (var setting in GetAllDefinedSettings())
        {
            var value = NAPI.Resource.GetSetting<string>(Script, setting.Key);

            if (string.IsNullOrWhiteSpace(value))
            {
                if (setting.Required && !setting.HasDefaultValue)
                {
                    ConsoleLogger.Warning($"Missing or empty required setting: '{setting.Key}'");
                    ConsoleLogger.Error("No default value found. Please update your meta.xml");
                    Console.ReadKey();
                    Environment.Exit(1);
                }

                if (setting.HasDefaultValue)
                {
                    value = setting.DefaultValue?.ToString();
                    ConsoleLogger.Info($"Setting '{setting.Key}' not found, using default: {value}");
                }
            }
            else
            {
                ConsoleLogger.Info($"Setting '{setting.Key}' set to: {value}");
            }

            _settings[setting.Key] = value;
        }
    }

    public string? Get(string key)
        => _settings.TryGetValue(key, out var value) ? value : null;

    public T Get<T>(string key, T defaultValue = default!)
    {
        if (!_settings.TryGetValue(key, out var value) || string.IsNullOrWhiteSpace(value))
            return defaultValue;

        try { return (T)Convert.ChangeType(value, typeof(T)); }
        catch { return defaultValue; }
    }

    private static IEnumerable<ISetting> GetAllDefinedSettings()
    {
        yield return Settings.VirtualServerUid;
        yield return Settings.IngameChannelId;
        yield return Settings.DevChannelId;
        yield return Settings.FallbackChannelId;
        yield return Settings.IngameChannelPassword;
        yield return Settings.DevChannelPassword;
        yield return Settings.DebuggingEnabled;
        yield return Settings.Language;
        yield return Settings.CalculationInterval;
        yield return Settings.VoiceRanges;
        yield return Settings.ExcludedChannels;
    }
}