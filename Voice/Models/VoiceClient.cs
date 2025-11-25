using GTA5Voice.Services;
using GTANetworkAPI;

namespace GTA5Voice.Voice.Models;

public class VoiceClient(Player player, string teamspeakName)
{
    public Player Player { get; } = player;
    private PluginData? PluginData { get; set; }

    public void Initialize(SettingsService settingsService)
        => Player.TriggerEvent("Client:GTA5Voice:initialize", new VoiceData(settingsService), teamspeakName);

    public void Start()
        => Player.TriggerEvent("Client:GTA5Voice:connect");

    public void SetPluginData(PluginData pluginData, Action<int, PluginData> onDataChanged)
    {
        PluginData = pluginData;
        onDataChanged(Player.Id, PluginData);
    }

    public PluginData? GetPluginData()
        => PluginData;
}
