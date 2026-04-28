using Mutagen.Bethesda.Synthesis.Settings;

namespace BossGearSynthesis.Settings;

public class Settings
{
    [SynthesisSettingName("Boss detection")]
    [SynthesisTooltip("Which NPCs are considered bosses for patching.")]
    public BossDetectionSettings BossDetection = new();

    [SynthesisSettingName("Strength analysis")]
    [SynthesisTooltip("How the patcher decides which two effects to apply.")]
    public StrengthAnalysisSettings StrengthAnalysis = new();

    [SynthesisSettingName("Magnitude scaling")]
    [SynthesisTooltip("Tier classification and per-tier magnitude curves.")]
    public MagnitudeScalingSettings MagnitudeScaling = new();

    [SynthesisSettingName("Effect map")]
    [SynthesisTooltip("Skill / attribute -> MGEF mapping.")]
    public EffectMapSettings EffectMap = new();

    [SynthesisSettingName("Gear selection")]
    [SynthesisTooltip("Slot priority + material handling for the replaced piece.")]
    public GearSelectionSettings GearSelection = new();

    [SynthesisSettingName("Sister sets")]
    [SynthesisTooltip("Which armor sets count as members of the same family for material upgrades.")]
    public SisterSetsSettings SisterSets = new();

    [SynthesisSettingName("Naming")]
    [SynthesisTooltip("Naming template for the synthesized item.")]
    public NamingSettings Naming = new();

    [SynthesisSettingName("Output")]
    [SynthesisTooltip("Dry-run, logging, etc.")]
    public OutputSettings Output = new();

    [SynthesisSettingName("Ahzidal special")]
    [SynthesisTooltip("Adds a non-disenchantable Fortify Enchanting effect to Ahzidal's piece.")]
    public AhzidalSpecialSettings AhzidalSpecial = new();
}
