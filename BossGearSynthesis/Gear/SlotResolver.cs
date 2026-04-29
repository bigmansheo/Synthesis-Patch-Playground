using BossGearSynthesis.Settings;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis;

namespace BossGearSynthesis.Gear;

/// <summary>
/// Describes which armor in the boss's outfit will be replaced (or, when
/// <see cref="AppendInsteadOfReplace"/> is true, what new piece will be appended
/// to the outfit because the boss had no wearable slot at all).
/// </summary>
public record SlotPick(
    BipedObjectFlag Slot,
    IArmorGetter Item,
    IOutfitGetter Outfit,
    bool AppendInsteadOfReplace = false);

public class SlotResolver
{
    private readonly Settings.Settings _settings;

    public SlotResolver(Settings.Settings settings) { _settings = settings; }

    public SlotPick? PickSlot(INpcGetter boss, IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
    {
        if (boss.DefaultOutfit.IsNull) return null;
        if (!state.LinkCache.TryResolve<IOutfitGetter>(boss.DefaultOutfit.FormKey, out var outfit)) return null;

        // Resolve every armor in the outfit, indexed by the slot it occupies.
        var bySlot = new Dictionary<BipedObjectFlag, IArmorGetter>();
        foreach (var itemLink in outfit.Items)
        {
            if (!state.LinkCache.TryResolve<IArmorGetter>(itemLink.FormKey, out var armor)) continue;
            if (armor.BodyTemplate is null) continue;
            foreach (var slot in EnumerateSlots(armor.BodyTemplate.FirstPersonFlags))
            {
                if (!bySlot.ContainsKey(slot)) bySlot[slot] = armor;
            }
        }

        foreach (var preferred in _settings.GearSelection.SlotPriority)
        {
            if (!bySlot.TryGetValue(preferred, out var armor)) continue;
            if (!_settings.GearSelection.OverwriteEnchanted && !armor.ObjectEffect.IsNull) continue;
            return new SlotPick(preferred, armor, outfit);
        }

        // No wearable slot resolved (e.g. dragons whose outfit only contains crafting drops).
        // Append a ring or amulet based on the configured fallback so the boss still drops something.
        if (_settings.GearSelection.EnableNoSlotFallback)
        {
            return BuildFallbackPick(outfit, state);
        }

        return null;
    }

    private SlotPick? BuildFallbackPick(IOutfitGetter outfit, IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
    {
        var kind = _settings.GearSelection.NoSlotFallbackKind;
        var link = kind == NoSlotFallbackKind.Amulet
            ? _settings.GearSelection.NoSlotFallbackAmulet
            : _settings.GearSelection.NoSlotFallbackRing;
        if (link.IsNull) return null;
        if (!state.LinkCache.TryResolve<IArmorGetter>(link.FormKey, out var baseArmor)) return null;

        var slot = kind == NoSlotFallbackKind.Amulet ? BipedObjectFlag.Amulet : BipedObjectFlag.Ring;
        return new SlotPick(slot, baseArmor, outfit, AppendInsteadOfReplace: true);
    }

    private static IEnumerable<BipedObjectFlag> EnumerateSlots(BipedObjectFlag flags)
    {
        foreach (BipedObjectFlag f in Enum.GetValues<BipedObjectFlag>())
            if (f != 0 && (flags & f) == f) yield return f;
    }
}
