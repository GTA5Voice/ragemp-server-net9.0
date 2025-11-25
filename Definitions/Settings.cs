namespace GTA5Voice.Definitions;

internal static class Settings
{
    internal static readonly Setting<string> VirtualServerUid = new("VirtualServerUID", true);
    internal static readonly Setting<int> IngameChannelId = new("IngameChannelId", true);
    internal static readonly Setting<int> DevChannelId = new("DevChannelId");
    internal static readonly Setting<int> FallbackChannelId = new("FallbackChannelId");
    internal static readonly Setting<string> IngameChannelPassword = new("IngameChannelPassword", true);
    internal static readonly Setting<string> DevChannelPassword = new("DevChannelPassword");
    internal static readonly Setting<bool> DebuggingEnabled = new("DebuggingEnabled");
    internal static readonly Setting<string> Language = new("Language", true);
    internal static readonly Setting<int> CalculationInterval = new("CalculationInterval", defaultValue: 3000);
    internal static readonly Setting<string> VoiceRanges = new("VoiceRanges", defaultValue: "[1, 3, 8, 15]");
    internal static readonly Setting<string> ExcludedChannels = new("ExcludedChannels");
}