using GTA5Voice.Voice.Models;
using GTA5Voice.Voice.Services;
using GTANetworkAPI;

namespace GTA5Voice.Extensions;

public static class PlayerExtension
{
    public static VoiceClient? ToVoiceClient(this Player player)
        => VoiceService.FindClient(player) ?? null;

    public static PhoneCall? GetCurrentCall(this Player player)
        => Main.PhoneService.GetCall(player.GetCurrentCallId()!);

    public static string? GetCurrentCallId(this Player player)
        => player.GetData<string>("CurrentCall") ?? null;

    public static bool IsInCall(this Player player)
        => player.GetCurrentCall() != null;
    
    public static string GetRadioFrequency(this Player player)
        => player.GetData<string>("RadioFrequency")!;
    
    public static bool HasRadioFrequency(this Player player)
        => player.HasData("RadioFrequency");

    public static RadioChannel? GetRadioChannel(this Player player)
        => Main.RadioService.GetRadioChannel(player.GetRadioFrequency());

    public static void SetForceMuted(this Player player, bool forceMuted)
        => Main.VoiceService.SetForceMuted(player, forceMuted);
    
    public static void SetPhoneSpeakerEnabled(this Player player, bool phoneSpeakerEnabled)
        => Main.VoiceService.SetPhoneSpeakerEnabled(player, phoneSpeakerEnabled);

    public static void MoveToVoiceChannel(this Player player)
        => player.ToVoiceClient()?.Start();
}
