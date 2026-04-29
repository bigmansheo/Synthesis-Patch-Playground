using BossGearSynthesis.Gear;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis;

namespace BossGearSynthesis.Outfit;

public class OutfitPatcher
{
    public void Apply(
        IPatcherState<ISkyrimMod, ISkyrimModGetter> state,
        INpcGetter boss,
        SlotPick pick,
        Armor newItem)
    {
        if (pick.Outfit is null) throw new InvalidOperationException("OutfitPatcher.Apply requires a non-null outfit; use AddToInventory for slot-less bosses.");
        var newOutfit = state.PatchMod.Outfits.DuplicateInAsNewRecord(pick.Outfit);
        newOutfit.EditorID = $"BossGear_OTFT_{Slug(boss)}";

        var oldKey = pick.Item.FormKey;
        for (int i = 0; i < newOutfit.Items.Count; i++)
        {
            if (newOutfit.Items[i].FormKey == oldKey)
            {
                newOutfit.Items[i] = new FormLink<IOutfitTargetGetter>(newItem.FormKey);
            }
        }

        var npcOverride = state.PatchMod.Npcs.GetOrAddAsOverride(boss);
        npcOverride.DefaultOutfit.SetTo(newOutfit);
    }

    /// <summary>
    /// Inventory drop for bosses with no usable outfit slot (dragons, some creatures).
    /// Adds a single copy of <paramref name="newItem"/> to the NPC's carried inventory
    /// so it lands on the corpse on death.
    /// </summary>
    public void AddToInventory(
        IPatcherState<ISkyrimMod, ISkyrimModGetter> state,
        INpcGetter boss,
        Armor newItem)
    {
        var npcOverride = state.PatchMod.Npcs.GetOrAddAsOverride(boss);
        npcOverride.Items ??= new();
        npcOverride.Items.Add(new ContainerEntry
        {
            Item = new ContainerItem
            {
                Item = new FormLink<IItemGetter>(newItem.FormKey),
                Count = 1,
            },
        });
    }

    private static string Slug(INpcGetter npc) => npc.EditorID ?? npc.FormKey.ID.ToString("X8");
}
