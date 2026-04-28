using BossGearSynthesis.Settings;
using Mutagen.Bethesda.Skyrim;

namespace BossGearSynthesis.Analysis;

public enum AttackStyle
{
    Unknown,
    Melee1H,
    Melee2H,
    Ranged,
    MagicAlteration,
    MagicConjuration,
    MagicDestruction,
    MagicIllusion,
    MagicRestoration,
}

public record BossStrengthProfile(
    Skill TopCombatSkill,
    Skill? SecondSkill,
    BasicAttribute? TopAttribute,
    AttackStyle DerivedStyle,
    int EffectiveLevel,
    BossTier Tier,
    bool IsNamedUnique,
    bool IsNamed)
{
    public bool UseAttributeForEffect2 { get; init; }
}
