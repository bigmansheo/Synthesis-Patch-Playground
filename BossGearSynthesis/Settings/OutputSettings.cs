using Mutagen.Bethesda.Synthesis.Settings;

namespace BossGearSynthesis.Settings;

public class OutputSettings
{
    [SynthesisSettingName("Dry run")]
    [SynthesisTooltip("If true, no records are written; the report still logs picks for every boss.")]
    public bool DryRun = false;

    [SynthesisSettingName("Verbose log")]
    public bool VerboseLog = false;

    [SynthesisSettingName("Report file name")]
    [SynthesisTooltip("Markdown report written next to the patch ESP.")]
    public string ReportFileName = "BossGearSynthesis.report.md";
}
