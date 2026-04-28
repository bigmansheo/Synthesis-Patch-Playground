using BossGearSynthesis.Settings;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;

namespace BossGearSynthesis.Defaults;

/// <summary>Default Skill -> MGEF and Attribute -> MGEF mappings for vanilla Skyrim.esm.</summary>
public static class EffectMapDefaults
{
    private static readonly ModKey Skyrim = ModKey.FromNameAndExtension("Skyrim.esm");
    private static FormKey K(uint id) => new(Skyrim, id);

    // Vanilla Fortify <Skill> "ApparelEnchantment" MGEFs (the ones used by armor enchantments).
    // FormIDs are stable across Skyrim.esm and identified by EditorID.
    public static readonly IReadOnlyDictionary<Skill, FormKey> SkillMgef = new Dictionary<Skill, FormKey>
    {
        { Skill.OneHanded,    K(0x0BABE4) }, // EnchFortifyOneHandedConstantSelf
        { Skill.TwoHanded,    K(0x0BABE5) }, // EnchFortifyTwoHandedConstantSelf
        { Skill.Archery,      K(0x0BABE7) }, // EnchFortifyMarksmanConstantSelf
        { Skill.Block,        K(0x0BABE6) }, // EnchFortifyBlockConstantSelf
        { Skill.Smithing,     K(0x10FC15) }, // EnchFortifySmithingConstantSelf
        { Skill.HeavyArmor,   K(0x0BABEA) }, // EnchFortifyHeavyArmorConstantSelf
        { Skill.LightArmor,   K(0x0BABEB) }, // EnchFortifyLightArmorConstantSelf
        { Skill.Pickpocket,   K(0x0BABEC) }, // EnchFortifyPickpocketConstantSelf
        { Skill.Lockpicking,  K(0x0BABED) }, // EnchFortifyLockpickingConstantSelf
        { Skill.Sneak,        K(0x0BABEE) }, // EnchFortifySneakConstantSelf
        { Skill.Alchemy,      K(0x0BABEF) }, // EnchFortifyAlchemyConstantSelf
        { Skill.Speech,       K(0x0BABF0) }, // EnchFortifySpeechConstantSelf
        { Skill.Alteration,   K(0x0BABF1) }, // EnchFortifyAlterationConstantSelf
        { Skill.Conjuration,  K(0x0BABF2) }, // EnchFortifyConjurationConstantSelf
        { Skill.Destruction,  K(0x0BABF3) }, // EnchFortifyDestructionConstantSelf
        { Skill.Illusion,     K(0x0BABF4) }, // EnchFortifyIllusionConstantSelf
        { Skill.Restoration,  K(0x0BABF5) }, // EnchFortifyRestorationConstantSelf
        { Skill.Enchanting,   K(0x107A4D) }, // EnchFortifyEnchantingConstantSelf
    };

    public static readonly IReadOnlyDictionary<BasicAttribute, FormKey> AttributeMgef =
        new Dictionary<BasicAttribute, FormKey>
        {
            { BasicAttribute.Health,  K(0x0AD402) }, // EnchFortifyHealthConstantSelf
            { BasicAttribute.Magicka, K(0x0AD409) }, // EnchFortifyMagickaConstantSelf
            { BasicAttribute.Stamina, K(0x0AD410) }, // EnchFortifyStaminaConstantSelf
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
