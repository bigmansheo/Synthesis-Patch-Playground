using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis.Settings;

namespace BossGearSynthesis.Settings;

public enum MaterialUpgradeMode
{
    Off,
    SisterSet,
    StrongerInFamily,
}

public enum JewelryFallbackKind
{
    Ring,
    Amulet,
}

public class GearSelectionSettings
{
    [SynthesisSettingName("Slot priority")]
    [SynthesisTooltip("Priority order; first slot in the boss's outfit that matches wins.")]
    public List<BipedObjectFlag> SlotPriority = new()
    {
        BipedObjectFlag.Head,
        BipedObjectFlag.Body,
        BipedObjectFlag.Hands,
        BipedObjectFlag.Feet,
        BipedObjectFlag.Amulet,
        BipedObjectFlag.Ring,
        BipedObjectFlag.Circlet,
    };

    [SynthesisSettingName("Overwrite already-enchanted items")]
    [SynthesisTooltip("If false, slots whose item already has an enchantment are skipped.")]
    public bool OverwriteEnchanted = false;

    [SynthesisSettingName("Material upgrade mode")]
    public MaterialUpgradeMode MaterialUpgrade = MaterialUpgradeMode.Off;

    [SynthesisSettingName("Value bump per 10 magnitude")]
    [SynthesisTooltip("Cosmetic loot value bump.")]
    public int ValueBumpPer10Magnitude = 50;

    [SynthesisSettingName("Jewelry fallback for slot-less bosses")]
    [SynthesisTooltip("When a boss has no outfit or no eligible biped slot (dragons, some creatures), " +
                      "build a ring or amulet from the bases below and add it to the NPC's inventory " +
                      "so it drops on death.")]
    public bool JewelryFallbackEnabled = true;

    [SynthesisSettingName("Jewelry fallback kind")]
    [SynthesisTooltip("Whether the fallback drop is a ring or an amulet.")]
    public JewelryFallbackKind JewelryFallbackKind = JewelryFallbackKind.Ring;

    [SynthesisSettingName("Jewelry fallback ring base")]
    [SynthesisTooltip("Armor used as the base when the fallback kind is Ring. " +
                      "Defaults to vanilla Silver Ring if left empty.")]
    public FormLink<IArmorGetter> JewelryFallbackRing = new();

    [SynthesisSettingName("Jewelry fallback amulet base")]
    [SynthesisTooltip("Armor used as the base when the fallback kind is Amulet. " +
                      "Defaults to vanilla Gold Necklace if left empty.")]
    public FormLink<IArmorGetter> JewelryFallbackAmulet = new();
}
