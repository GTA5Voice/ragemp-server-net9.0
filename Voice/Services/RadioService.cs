using GTA5Voice.Definitions;
using GTA5Voice.Extensions;
using GTA5Voice.Logging;
using GTA5Voice.Services;
using GTA5Voice.Voice.Models;
using GTANetworkAPI;

namespace GTA5Voice.Voice.Services;

public class RadioService
{
    private List<RadioChannel> _radioChannels = [];
    
    private void CreateFrequency(Player player, string frequency)
    {
        if (_radioChannels.All(c => c.Frequency != frequency))
            _radioChannels.Add(new RadioChannel(frequency, player));
    }

    private void DestroyFrequency(string frequency)
    {
        if (_radioChannels.All(c => c.Frequency == frequency))
            _radioChannels.RemoveAll(c => c.Frequency == frequency);
    }
    
    public void EnterRadioChannel(Player player, string frequency)
    {
        if (player.HasRadioFrequency())
        {
            var oldFrequency = player.GetRadioFrequency();
            if (frequency == oldFrequency)
                return;
            
            var oldChannel = GetRadioChannel(oldFrequency);
            oldChannel?.RemoveFromRadioChannel(player);
            CheckForDeadFrequency(oldChannel);
        }
        
        var newChannel = GetRadioChannel(frequency);
        if (newChannel == null)
            CreateFrequency(player, frequency);
        else
            newChannel.AddToRadioChannel(player);
    }

    public void LeaveRadioChannel(Player player)
    {
        if (!player.HasRadioFrequency())
            return;

        var frequency = player.GetRadioFrequency();
        var channel = GetRadioChannel(frequency);
        channel?.RemoveFromRadioChannel(player);
        CheckForDeadFrequency(channel);
    }

    public void OnPlayerDisconnected(Player player)
    {
        foreach (var channel in _radioChannels.Where(c => c.IsPlayerInRadio(player)))
        {
            channel.RemoveFromRadioChannel(player);
            CheckForDeadFrequency(channel);
        }
    }

    public RadioChannel? GetRadioChannel(string frequency)
        => _radioChannels.FirstOrDefault(c => c.Frequency == frequency);

    private void CheckForDeadFrequency(RadioChannel? channel)
    {
        if (channel?.GetRadioMemberCount() <= 0)
            DestroyFrequency(channel.Frequency);
    }
}