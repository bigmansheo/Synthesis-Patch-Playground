using BossGearSynthesis.Analysis;
using BossGearSynthesis.Gear;
using Mutagen.Bethesda.Skyrim;
using System.Text;

namespace BossGearSynthesis.Logging;

public class DryRunReport
{
    private readonly StringBuilder _patched = new();
    private readonly StringBuilder _skipped = new();
    private int _patchedCount;
    private int _skippedCount;

    public DryRunReport()
    {
        _patched.AppendLine("| Boss | Tier | Top combat skill | Effect 2 | Slot | Magnitude |");
        _patched.AppendLine("|------|------|------------------|----------|------|-----------|");
        _skipped.AppendLine("| Boss | Reason |");
        _skipped.AppendLine("|------|--------|");
    }

    public void Patched(INpcGetter boss, BossStrengthProfile profile, SlotPick pick, float magnitude)
    {
        _patchedCount++;
        var name = boss.Name?.String ?? boss.EditorID ?? boss.FormKey.ToString();
        var effect2 = profile.UseAttributeForEffect2
            ? profile.TopAttribute?.ToString() ?? "-"
            : profile.SecondSkill?.ToString() ?? "-";
        _patched.AppendLine($"| {name} | {profile.Tier} | {profile.TopCombatSkill} | {effect2} | {pick.Slot} | {magnitude:F1} |");
    }

    public void Skipped(INpcGetter boss, string reason)
    {
        _skippedCount++;
        var name = boss.Name?.String ?? boss.EditorID ?? boss.FormKey.ToString();
        _skipped.AppendLine($"| {name} | {reason} |");
    }

    public void Write(string outputPath)
    {
        var doc = new StringBuilder();
        doc.AppendLine("# BossGearSynthesis report");
        doc.AppendLine();
        doc.AppendLine($"- Patched: {_patchedCount}");
        doc.AppendLine($"- Skipped: {_skippedCount}");
        doc.AppendLine();
        doc.AppendLine("## Patched");
        doc.AppendLine();
        doc.AppendLine(_patched.ToString());
        doc.AppendLine("## Skipped");
        doc.AppendLine();
        doc.AppendLine(_skipped.ToString());
        File.WriteAllText(outputPath, doc.ToString());
    }
}
