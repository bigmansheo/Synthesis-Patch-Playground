using BossGearSynthesis.Settings;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis;

namespace BossGearSynthesis.Gear;

public class MaterialMatcher
{
    private readonly Settings.Settings _settings;
    private readonly Dictionary<FormKey, HashSet<FormKey>> _sisterIndex;

    public MaterialMatcher(Settings.Settings settings)
    {
        _settings = settings;
        _sisterIndex = BuildSisterIndex(settings.SisterSets);
    }

    /// <summary>
    /// Returns the armor to actually clone. Default mode just returns <paramref name="picked"/>.
    /// </summary>
    public IArmorGetter Choose(IArmorGetter picked, BipedObjectFlag slot, IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
    {
        if (_settings.GearSelection.MaterialUpgrade == MaterialUpgradeMode.Off)
            return picked;

        var pickedMaterial = ResolveMaterialKeyword(picked);
        if (pickedMaterial is null) return picked;

        var family = ResolveFamily(pickedMaterial.Value);
        var armorType = picked.BodyTemplate?.ArmorType ?? ArmorType.LightArmor;

        var candidates = state.LoadOrder.PriorityOrder.Armor().WinningOverrides()
            .Where(a => a.BodyTemplate is { } bt && bt.ArmorType == armorType)
            .Where(a => a.BodyTemplate is { } bt && (bt.FirstPersonFlags & slot) == slot)
            .Where(a => a.Keywords?.Any(k => family.Contains(k.FormKey)) == true)
            .ToList();
        if (candidates.Count == 0) return picked;

        return _settings.GearSelection.MaterialUpgrade switch
        {
            MaterialUpgradeMode.SisterSet => SeededPick(candidates, picked.FormKey),
            MaterialUpgradeMode.StrongerInFamily => candidates
                .OrderByDescending(a => a.ArmorRating)
                .ThenBy(a => a.FormKey.ID)
                .First(),
            _ => picked,
        };
    }

    private FormKey? ResolveMaterialKeyword(IArmorGetter armor)
    {
        if (armor.Keywords is null) return null;
        // Heuristic: any keyword whose FormKey appears in the sister-set table is a material keyword.
        foreach (var kw in armor.Keywords)
            if (_sisterIndex.ContainsKey(kw.FormKey)) return kw.FormKey;
        return null;
    }

    private HashSet<FormKey> ResolveFamily(FormKey material)
    {
        var set = new HashSet<FormKey> { material };
        if (_sisterIndex.TryGetValue(material, out var sisters))
            foreach (var s in sisters) set.Add(s);
        return set;
    }

    private static IArmorGetter SeededPick(IList<IArmorGetter> options, FormKey seed)
    {
        var idx = (int)(seed.ID % (uint)options.Count);
        return options[idx];
    }

    private static Dictionary<FormKey, HashSet<FormKey>> BuildSisterIndex(SisterSetsSettings settings)
    {
        var idx = new Dictionary<FormKey, HashSet<FormKey>>();
        foreach (var pair in settings.Pairs)
        {
            if (pair.Material.IsNull) continue;
            var key = pair.Material.FormKey;
            if (!idx.TryGetValue(key, out var set)) idx[key] = set = new HashSet<FormKey>();
            foreach (var s in pair.Sisters)
            {
                if (s.IsNull) continue;
                set.Add(s.FormKey);
                if (!idx.TryGetValue(s.FormKey, out var rev)) idx[s.FormKey] = rev = new HashSet<FormKey>();
                rev.Add(key);
            }
        }
        return idx;
    }
}
