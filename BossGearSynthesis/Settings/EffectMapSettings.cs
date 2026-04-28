using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis.Settings;

namespace BossGearSynthesis.Settings;

public enum BasicAttribute
{
    Health,
    Magicka,
    Stamina,
}

public class SkillEffectEntry
{
    [SynthesisSettingName("Skill")]
    public Skill Skill;

    [SynthesisSettingName("Magic effect")]
    public FormLink<IMagicEffectGetter> MagicEffect = new();

    [SynthesisSettingName("Magnitude multiplier")]
    public float Multiplier = 1.0f;
}

public class AttributeEffectEntry
{
    [SynthesisSettingName("Attribute")]
    public BasicAttribute Attribute;

    [SynthesisSettingName("Magic effect")]
    public FormLink<IMagicEffectGetter> MagicEffect = new();

    [SynthesisSettingName("Magnitude multiplier")]
    public float Multiplier = 1.0f;
}

public class EffectMapSettings
{
    [SynthesisSettingName("Skill effects")]
    [SynthesisTooltip("Maps each skill to a Fortify <Skill> magic effect.")]
    public List<SkillEffectEntry> SkillEffects = new();

    [SynthesisSettingName("Attribute effects")]
    [SynthesisTooltip("Maps each basic attribute (Health/Magicka/Stamina) to a Fortify magic effect.")]
    public List<AttributeEffectEntry> AttributeEffects = new();
}
