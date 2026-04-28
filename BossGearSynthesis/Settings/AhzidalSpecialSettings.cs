using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis.Settings;

namespace BossGearSynthesis.Settings;

public class AhzidalSpecialSettings
{
    [SynthesisSettingName("Enabled")]
    public bool Enabled = true;

    [SynthesisSettingName("Ahzidal NPC")]
    [SynthesisTooltip("If unset, defaulted to Dragonborn.esm Ahzidal at runtime.")]
    public FormLink<INpcGetter> Ahzidal = new();

    [SynthesisSettingName("Extra effect (Fortify Enchanting MGEF)")]
    [SynthesisTooltip("If unset, defaulted to vanilla Fortify Enchanting at runtime.")]
    public FormLink<IMagicEffectGetter> ExtraEffectMgef = new();

    [SynthesisSettingName("Extra effect magnitude")]
    public float Magnitude = 10f;
}
