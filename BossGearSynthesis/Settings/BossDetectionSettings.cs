using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis.Settings;

namespace BossGearSynthesis.Settings;

public class BossDetectionSettings
{
    [SynthesisSettingName("Use encounter-zone boss flag")]
    public bool UseEncounterZoneFlag = true;

    [SynthesisSettingName("Use NPC unique flag")]
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
