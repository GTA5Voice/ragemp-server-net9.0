using GTA5Voice.Extensions;
using GTA5Voice.Logging;
using GTA5Voice.Services;
using GTA5Voice.Voice.Services;
using GTANetworkAPI;

namespace GTA5Voice;

public class Main : Script
{
    private SettingsService _settingsService = null!;
    public static VoiceService VoiceService { get; } = new();
    public static PhoneService PhoneService { get; } = new();
    public static RadioService RadioService { get; } = new();

    [ServerEvent(Event.ResourceStart)]
    public void OnResourceStart()
    {
        _settingsService = new SettingsService(this);
        _settingsService.Initialize();

        ConsoleLogger.Configure(_settingsService);
    }

    [ServerEvent(Event.PlayerConnected)]
    public void OnPlayerConnected(Player player)
    {
        RunThreadSafe(() =>
        {
            var vsClient = VoiceService.AddClient(player);
            vsClient?.Initialize(_settingsService);
            VoiceService.LoadLocalClientData(player.Id);
            vsClient?.Start();
        });
    }

    [ServerEvent(Event.PlayerDeath)]
    public void OnPlayerDeath(Player player, Player killer, uint reason)
    {
        player.SetForceMuted(true);
    }

    [ServerEvent(Event.PlayerSpawn)]
    public void OnPlayerSpawn(Player player)
    {
        player.SetForceMuted(false);
    }

    [ServerEvent(Event.PlayerDisconnected)]
    public void OnPlayerDisconnected(Player player, DisconnectionType type, string reason)
    {
        RunThreadSafe(() =>
        {
            player.TriggerEvent("Client:GTA5Voice:OnPlayerDisconnected");
            VoiceService.RemoveClient(player);
            PhoneService.OnPlayerDisconnected(player);
            RadioService.OnPlayerDisconnected(player);
        });
    }
    
    public static void RunThreadSafe(Action action)
    {
        if (Thread.CurrentThread.ManagedThreadId == NAPI.MainThreadId)
            action();
        else
            NAPI.Task.Run(action);
    }
}