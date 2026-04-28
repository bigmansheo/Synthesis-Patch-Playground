using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis.Settings;

namespace BossGearSynthesis.Settings;

public class SisterSetPair
{
    [SynthesisSettingName("Material keyword")]
    public FormLink<IKeywordGetter> Material = new();

    [SynthesisSettingName("Sister material keywords")]
    public List<FormLink<IKeywordGetter>> Sisters = new();
}

public class SisterSetsSettings
{
    [SynthesisSettingName("Sister-set table")]
    [SynthesisTooltip("Each entry says: items with material X may also be drawn from items with material Y/Z (same armor type).")]
    public List<SisterSetPair> Pairs = new();
}
