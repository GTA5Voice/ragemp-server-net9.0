using GTA5Voice.Extensions;
using GTA5Voice.Voice.Models;
using GTANetworkAPI;

namespace GTA5Voice.Voice.Services;

public class PhoneService
{
    private List<PhoneCall> _phoneCalls = [];

    /**
     * Should be executed when the target accepts the call
     */
    public void StartCall(Player player, Player target)
    {
        if (player.IsInCall() || target.IsInCall())
            return;
        
        var call = new PhoneCall(player, target);
        _phoneCalls.Add(call);
    }

    /**
     * Should be executed when a player hits "End call" on the phone
     */
    public void EndCall(Player player)
    {
        var call = player.GetCurrentCall();
        if (call == null) return;

        if (call.IsCallOwner(player) || call.GetCallMemberCount() <= 2)
        {
            call.KillCall();
            _phoneCalls.Remove(call);
        }
        else
            call.RemoveClientFromCall(player);
    }
    
    /**
     * Should be executed when someone adds a new target to the call (group call)
     */
    public bool AddToCallGroup(Player caller, Player target)
    {
        if (target.IsInCall())
            return false;
        
        var callIdentifier = caller.GetCurrentCallId();
        if (callIdentifier == null)
            return false;
        
        var sPhoneCall = _phoneCalls.FirstOrDefault(p => p.Identifier == callIdentifier);
        if (sPhoneCall == null)
            return false;
        
        return sPhoneCall.AddClientToCall(target);
    }

    public void OnPlayerDisconnected(Player player)
    {
        foreach (var call in _phoneCalls.Where(c => c.GetCallMembers().Contains(player)))
            call.RemoveClientFromCall(player);
    }

    public PhoneCall? GetCall(string callIdentifier)
        => _phoneCalls.FirstOrDefault(p => p.Identifier == callIdentifier);
}