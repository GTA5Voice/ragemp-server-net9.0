using System.Text.Json;
using System.Text.Json.Serialization;
using GTA5Voice.Extensions;
using GTA5Voice.Logging;
using GTA5Voice.Voice.Models;
using GTA5Voice.Voice.Services;
using GTANetworkAPI;

namespace GTA5Voice.Voice.Events;

public class VoiceEvents : Script
{
    [RemoteEvent("Server:GTA5Voice:OnTalkingStateChanged")]
    public void OnTalkingStateChanged(Player player, bool talking)
        => NAPI.Task.Run(() => NAPI.ClientEvent.TriggerClientEventInRange(player.Position, 50f, "Client:GTA5Voice:SyncTalkingState", player, talking));

    [RemoteEvent("Server:GTA5Voice:OnTeamspeakDataChanged")]
    public void OnTeamspeakDataChanged(Player player, string pluginData)
        => NAPI.Task.Run(() => player.ToVoiceClient()?.SetPluginData(JsonSerializer.Deserialize<PluginData>(pluginData)!, Main.VoiceService.UpdateLocalClientData));
}