using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Modules.Cvars;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Drawing;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;

namespace SLAYER_KillStreak;

public class SLAYER_KillStreakConfig : BasePluginConfig
{
    [JsonPropertyName("ShowKillInfo")] public bool ShowKillInfo { get; set; } = true;
    [JsonPropertyName("KillInfoMessage")] public string KillInfoMessage { get; set; } = "<br><font class='fontSize-m' color='red'>Killed</font> <font class='fontSize-m' color='lime'>{PlayerName}</font> <font class='fontSize-m' color='gold'>[{WeaponName}]</font>";
    [JsonPropertyName("LoopIfKillIconsEnd")] public bool LoopIfKillIconsEnd { get; set; } = true;
    [JsonPropertyName("ExtendKillStreakIcons")] public bool ExtendKillStreakIcons { get; set; } = false;
    [JsonPropertyName("SoundEventPath")] public string SoundEventPath { get; set; } = "soundevents/slayer-killstreak.vsndevts";
    [JsonPropertyName("SoundVolume")] public float SoundVolume { get; set; } = 1f;
    [JsonPropertyName("KillIcons")] public Dictionary<int, KillStreakIconsSettings> KillIcons { get; set; } = new Dictionary<int, KillStreakIconsSettings>
    {
        {1, new KillStreakIconsSettings { Icon = "<a href=\"https://imgbb.com/\"><img src=\"https://i.ibb.co/PLL04Q8/kill1.png\" alt=\"kill1\" border=\"0\"></a>", Sound = "Kill.Sound_01", Duration = 3.0f } },
        {2, new KillStreakIconsSettings { Icon = "<a href=\"https://imgbb.com/\"><img src=\"https://i.ibb.co/h1F9FSpk/kill2.png\" alt=\"kill2\" border=\"0\"></a>", Sound = "Kill.Sound_02", Duration = 3.0f } },
        {3, new KillStreakIconsSettings { Icon = "<a href=\"https://imgbb.com/\"><img src=\"https://i.ibb.co/FLt4JW6h/kill3.png\" alt=\"kill3\" border=\"0\"></a>", Sound = "Kill.Sound_03", Duration = 3.0f } },
        {4, new KillStreakIconsSettings { Icon = "<a href=\"https://imgbb.com/\"><img src=\"https://i.ibb.co/F4d16TXh/kill4.png\" alt=\"kill4\" border=\"0\"></a>", Sound = "Kill.Sound_04", Duration = 3.0f } },
        {5, new KillStreakIconsSettings { Icon = "<a href=\"https://imgbb.com/\"><img src=\"https://i.ibb.co/PZfvmKDY/kill5.png\" alt=\"kill5\" border=\"0\"></a>", Sound = "Kill.Sound_05", Duration = 5.5f } },
        {6, new KillStreakIconsSettings { Icon = "<a href=\"https://imgbb.com/\"><img src=\"https://i.ibb.co/BKY6SsdC/kill6.png\" alt=\"kill6\" border=\"0\"></a>", Sound = "Kill.Sound_06", Duration = 5.5f } },
    };
    [JsonPropertyName("HeadshotIcons")] public Dictionary<int, KillStreakIconsSettings> HeadshotIcons { get; set; } = new Dictionary<int, KillStreakIconsSettings>();
}
public class KillStreakIconsSettings
{
    [JsonPropertyName("Icon")] public string Icon { get; set; } = "";
    [JsonPropertyName("Sound")] public string Sound { get; set; } = "";
    [JsonPropertyName("Duration")] public float Duration { get; set; } = 3.0f;
}

public partial class SLAYER_KillStreak : BasePlugin, IPluginConfig<SLAYER_KillStreakConfig>
{
    public override string ModuleName => "SLAYER_KillStreak";
    public override string ModuleVersion => "1.0";
    public override string ModuleAuthor => "SLAYER";
    public override string ModuleDescription => "Show Kill Icon after killing Someone";
    public required SLAYER_KillStreakConfig Config {get; set;}
    public void OnConfigParsed(SLAYER_KillStreakConfig config)
    {
        Config = config;
    }

    public Dictionary<CCSPlayerController, int> PlayerKillStreaks = new Dictionary<CCSPlayerController, int>();
    public override void Load(bool hotReload)
    {
        RegisterListener<Listeners.OnServerPrecacheResources>((manifest) =>
        {
            // Add resources to the manifest for pre-caching
            manifest.AddResource($"{Config.SoundEventPath}");
        });
        RegisterListener<Listeners.OnTick>(() =>
        {
            PrintCenterMessageTick();
        });
        RegisterEventHandler<EventPlayerSpawn>((@event, info) =>
        {
            var player = @event.Userid;
            if(player == null || !player.IsValid || player.IsBot || player.IsHLTV) return HookResult.Continue;

            PlayerKillStreaks[player] = 0;

            return HookResult.Continue;
        });
        RegisterEventHandler<EventPlayerDeath>((@event, info) =>
        {
            var player = @event.Userid;
            var attacker = @event.Attacker;
            var weapon = @event.Weapon;
            if (player == null || !player.IsValid) return HookResult.Continue;
            if (attacker == null || !attacker.IsValid || attacker.IsBot || attacker.IsHLTV) return HookResult.Continue;
            if (player == attacker || player.TeamNum == attacker.TeamNum) return HookResult.Continue; 

            if (!PlayerKillStreaks.ContainsKey(attacker))
            {
                PlayerKillStreaks[attacker] = 0;
            }

            PlayerKillStreaks[attacker] += 1;

            if (@event.Headshot && Config.HeadshotIcons.Count > 0) ShowKillStreak(player, attacker, true, weapon); // Show headshot icon if available
            else ShowKillStreak(player, attacker, false, weapon); // Show normal kill icon

            return HookResult.Continue;
        });
    }
    public void ShowKillStreak(CCSPlayerController player, CCSPlayerController attacker, bool isHeadshot, string weapon)
    {
        if (!PlayerKillStreaks.ContainsKey(attacker)) return;

        int count = PlayerKillStreaks[attacker];
        var iconsDict = isHeadshot ? Config.HeadshotIcons : Config.KillIcons;

        if (iconsDict.Count == 0) return;

        KillStreakIconsSettings? settings = null;
        if (iconsDict.ContainsKey(count))
        {
            settings = iconsDict[count];
        }
        else if (Config.LoopIfKillIconsEnd && iconsDict.Count > 0) // Loop through icons if enabled
        {
            int modCount = count % iconsDict.Count;
            if (modCount == 0) modCount = iconsDict.Count; // To handle exact multiples
            if (iconsDict.ContainsKey(modCount))
            {
                settings = iconsDict[modCount];
            }
        }

        if (settings == null) return;

        if (!string.IsNullOrEmpty(settings.Icon))
        {
            if (!CenterMessageLines.ContainsKey(1)) UpdateCenterMessageLine(1, $"{settings.Icon}", new RecipientFilter { attacker }, settings.Duration);
            else if (!Config.ExtendKillStreakIcons && CenterMessageLines.ContainsKey(1)) UpdateCenterMessageLine(1, $"{settings.Icon}", new RecipientFilter { attacker }, settings.Duration, true);
            else if (Config.ExtendKillStreakIcons && CenterMessageLines.ContainsKey(1)) ExtendCenterMessageLine(1, $" {settings.Icon}", settings.Duration);

            if (Config.ShowKillInfo)
            {
                var killInfoMessage = Config.KillInfoMessage.Replace("{PlayerName}", player.PlayerName).Replace("{WeaponName}", RemoveWeaponPrefix(weapon).ToUpper());
                UpdateCenterMessageLine(2, killInfoMessage, new RecipientFilter { attacker }, settings.Duration, true);
            }
            
        }
        if (!string.IsNullOrEmpty(settings.Sound))
        {
            attacker.EmitSound(settings.Sound, new RecipientFilter { attacker }, Config.SoundVolume);
        }
    }
    public string RemoveWeaponPrefix(string weaponName)
    {
        if (string.IsNullOrEmpty(weaponName)) return weaponName;

        if (weaponName.StartsWith("weapon_", StringComparison.OrdinalIgnoreCase))
        {
            return weaponName.Substring("weapon_".Length);
        }

        return weaponName; // Return original if no prefix matched
    }

    public override void Unload(bool hotReload)
    {
        base.Unload(hotReload);

        PlayerKillStreaks.Clear();
    }
}