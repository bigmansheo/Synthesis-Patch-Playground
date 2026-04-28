using BossGearSynthesis.Analysis;
using BossGearSynthesis.Settings;

namespace BossGearSynthesis.Enchantments;

public static class MagnitudeFormula
{
    public static float ComputePrimary(BossStrengthProfile profile, MagnitudeScalingSettings cfg)
    {
        if (profile.IsNamedUnique) return cfg.UniqueFlatMagnitude;

        var tier = cfg.Tiers.FirstOrDefault(t => t.Tier == profile.Tier);
        if (tier == null) return cfg.UniqueFlatMagnitude * 0.5f;

        var bump = profile.IsNamed ? cfg.NamedBump : 0f;
        var raw = tier.Base + tier.LevelCoef * profile.EffectiveLevel + bump;
        return Math.Clamp(raw, tier.Floor, tier.Ceiling);
    }

    public static float ComputeSecondary(float primary, MagnitudeScalingSettings cfg) =>
        primary * cfg.SecondaryEffectScale;

    public static float ApplyAttributeMultiplier(float magnitude, BasicAttribute attr, MagnitudeScalingSettings cfg) =>
        attr switch
        {
            BasicAttribute.Health => magnitude * cfg.HealthMultiplier,
            BasicAttribute.Magicka => magnitude * cfg.MagickaMultiplier,
            BasicAttribute.Stamina => magnitude * cfg.StaminaMultiplier,
            _ => magnitude,
        };
}
