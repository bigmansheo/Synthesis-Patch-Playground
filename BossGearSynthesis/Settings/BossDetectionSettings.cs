using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis.Settings;

namespace BossGearSynthesis.Settings;

public class BossDetectionSettings
{
    [SynthesisSettingName("Use default whitelist only")]
    [SynthesisTooltip("When on, only NPCs that match the explicit allowlist, named-unique allowlist, race allowlist, keyword allowlist, or EditorID prefix whitelist are patched. Disables broad unique-flag matching so modded named NPCs are not touched.")]
    public bool UseDefaultWhitelistOnly = true;

    [SynthesisSettingName("Use NPC unique flag")]
    [SynthesisTooltip("When on, named-unique NPCs are treated as bosses. With 'Use default whitelist only' enabled, this is restricted to NPCs originating from the trusted mod-keys list below so modded named NPCs are not touched.")]
    public bool UseUniqueFlag = true;

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
