using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis.Settings;

namespace BossGearSynthesis.Settings;

public class BossDetectionSettings
{
    [SynthesisSettingName("Restrict to curated boss list")]
    [SynthesisTooltip(
        "When ON (default), only NPCs in the explicit allowlist or named-unique allowlist " +
        "are patched. This matches the boss roster from the Elder Scrolls wiki and prevents " +
        "modded / vanilla named NPCs (merchants, quest characters, etc.) from being swept in. " +
        "Turn OFF to fall back to the broad heuristics (unique flag / race / keyword).")]
    public bool RestrictToCuratedList = true;

    [SynthesisSettingName("Use encounter-zone boss flag")]
    public bool UseEncounterZoneFlag = true;

    [SynthesisSettingName("Use NPC unique flag")]
    [SynthesisTooltip("Ignored when 'Restrict to curated boss list' is ON.")]
    public bool UseUniqueFlag = true;

    [SynthesisSettingName("Race allowlist")]
    [SynthesisTooltip("Any NPC of these races qualifies as a boss. Ignored when 'Restrict to curated boss list' is ON.")]
    public List<FormLink<IRaceGetter>> RaceAllowlist = new();

    [SynthesisSettingName("Keyword allowlist")]
    [SynthesisTooltip("Any NPC carrying one of these keywords qualifies as a boss. Ignored when 'Restrict to curated boss list' is ON.")]
    public List<FormLink<IKeywordGetter>> KeywordAllowlist = new();

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
