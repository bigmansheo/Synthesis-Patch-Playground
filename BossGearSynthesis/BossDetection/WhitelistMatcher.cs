namespace BossGearSynthesis.BossDetection;

/// <summary>
/// Pure string matching helpers used by <see cref="BossSelector"/>.
/// Pulled out so they can be unit-tested without instantiating
/// the full Mutagen pipeline.
/// </summary>
public static class WhitelistMatcher
{
    /// <summary>
    /// Returns true if <paramref name="editorId"/> starts with any of <paramref name="prefixes"/>
    /// (case-insensitive, ignoring blank prefixes). Returns false for null/empty inputs.
    /// </summary>
    public static bool MatchesEditorIdPrefix(string? editorId, IEnumerable<string> prefixes)
    {
        if (string.IsNullOrEmpty(editorId)) return false;
        foreach (var prefix in prefixes)
        {
            if (string.IsNullOrWhiteSpace(prefix)) continue;
            if (editorId.StartsWith(prefix.Trim(), StringComparison.OrdinalIgnoreCase))
                return true;
        }
        return false;
    }

    /// <summary>
    /// Returns true if <paramref name="pluginFileName"/> is in <paramref name="allowed"/>
    /// (case-insensitive). An empty allowlist means "any plugin allowed".
    /// </summary>
    public static bool IsPluginAllowed(string pluginFileName, IReadOnlyCollection<string> allowed)
    {
        if (allowed.Count == 0) return true;
        foreach (var name in allowed)
        {
            if (string.Equals(name, pluginFileName, StringComparison.OrdinalIgnoreCase))
                return true;
        }
        return false;
    }
}
