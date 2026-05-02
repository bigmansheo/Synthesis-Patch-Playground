using BossGearSynthesis.Settings;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using FK = Mutagen.Bethesda.FormKeys.SkyrimSE;

namespace BossGearSynthesis.Defaults;

/// <summary>
/// Default Skill -> MGEF and Attribute -> MGEF mappings for vanilla Skyrim.esm.
/// All FormKeys come from Mutagen.Bethesda.FormKeys.SkyrimSE so they are
/// verified at compile time — the previous hand-typed values (0x0BABE4...)
/// did not resolve to any vanilla MGEF.
/// </summary>
public static class EffectMapDefaults
{
    public static readonly IReadOnlyDictionary<Skill, FormKey> SkillMgef = new Dictionary<Skill, FormKey>
    {
        { Skill.OneHanded,    FK.Skyrim.MagicEffect.EnchFortifyOneHandedConstantSelf.FormKey },
        { Skill.TwoHanded,    FK.Skyrim.MagicEffect.EnchFortifyTwoHandedConstantSelf.FormKey },
        { Skill.Archery,      FK.Skyrim.MagicEffect.EnchFortifyArcheryConstantSelf.FormKey },
        { Skill.Block,        FK.Skyrim.MagicEffect.EnchFortifyBlockConstantSelf.FormKey },
        { Skill.Smithing,     FK.Skyrim.MagicEffect.EnchFortifySmithingConstantSelf.FormKey },
        { Skill.HeavyArmor,   FK.Skyrim.MagicEffect.EnchFortifyHeavyArmorConstantSelf.FormKey },
        { Skill.LightArmor,   FK.Skyrim.MagicEffect.EnchFortifyLightArmorConstantSelf.FormKey },
        { Skill.Pickpocket,   FK.Skyrim.MagicEffect.EnchFortifyPickpocketConstantSelf.FormKey },
        { Skill.Lockpicking,  FK.Skyrim.MagicEffect.EnchFortifyLockpickingConstantSelf.FormKey },
        { Skill.Sneak,        FK.Skyrim.MagicEffect.EnchFortifySneakConstantSelf.FormKey },
        { Skill.Alchemy,      FK.Skyrim.MagicEffect.EnchFortifyAlchemyConstantSelf.FormKey },
        { Skill.Speech,       FK.Skyrim.MagicEffect.EnchFortifySpeechcraftConstantSelf.FormKey },
        { Skill.Alteration,   FK.Skyrim.MagicEffect.EnchFortifyAlterationConstantSelf.FormKey },
        { Skill.Conjuration,  FK.Skyrim.MagicEffect.EnchFortifyConjurationConstantSelf.FormKey },
        { Skill.Destruction,  FK.Skyrim.MagicEffect.EnchFortifyDestructionConstantSelf.FormKey },
        { Skill.Illusion,     FK.Skyrim.MagicEffect.EnchFortifyIllusionConstantSelf.FormKey },
        { Skill.Restoration,  FK.Skyrim.MagicEffect.EnchFortifyRestorationConstantSelf.FormKey },
        { Skill.Enchanting,   FK.Skyrim.MagicEffect.EnchRobesFortifyEnchantingConstantSelf.FormKey },
    };

    public static readonly IReadOnlyDictionary<BasicAttribute, FormKey> AttributeMgef =
        new Dictionary<BasicAttribute, FormKey>
        {
            { BasicAttribute.Health,  FK.Skyrim.MagicEffect.EnchFortifyHealthConstantSelf.FormKey },
            { BasicAttribute.Magicka, FK.Skyrim.MagicEffect.EnchFortifyMagickaConstantSelf.FormKey },
            { BasicAttribute.Stamina, FK.Skyrim.MagicEffect.EnchFortifyStaminaConstantSelf.FormKey },
        };

    public static List<SkillEffectEntry> BuildSkillDefaults() =>
        SkillMgef.Select(kv => new SkillEffectEntry
        {
            Skill = kv.Key,
            MagicEffect = new FormLink<IMagicEffectGetter>(kv.Value),
            Multiplier = 1.0f,
        }).ToList();

    public static List<AttributeEffectEntry> BuildAttributeDefaults() =>
        AttributeMgef.Select(kv => new AttributeEffectEntry
        {
            Attribute = kv.Key,
            MagicEffect = new FormLink<IMagicEffectGetter>(kv.Value),
            Multiplier = 1.0f,
        }).ToList();
}
