using GTA5Voice.Extensions;
using GTANetworkAPI;

namespace GTA5Voice.Voice.Events;

public class RadioEvents : Script
{
    [RemoteEvent("Server:GTA5Voice:OnRadioPTTChanged")]
    public void OnRadioPTTChanged(Player player, bool talking)
    {
        NAPI.Task.Run(() =>
        {
            player.GetRadioChannel()?.SetTalkingState(player, talking);
        });
    }
}