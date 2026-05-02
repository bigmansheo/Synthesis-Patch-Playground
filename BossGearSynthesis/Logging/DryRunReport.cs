using BossGearSynthesis.Analysis;
using BossGearSynthesis.BossDetection;
using BossGearSynthesis.Gear;
using Mutagen.Bethesda.Skyrim;
using System.Text;

namespace BossGearSynthesis.Logging;

public class DryRunReport
{
    private readonly StringBuilder _patched = new();
    private readonly StringBuilder _skipped = new();
    private readonly StringBuilder _rejected = new();
    private int _patchedCount;
    private int _skippedCount;
    private int _rejectedCount;
    private int _rejectedSampled;

    private readonly Dictionary<MatchReason, int> _reasonCounts = new();
    private readonly Dictionary<string, int> _patchedByPlugin = new(StringComparer.OrdinalIgnoreCase);

    public DryRunReport()
    {
        _patched.AppendLine("| Boss | Plugin | Match reason | Detail | Tier | Top combat skill | Effect 2 | Slot | Magnitude |");
        _patched.AppendLine("|------|--------|--------------|--------|------|------------------|----------|------|-----------|");
        _skipped.AppendLine("| Boss | Plugin | Match reason | Reason skipped |");
        _skipped.AppendLine("|------|--------|--------------|----------------|");
        _rejected.AppendLine("| NPC | Plugin | Filter |");
        _rejected.AppendLine("|-----|--------|--------|");
    }

    public void Patched(BossMatch match, BossStrengthProfile profile, SlotPick pick, float magnitude)
    {
        _patchedCount++;
        Tally(match);
        var name = NameOf(match.Npc);
        var plugin = PluginOf(match.Npc);
        var effect2 = profile.UseAttributeForEffect2
            ? profile.TopAttribute?.ToString() ?? "-"
            : profile.SecondSkill?.ToString() ?? "-";
        _patched.AppendLine(
            $"| {name} | {plugin} | {match.Reason} | {EscapeDetail(match.Detail)} | {profile.Tier} | {profile.TopCombatSkill} | {effect2} | {pick.Slot} | {magnitude:F1} |");
    }

    public void Skipped(BossMatch match, string reason)
    {
        _skippedCount++;
        var name = NameOf(match.Npc);
        var plugin = PluginOf(match.Npc);
        _skipped.AppendLine($"| {name} | {plugin} | {match.Reason} | {reason} |");
    }

    public void RecordRejections(IEnumerable<(INpcGetter Npc, string Reason)> samples, int totalCount)
    {
        _rejectedCount = totalCount;
        foreach (var (npc, reason) in samples)
        {
            _rejectedSampled++;
            _rejected.AppendLine($"| {NameOf(npc)} | {PluginOf(npc)} | {reason} |");
        }
    }

    public void Write(string outputPath)
    {
        var doc = new StringBuilder();
        doc.AppendLine("# BossGearSynthesis report");
        doc.AppendLine();
        doc.AppendLine("## Summary");
        doc.AppendLine();
        doc.AppendLine($"- Patched: **{_patchedCount}**");
        doc.AppendLine($"- Skipped (matched but unprocessable): **{_skippedCount}**");
        doc.AppendLine($"- Rejected (did not match any whitelist matcher): **{_rejectedCount}** ({_rejectedSampled} sampled below)");
        doc.AppendLine();

        if (_reasonCounts.Count > 0)
        {
            doc.AppendLine("### Patched-by-matcher");
            doc.AppendLine();
            doc.AppendLine("| Match reason | Count |");
            doc.AppendLine("|--------------|-------|");
            foreach (var kv in _reasonCounts.OrderByDescending(kv => kv.Value))
                doc.AppendLine($"| {kv.Key} | {kv.Value} |");
            doc.AppendLine();
        }

        if (_patchedByPlugin.Count > 0)
        {
            doc.AppendLine("### Patched-by-plugin");
            doc.AppendLine();
            doc.AppendLine("| Plugin | Count |");
            doc.AppendLine("|--------|-------|");
            foreach (var kv in _patchedByPlugin.OrderByDescending(kv => kv.Value))
                doc.AppendLine($"| {kv.Key} | {kv.Value} |");
            doc.AppendLine();
        }

        doc.AppendLine("## Patched");
        doc.AppendLine();
        doc.AppendLine(_patched.ToString());
        doc.AppendLine("## Skipped");
        doc.AppendLine();
        doc.AppendLine(_skipped.ToString());
        doc.AppendLine("## Rejected (sampled)");
        doc.AppendLine();
        doc.AppendLine(_rejected.ToString());
        File.WriteAllText(outputPath, doc.ToString());
    }

    private void Tally(BossMatch match)
    {
        _reasonCounts[match.Reason] = _reasonCounts.GetValueOrDefault(match.Reason) + 1;
        var plugin = PluginOf(match.Npc);
        _patchedByPlugin[plugin] = _patchedByPlugin.GetValueOrDefault(plugin) + 1;
    }

    private static string NameOf(INpcGetter npc) =>
        npc.Name?.String ?? npc.EditorID ?? npc.FormKey.ToString();

    private static string PluginOf(INpcGetter npc) =>
        npc.FormKey.ModKey.FileName.String;

    private static string EscapeDetail(string detail) =>
        string.IsNullOrEmpty(detail) ? "-" : detail.Replace("|", "\\|");
}
