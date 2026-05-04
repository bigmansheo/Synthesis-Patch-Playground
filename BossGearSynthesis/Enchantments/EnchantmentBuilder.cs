using BossGearSynthesis.Analysis;
using BossGearSynthesis.Defaults;
using BossGearSynthesis.Settings;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis;

namespace BossGearSynthesis.Enchantments;

public class EnchantmentBuilder
{
    private readonly Settings.Settings _settings;
    private readonly EffectMap _map;

    public EnchantmentBuilder(Settings.Settings settings, EffectMap map)
    {
        _settings = settings;
        _map = map;
    }

    public ObjectEffect? Build(
        IPatcherState<ISkyrimMod, ISkyrimModGetter> state,
        INpcGetter boss,
        BossStrengthProfile profile,
        string editorIdSuffix)
    {
        var primary = _map.ForSkill(profile.TopCombatSkill);
        if (primary is null) return null;

        var basePrimary = MagnitudeFormula.ComputePrimary(profile, _settings.MagnitudeScaling);
        var baseSecondary = MagnitudeFormula.ComputeSecondary(basePrimary, _settings.MagnitudeScaling);
        var primaryMag = basePrimary * primary.Value.Multiplier;

        var ench = state.PatchMod.ObjectEffects.AddNew("BossGear_ENCH_" + editorIdSuffix);
        ench.Name = "Boss Gear Enchantment";
        ench.CastType = CastType.ConstantEffect;
        ench.TargetType = TargetType.Self;
        ench.EnchantType = ObjectEffect.EnchantTypeEnum.Enchantment;
        ench.Flags = ObjectEffect.Flag.NoAutoCalc;
        ench.EnchantmentCost = 0;
        ench.EnchantmentAmount = 0;
        ench.ChargeTime = 0;
        ench.WornRestrictions.SetTo(FormKey.Null);

        ench.Effects.Add(MakeEffect(primary.Value.Mgef, primaryMag));

        var second = ResolveSecondEffect(profile, baseSecondary);
        if (second is not null)
            ench.Effects.Add(MakeEffect(second.Value.mgef, second.Value.magnitude));

        var isAhzidal = _settings.AhzidalSpecial.Enabled && boss.FormKey == ResolveAhzidalKey();
        if (isAhzidal)
        {
            var extraMgef = _settings.AhzidalSpecial.ExtraEffectMgef.IsNull
                ? NamedUniqueAllowlist.FortifyEnchantingMgef
                : _settings.AhzidalSpecial.ExtraEffectMgef.FormKey;
            ench.Effects.Add(MakeEffect(extraMgef, _settings.AhzidalSpecial.Magnitude));
        }

        // Pointing BaseEnchantment at self makes the engine refuse to disenchant —
        // same mechanism vanilla uses for Spellbreaker/Aetherial Crown/etc. Ahzidal's
        // piece is always protected because its extra effect must not enter the
        // player's enchantment library.
        if (isAhzidal || _settings.GearSelection.PreventDisenchant)
            ench.BaseEnchantment.SetTo(ench.FormKey);

        return ench;
    }

    private (FormKey mgef, float magnitude)? ResolveSecondEffect(BossStrengthProfile profile, float baseSecondary)
    {
        if (profile.UseAttributeForEffect2 && profile.TopAttribute is { } attr)
        {
            var lookup = _map.ForAttribute(attr);
            if (lookup is null) return null;
            var mag = MagnitudeFormula.ApplyAttributeMultiplier(baseSecondary, attr, _settings.MagnitudeScaling);
            return (lookup.Value.Mgef, mag * lookup.Value.Multiplier);
        }
        if (profile.SecondSkill is { } sk)
        {
            var lookup = _map.ForSkill(sk);
            if (lookup is null) return null;
            return (lookup.Value.Mgef, baseSecondary * lookup.Value.Multiplier);
        }
        return null;
    }

    private FormKey ResolveAhzidalKey()
    {
        var configured = _settings.AhzidalSpecial.Ahzidal;
        return configured.IsNull ? NamedUniqueAllowlist.Ahzidal : configured.FormKey;
    }

    private static Effect MakeEffect(FormKey mgef, float magnitude) =>
        new()
        {
            BaseEffect = new FormLinkNullable<IMagicEffectGetter>(mgef),
            Data = new EffectData
            {
                Magnitude = magnitude,
                Area = 0,
                Duration = 0,
            },
        };
}
