using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis.Settings;

namespace BossGearSynthesis.Settings;

public enum SecondEffectMode
{
    Auto,
    PreferSkill,
    PreferAttribute,
    AlwaysSkill,
    AlwaysAttribute,
}

public class StrengthAnalysisSettings
{
    [SynthesisSettingName("Second effect mode")]
    [SynthesisTooltip("How to choose effect 2 (skill vs attribute).")]
    public SecondEffectMode SecondEffectMode = SecondEffectMode.Auto;

    [SynthesisSettingName("Combat skills")]
    [SynthesisTooltip("Skills eligible to drive effect 1 (the offensive enchantment).")]
    public List<Skill> CombatSkills = new()
    {
        Skill.OneHanded,
        Skill.TwoHanded,
        Skill.Archery,
        Skill.Destruction,
        Skill.Conjuration,
        Skill.Illusion,
        Skill.Alteration,
        Skill.Restoration,
    };

    [SynthesisSettingName("Skill baseline")]
    [SynthesisTooltip("Used by Auto mode to z-score skill values: baseline = SkillBaseline + level * SkillBaselinePerLevel.")]
    public float SkillBaseline = 15f;

    [SynthesisSettingName("Skill baseline per level")]
    public float SkillBaselinePerLevel = 1.5f;

    [SynthesisSettingName("Attribute baseline")]
    public float AttributeBaseline = 50f;

    [SynthesisSettingName("Attribute baseline per level")]
    public float AttributeBaselinePerLevel = 5f;
}
