using GTA5Voice.Extensions;
using GTA5Voice.Logging;
using GTANetworkAPI;

namespace GTA5Voice.Voice.Models;

public class RadioChannel
{
    public string Frequency { get; }
    private List<RadioMember> RadioMembers { get; } = [];
    
    public RadioChannel(string frequency, Player player)
    {
        Frequency = frequency;
        AddToRadioChannel(player);
    }

    public void AddToRadioChannel(Player player)
    {
        var member = GetPlayerInRadio(player);
        if (member != null)
            return;

        player.TriggerEvent("Client:GTA5Voice:EnterRadio", Frequency);
        player.SetData("RadioFrequency", Frequency);
        RadioMembers.Add(new RadioMember(player));
        UpdateRadioData();
    }

    public void RemoveFromRadioChannel(Player player)
    {
        var member = GetPlayerInRadio(player);
        if (member == null)
            return;

        RadioMembers.Remove(member);
        player.TriggerEvent("Client:GTA5Voice:LeaveRadio", Frequency);
        player.ResetData("RadioFrequency");
        UpdateRadioData();
    }

    private RadioMember? GetPlayerInRadio(Player player)
        => RadioMembers.FirstOrDefault(r => r.Player == player);
    
    public bool IsPlayerInRadio(Player player)
        => RadioMembers.Any(r => r.Player == player);

    public void SetTalkingState(Player player, bool talking)
    {
        var p = GetPlayerInRadio(player);
        if (p == null)
            return;
        p.IsTalking = talking;
        UpdateRadioData();
    }

    private void UpdateRadioData()
    {
        foreach (var p in RadioMembers)
            NAPI.ClientEvent.TriggerClientEvent(
                p.Player,
                "Client:GTA5Voice:UpdateRadioMembers",
                GetRadioMemberIds().Where(id => id != p.Player.Id && p.IsTalking).ToArray()
            );
    }
    
    public RadioMember[] GetRadioMembers()
        => RadioMembers.ToArray();
    
    public ushort[] GetRadioMemberIds()
        => RadioMembers.Select(x => x.Player.Id).ToArray();

    public int GetRadioMemberCount()
        => RadioMembers.Count;
}