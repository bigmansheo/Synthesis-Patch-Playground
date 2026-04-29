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

    [SynthesisSettingName("Fall back to jewelry for slot-less bosses")]
    [SynthesisTooltip("When a boss has no eligible armor slot (dragons, hagravens, dwarven centurions, etc.), build a ring or amulet and add it to the NPC's inventory instead of skipping.")]
    public bool FallbackToJewelry = true;

    [SynthesisSettingName("Fallback jewelry kind")]
    [SynthesisTooltip("Whether the fallback piece is a ring or an amulet.")]
    public JewelryFallbackKind FallbackKind = JewelryFallbackKind.Ring;

    [SynthesisSettingName("Fallback ring base item")]
    [SynthesisTooltip("Base armor used when generating a ring fallback. Defaulted to vanilla GoldRing if unset.")]
    public FormLink<IArmorGetter> FallbackRingBase = new();

    [SynthesisSettingName("Fallback amulet base item")]
    [SynthesisTooltip("Base armor used when generating an amulet fallback. Defaulted to vanilla GoldNecklace if unset.")]
    public FormLink<IArmorGetter> FallbackAmuletBase = new();
}
