using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis.Settings;

namespace BossGearSynthesis.Settings;

public enum MaterialUpgradeMode
{
    Off,
    SisterSet,
    StrongerInFamily,
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
}
