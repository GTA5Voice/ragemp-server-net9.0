using GTA5Voice.Definitions;
using GTA5Voice.Services;

namespace GTA5Voice.Voice.Models;

public class VoiceData(SettingsService settingsService)
{
    public string VirtualServerUid { get; set; } = settingsService.Get<string>(Settings.VirtualServerUid.Key);
    public int IngameChannelId { get; set; } = settingsService.Get<int>(Settings.IngameChannelId.Key);
    public string IngameChannelPassword { get; set; } = settingsService.Get<string>(Settings.IngameChannelPassword.Key);
    public int FallbackChannelId { get; set; } = settingsService.Get<int>(Settings.FallbackChannelId.Key);
    public string Language { get; set; } = settingsService.Get<string>(Settings.Language.Key);
    public int CalculationInterval { get; set; } = settingsService.Get<int>(Settings.CalculationInterval.Key);
    public string VoiceRanges { get; set; } = settingsService.Get<string>(Settings.VoiceRanges.Key);
    public string ExcludedChannels { get; set; } = settingsService.Get<string>(Settings.ExcludedChannels.Key);
}