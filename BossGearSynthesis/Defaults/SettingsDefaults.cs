using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;

namespace BossGearSynthesis.Defaults;

/// <summary>
/// One-shot seeding of empty settings so the patcher's curated defaults
/// (boss whitelist, effect map, jewelry fallbacks, Ahzidal special) work
/// out of the box without the user having to fill anything in.
/// </summary>
public static class SettingsDefaults
{
    public static void Apply(Settings.Settings settings)
    {
        SeedEffectMap(settings);
        SeedBossDetection(settings);
        SeedGearFallbacks(settings);
        SeedAhzidalSpecial(settings);
    }

    private static void SeedEffectMap(Settings.Settings settings)
    {
        if (settings.EffectMap.SkillEffects.Count == 0)
            settings.EffectMap.SkillEffects = EffectMapDefaults.BuildSkillDefaults();
        if (settings.EffectMap.AttributeEffects.Count == 0)
            settings.EffectMap.AttributeEffects = EffectMapDefaults.BuildAttributeDefaults();
    }

    private static void SeedBossDetection(Settings.Settings settings)
    {
        var bd = settings.BossDetection;

        if (bd.NamedUniqueAllowlist.Count == 0)
            foreach (var fk in NamedUniqueAllowlist.FormKeys)
                bd.NamedUniqueAllowlist.Add(new FormLink<INpcGetter>(fk));

        if (bd.EditorIdPrefixWhitelist.Count == 0)
            bd.EditorIdPrefixWhitelist.AddRange(BossWhitelistDefaults.EditorIdPrefixes);

        if (bd.RaceAllowlist.Count == 0)
            foreach (var fk in BossWhitelistDefaults.Races)
                bd.RaceAllowlist.Add(new FormLink<IRaceGetter>(fk));

        if (bd.TrustedUniqueModKeys.Count == 0)
            bd.TrustedUniqueModKeys.AddRange(BossWhitelistDefaults.TrustedUniqueModKeys);

        // PluginSourceAllowlist intentionally left empty by default → scan all plugins.
    }

    private static void SeedGearFallbacks(Settings.Settings settings)
    {
        if (settings.GearSelection.FallbackRingBase.IsNull)
            settings.GearSelection.FallbackRingBase = new FormLink<IArmorGetter>(BossWhitelistDefaults.GoldRing);
        if (settings.GearSelection.FallbackAmuletBase.IsNull)
            settings.GearSelection.FallbackAmuletBase = new FormLink<IArmorGetter>(BossWhitelistDefaults.GoldNecklace);
    }

    private static void SeedAhzidalSpecial(Settings.Settings settings)
    {
        if (settings.AhzidalSpecial.Ahzidal.IsNull)
            settings.AhzidalSpecial.Ahzidal = new FormLink<INpcGetter>(NamedUniqueAllowlist.Ahzidal);
        if (settings.AhzidalSpecial.ExtraEffectMgef.IsNull)
            settings.AhzidalSpecial.ExtraEffectMgef = new FormLink<IMagicEffectGetter>(NamedUniqueAllowlist.FortifyEnchantingMgef);
    }
}
