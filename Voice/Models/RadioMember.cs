using GTANetworkAPI;

namespace GTA5Voice.Voice.Models;

public class RadioMember(Player player, bool isTalking = false)
{
    public Player Player { get; private set; } = player;
    public bool IsTalking { get; set; } = isTalking;
}