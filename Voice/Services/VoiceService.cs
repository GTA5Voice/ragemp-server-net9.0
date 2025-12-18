using GTA5Voice.Logging;
using GTA5Voice.Voice.Models;
using GTANetworkAPI;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using GTA5Voice.Extensions;

namespace GTA5Voice.Voice.Services;

public class VoiceService
{
    private static readonly List<VoiceClient> Clients = [];

    internal static VoiceClient? FindClient(Player player)
        => Clients.FirstOrDefault(x => x.Player == player);

    private static VoiceClient? FindClient(int playerId)
        => Clients.FirstOrDefault(x => x.Player.Id == playerId);

    private static Player[] GetVoiceClientPlayers()
        => Clients.Select(x => x.Player).ToArray();
    
    private static Player[] GetOtherVoiceClientPlayers(int selfId)
        => Clients.Where(x => x.Player.Id != selfId).Select(x => x.Player).ToArray();

    public VoiceClient? AddClient(Player player)
    {
        if (FindClient(player) != null)
        {
            ConsoleLogger.Debug($"Voice client (id: {player.Id}) already exists");
            return null;
        }

        var client = new VoiceClient(player, Guid.NewGuid().ToString("N")[..24]);
        Clients.Add(client);
        ConsoleLogger.Debug($"Added voice client (id: {client.Player.Id})");
        return client;
    }

    public void RemoveClient(Player player)
    {
        var client = FindClient(player);
        if (client != null)
        {
            Clients.Remove(client);
            RemoveLocalClientData(client.Player.Id);
            ConsoleLogger.Debug($"Removed voice client (id: {client.Player.Id})");
            return;
        }
        ConsoleLogger.Debug($"Couldn't find voice client (id: {player.Id})");
    }
    
    /**
     * Load client data after connecting
     */
    private record VoiceClientData(ushort RemoteId, int? TeamspeakId, bool WebsocketConnection, float CurrentVoiceRange, bool ForceMuted, bool PhoneSpeakerEnabled);

    public void LoadLocalClientData(int remoteId)
    {
        var requestingClient = FindClient(remoteId);
        if (requestingClient == null)
            return;

        var otherClients = Clients.Where(x => x.Player.Id != remoteId).ToArray();
        if (otherClients.Length == 0)
            return;

        const int chunkSize = 1 << 15; // Max length is 2^16, recommended is 2^15 tho

        foreach (var chunk in otherClients.Chunk(chunkSize))
        {
            var clientData = chunk.Select(client =>
            {
                var pluginData = client.GetPluginData();
                return new VoiceClientData(
                    client.Player.Id,
                    pluginData?.TeamspeakId,
                    pluginData?.WebsocketConnection ?? false,
                    pluginData?.CurrentVoiceRange ?? 0,
                    pluginData?.ForceMuted ?? false,
                    pluginData?.PhoneSpeakerEnabled ?? false
                );
            }).ToArray();
            
            NAPI.ClientEvent.TriggerClientEvent(requestingClient.Player, "Client:GTA5Voice:LoadClientData", JsonSerializer.Serialize(clientData));
        }
    }

    /**
     * Updates the client data for all other voice clients
     */
    public void UpdateLocalClientData(int remoteId, PluginData pluginData)
    {
        var vClients = GetOtherVoiceClientPlayers(remoteId);
        NAPI.ClientEvent.TriggerClientEventToPlayers(vClients, "Client:GTA5Voice:UpdateClientData", remoteId, pluginData);
    }

    public void SetForceMuted(Player player, bool forceMuted)
    {
        var client = FindClient(player);
        if (client == null)
        {
            ConsoleLogger.Debug($"Couldn't find voice client (id: {player.Id})");
            return;
        }

        var pluginData = client.GetPluginData();
        if (pluginData == null || pluginData.ForceMuted == forceMuted)
            return;

        client.SetPluginData(pluginData with { ForceMuted = forceMuted }, UpdateLocalClientData);
    }
    
    public void SetPhoneSpeakerEnabled(Player player, bool phoneSpeakerEnabled)
    {
        var client = FindClient(player);
        if (client == null)
        {
            ConsoleLogger.Debug($"Couldn't find voice client (id: {player.Id})");
            return;
        }

        var pluginData = client.GetPluginData();
        if (pluginData == null || pluginData.PhoneSpeakerEnabled == phoneSpeakerEnabled)
            return;

        client.SetPluginData(pluginData with { PhoneSpeakerEnabled = phoneSpeakerEnabled }, UpdateLocalClientData);
    }

    /**
     * Removes a specific player from the data for all other voice clients
     */
    private void RemoveLocalClientData(int remoteId)
    {
        var vClients = GetOtherVoiceClientPlayers(remoteId);
        NAPI.ClientEvent.TriggerClientEventToPlayers(vClients, "Client:GTA5Voice:RemoveClient", remoteId);
    }
}
