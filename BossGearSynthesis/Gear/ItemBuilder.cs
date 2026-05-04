using BossGearSynthesis.Analysis;
using BossGearSynthesis.Settings;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis;
using FK = Mutagen.Bethesda.FormKeys.SkyrimSE;

namespace BossGearSynthesis.Gear;

public class ItemBuilder
{
    private static readonly FormKey MagicDisallowEnchantingKeyword =
        FK.Skyrim.Keyword.MagicDisallowEnchanting.FormKey;

    private readonly Settings.Settings _settings;

    public ItemBuilder(Settings.Settings settings) { _settings = settings; }

    public Armor Build(
        IPatcherState<ISkyrimMod, ISkyrimModGetter> state,
        INpcGetter boss,
        IArmorGetter baseArmor,
        ObjectEffect ench,
        BossStrengthProfile profile,
        float magnitude)
    {
        var copy = state.PatchMod.Armors.DuplicateInAsNewRecord(baseArmor);
        copy.EditorID = $"BossGear_ARMO_{Slug(boss)}_{Slug(baseArmor)}";
        copy.ObjectEffect.SetTo(ench);

        var bumpUnits = (int)Math.Round(magnitude / 10f);
        copy.Value = (uint)(copy.Value + bumpUnits * _settings.GearSelection.ValueBumpPer10Magnitude);

        copy.Name = RenderName(boss, baseArmor, profile);

        if (_settings.GearSelection.PreventDisenchant)
            EnsureKeyword(copy, MagicDisallowEnchantingKeyword);

        return copy;
    }

    /// <summary>
    /// Adds <paramref name="keyword"/> to <paramref name="armor"/>'s keyword list if not
    /// already present. Skyrim's arcane enchanter excludes any item carrying
    /// MagicDisallowEnchanting from the disenchant menu (this is what vanilla unique gear
    /// like Spellbreaker and Auriel's Bow uses), so this is the reliable way to keep
    /// boss gear off the list — independent of the BaseEnchantment self-pointer trick on
    /// the ObjectEffect record.
    /// </summary>
    private static void EnsureKeyword(Armor armor, FormKey keyword)
    {
        armor.Keywords ??= new Noggog.ExtendedList<IFormLinkGetter<IKeywordGetter>>();
        foreach (var existing in armor.Keywords)
            if (existing.FormKey == keyword) return;
        armor.Keywords.Add(new FormLink<IKeywordGetter>(keyword));
    }

    private string RenderName(INpcGetter boss, IArmorGetter baseArmor, BossStrengthProfile profile)
    {
        var bossName = boss.Name?.String ?? boss.EditorID ?? "Boss";
        var baseName = baseArmor.Name?.String ?? baseArmor.EditorID ?? "Gear";
        var effectShort = profile.TopCombatSkill.ToString();

        var name = _settings.Naming.Template
            .Replace("{BossName}", bossName)
            .Replace("{BaseItemName}", baseName)
            .Replace("{Effect1ShortName}", effectShort)
            .Replace("{Effect2ShortName}", profile.SecondSkill?.ToString() ?? profile.TopAttribute?.ToString() ?? "");

        if (_settings.Naming.AppendEffectSuffix)
            name = $"{name} of {effectShort}";
        return name;
    }

    private static string Slug(INpcGetter npc) => npc.EditorID ?? npc.FormKey.ID.ToString("X8");
    private static string Slug(IArmorGetter armor) => armor.EditorID ?? armor.FormKey.ID.ToString("X8");
}
