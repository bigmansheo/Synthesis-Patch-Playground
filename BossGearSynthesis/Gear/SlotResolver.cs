using BossGearSynthesis.Settings;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis;

namespace BossGearSynthesis.Gear;

public record SlotPick(BipedObjectFlag Slot, IArmorGetter Item, IOutfitGetter? Outfit);

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
        return null;
    }

    private static IEnumerable<BipedObjectFlag> EnumerateSlots(BipedObjectFlag flags)
    {
        foreach (BipedObjectFlag f in Enum.GetValues<BipedObjectFlag>())
            if (f != 0 && (flags & f) == f) yield return f;
    }
}
