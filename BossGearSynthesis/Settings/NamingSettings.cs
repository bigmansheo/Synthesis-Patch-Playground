using Mutagen.Bethesda.Synthesis.Settings;

namespace BossGearSynthesis.Settings;

public class NamingSettings
{
    [SynthesisSettingName("Name template")]
    [SynthesisTooltip("Tokens: {BossName}, {BaseItemName}, {Effect1ShortName}, {Effect2ShortName}.")]
    public string Template = "{BossName}'s {BaseItemName}";

    [SynthesisSettingName("Append \" of <Effect1>\" suffix")]
    public bool AppendEffectSuffix = false;
}
