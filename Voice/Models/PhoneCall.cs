using GTA5Voice.Extensions;
using GTANetworkAPI;

namespace GTA5Voice.Voice.Models;

public class PhoneCall
{
    public string Identifier { get; } = Guid.NewGuid().ToString("N");
    private Player CallOwner { get; }
    private List<Player> CallMembers { get; }

    public PhoneCall(Player owner, Player target)
    {
        CallOwner = owner;
        CallMembers = [owner, target];

        owner.SetData("CurrentCall", Identifier);
        target.SetData("CurrentCall", Identifier);
        UpdateCall();
    }

    public bool AddClientToCall(Player player)
    {
        if (CallMembers.Contains(player))
            return false;

        CallMembers.Add(player);
        player.SetData("CurrentCall", Identifier);
        UpdateCall();
        return true;
    }

    public void RemoveClientFromCall(Player player)
    {
        if (!CallMembers.Contains(player))
            return;

        CallMembers.Remove(player);
        player.TriggerEvent("Client:GTA5Voice:KillPhoneCall");
        player.ResetData("CurrentCall");
        player.SetPhoneSpeakerEnabled(false);
        Main.VoiceService.ClearCurrentCallMembers(player);
        UpdateCall();
    }

    private void UpdateCall()
    {
        var allIds = GetCallMembersIds();

        foreach (var p in CallMembers)
        {
            var otherMemberIds = allIds.Where(id => id != p.Id).ToArray();
            NAPI.ClientEvent.TriggerClientEvent(
                p,
                "Client:GTA5Voice:UpdatePhoneCall",
                otherMemberIds
            );
            Main.VoiceService.SetCurrentCallMembers(p, otherMemberIds.Select(id => (int)id).ToArray());
        }
    }

    public void KillCall()
    {
        NAPI.ClientEvent.TriggerClientEventToPlayers(
            CallMembers.ToArray(),
            "Client:GTA5Voice:KillPhoneCall"
        );

        foreach (var member in CallMembers)
        {
            member.ResetData("CurrentCall");
            member.SetPhoneSpeakerEnabled(false);
            Main.VoiceService.ClearCurrentCallMembers(member);
        }
    }

    public Player[] GetCallMembers()
        => CallMembers.ToArray();
    
    public ushort[] GetCallMembersIds()
        => CallMembers.Select(x => x.Id).ToArray();

    public bool IsCallOwner(Player player)
        => CallOwner.Equals(player);

    public int GetCallMemberCount()
        => CallMembers.Count;
}