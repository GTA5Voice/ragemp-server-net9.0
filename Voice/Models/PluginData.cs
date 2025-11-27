namespace GTA5Voice.Voice.Models;

public record PluginData(int? TeamspeakId, bool WebsocketConnection, float CurrentVoiceRange, bool ForceMuted = false);
