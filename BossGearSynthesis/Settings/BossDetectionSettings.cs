using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis.Settings;

namespace BossGearSynthesis.Settings;

public class BossDetectionSettings
{
    [SynthesisSettingName("Use default whitelist only")]
    [SynthesisTooltip("When on (default), heuristic matchers (race, keyword, EditorID prefix, unique-flag) ONLY fire for NPCs originating from a trusted plugin (vanilla + DLC + CC). Modded NPCs that share a vanilla race (e.g. modded dragons) or a vanilla EditorID pattern (e.g. EncBanditChief_MyMod) are NOT touched. The Explicit allowlist and Named-unique allowlist still fire for any plugin so you can opt specific modded bosses in by FormKey.")]
    public bool UseDefaultWhitelistOnly = true;

    [SynthesisSettingName("Use NPC unique flag (only when whitelist-only is OFF)")]
    [SynthesisTooltip("Broad fallback: when 'Use default whitelist only' is OFF, treat any unique-flagged named NPC from a trusted plugin as a boss. WARNING: this catches every named vanilla NPC including Lydia, shopkeepers, and jarls. Leave off unless you know what you want.")]
    public bool UseUniqueFlag = false;

    [SynthesisSettingName("Source plugins (NPC scan scope)")]
    [SynthesisTooltip("Limit boss detection to NPCs that originate from these plugins. Leave empty to scan ALL loaded plugins (default). Use this to source bosses only from specific overhauls (e.g. OBIS, MorrowLoot) or to keep vanilla-only.")]
    public List<string> PluginSourceAllowlist = new();

    [SynthesisSettingName("Trusted mod keys for unique-flag matching")]
    [SynthesisTooltip("When the default whitelist is on, unique-flag matching only fires for NPCs whose master is one of these plugins. Defaults to vanilla + official DLC.")]
    public List<string> TrustedUniqueModKeys = new();

    [SynthesisSettingName("Race allowlist")]
    [SynthesisTooltip("Any NPC of these races qualifies as a boss.")]
    public List<FormLink<IRaceGetter>> RaceAllowlist = new();

    [SynthesisSettingName("Keyword allowlist")]
    [SynthesisTooltip("Any NPC carrying one of these keywords qualifies as a boss.")]
    public List<FormLink<IKeywordGetter>> KeywordAllowlist = new();

    [SynthesisSettingName("EditorID prefix whitelist")]
    [SynthesisTooltip("NPCs whose EditorID starts with one of these prefixes (case-insensitive) qualify as bosses. Used for generic encounter templates such as Draugr Overlord, Bandit Chief, Master Vampire, etc.")]
    public List<string> EditorIdPrefixWhitelist = new();

    [SynthesisSettingName("Explicit allowlist")]
    [SynthesisTooltip("These NPCs are always treated as bosses.")]
    public List<FormLink<INpcGetter>> ExplicitAllowlist = new();

    [SynthesisSettingName("Explicit blocklist")]
    [SynthesisTooltip("These NPCs are never patched.")]
    public List<FormLink<INpcGetter>> ExplicitBlocklist = new();

    [SynthesisSettingName("Named-unique allowlist")]
    [SynthesisTooltip("Bosses in this list always get a piece at the unique-flat magnitude.")]
    public List<FormLink<INpcGetter>> NamedUniqueAllowlist = new();
}
