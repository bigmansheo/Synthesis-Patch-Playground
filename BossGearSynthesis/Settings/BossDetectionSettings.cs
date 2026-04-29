using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis.Settings;

namespace BossGearSynthesis.Settings;

public class BossDetectionSettings
{
    [SynthesisSettingName("Strict bosses only (recommended)")]
    [SynthesisTooltip("When ON, only NPCs in the named-unique allowlist or explicit allowlist are patched. " +
                      "This matches the curated wiki list of Skyrim bosses (Dragon Priests, Harkon, Miraak, etc.) " +
                      "and prevents the patcher from latching onto every named NPC, modded follower, or quest character. " +
                      "Turn OFF to fall back to the legacy heuristic (unique flag / race / keyword detection).")]
    public bool StrictBossesOnly = true;

    [SynthesisSettingName("Use encounter-zone boss flag")]
    [SynthesisTooltip("Ignored when 'Strict bosses only' is ON.")]
    public bool UseEncounterZoneFlag = true;

    [SynthesisSettingName("Use NPC unique flag")]
    [SynthesisTooltip("Ignored when 'Strict bosses only' is ON. Catches every named/unique NPC in the load order, " +
                      "including modded ones, which is usually too broad.")]
    public bool UseUniqueFlag = true;

    [SynthesisSettingName("Race allowlist")]
    [SynthesisTooltip("Any NPC of these races qualifies as a boss.")]
    public List<FormLink<IRaceGetter>> RaceAllowlist = new();

    [SynthesisSettingName("Keyword allowlist")]
    [SynthesisTooltip("Any NPC carrying one of these keywords qualifies as a boss.")]
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
