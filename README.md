## GTA5Voice Server Integration (net9.0) for [RAGE Multiplayer](https://rage.mp)
For more information, visit our website: https://gta5voice.com<br>
We are not in any way affiliated, associated, authorized, endorsed by, or connected with Rockstar Games or Take-Two Interactive.

## Links
- The latest plugin version can be found here: https://gta5voice.com/downloads
- The documentation can be found here: https://docs.gta5voice.com
- Join our Discord for more information: https://gta5voice.com/discord

## Notes
- If you're experienced in development, building from source is strongly recommended over using the pre-built version. The pre-built release doesn't support custom features like muting on death, delayed channel joining, and other gameplay-specific mechanics.
- It is recommended to keep the code up to date to avoid security vulnerabilities and ensure compatibility. All updates are announced on our [Discord server](https://gta5voice.com/discord), including detailed changelogs and more.

## Quick setup (only recommended for non-experienced users)
1. Download the [latest pre-built version](https://github.com/GTA5Voice/ragemp-server-net9.0/releases).
2. Put the downloaded release into the '`dotnet/resources`' folder.
3. Register the resource in **settings.xml** as shown below:
```xml
<?xml version="1.0"?>
<config xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
   <resource src="gta5voice" />
</config>
```

## Configuration
1. Configure **meta.xml** according to your needs:
```xml
<meta>
  <info name="gta5voice" author="https://gta5voice.com" type="script" />
  <script src="GTA5Voice.dll"/>
  <settings>
    <setting name="VirtualServerUID" value="YOUR_VIRTUAL_SERVER_UID_HERE" description="Unique identifier of your TeamSpeak server" />
    <setting name="IngameChannelId" value="INGAME_CHANNEL_ID_HERE" description="ID of the TeamSpeak channel the client should get moved into after log-in" />
    <setting name="FallbackChannelId" value="FALLBACK_CHANNEL_ID_HERE" description="ID of the TeamSpeak channel the client should get moved into after disconnecting from the server" />
    <setting name="IngameChannelPassword" value="INGAME_CHANNEL_PASSWORD_HERE" description="Password of the Ingame TeamSpeak channel" />
    <setting name="DebuggingEnabled" value="false" description="Enables server debugging for development purposes. Not recommended in prod environment" />
    <setting name="Language" value="en" description="Language for popup, error messages, etc." />
    <setting name="CalculationInterval" value="250" description="Calculation tick interval" />
    <setting name="VoiceRanges" value="[2, 5, 8, 15]" description="Usable voice ranges" />
    <setting name="ExcludedChannels" value="[100, 200, 300]" description="Channels excluded from voice chat transfers. It is recommended to exclude only restricted channels, not open ones" />
    <setting name="EnableDistanceBasedVolume" value="false" description="Enables distance-based volume attenuation to make remote players quieter" />
    <setting name="VolumeDecreaseMultiplier" value="1.0" description="Scales how strongly volume decreases with distance when distance-based volume is enabled" />
    <setting name="MinimumVoiceVolume" value="0.25" description="Lowest allowed voice volume level when distance-based volume attenuation is applied" />
  </settings>
</meta>
```
2. Make sure your Virtual Server UID is registered at [https://gta5voice.com](https://gta5voice.com).
