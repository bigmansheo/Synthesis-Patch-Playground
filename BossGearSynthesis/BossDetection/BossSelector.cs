using BossGearSynthesis.Settings;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Order;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis;

namespace BossGearSynthesis.BossDetection;

public class BossSelector
{
    private readonly IPatcherState<ISkyrimMod, ISkyrimModGetter> _state;
    private readonly Settings.Settings _settings;
    private readonly HashSet<FormKey> _explicitAllow;
    private readonly HashSet<FormKey> _explicitBlock;
    private readonly HashSet<FormKey> _namedUniqueAllow;
    private readonly HashSet<FormKey> _raceAllow;
    private readonly HashSet<FormKey> _keywordAllow;
    private readonly string[] _editorIdPrefixes;
    private readonly HashSet<string> _trustedModKeys;
    private readonly HashSet<string> _pluginSourceAllow;

    /// <summary>
    /// NPCs that were considered but rejected, with the reason. Populated
    /// by <see cref="Select"/> for the dry-run report. Capped to keep the
    /// report readable.
    /// </summary>
    public List<(INpcGetter Npc, string Reason)> RejectedSamples { get; } = new();

    /// <summary>Total number of NPCs rejected, even if not all are sampled.</summary>
    public int RejectedCount { get; private set; }

    private const int RejectedSampleCap = 100;

    public BossSelector(IPatcherState<ISkyrimMod, ISkyrimModGetter> state, Settings.Settings settings)
    {
        _state = state;
        _settings = settings;
        _explicitAllow = settings.BossDetection.ExplicitAllowlist.Select(l => l.FormKey).ToHashSet();
        _explicitBlock = settings.BossDetection.ExplicitBlocklist.Select(l => l.FormKey).ToHashSet();
        _namedUniqueAllow = settings.BossDetection.NamedUniqueAllowlist.Select(l => l.FormKey).ToHashSet();
        _raceAllow = settings.BossDetection.RaceAllowlist.Select(l => l.FormKey).ToHashSet();
        _keywordAllow = settings.BossDetection.KeywordAllowlist.Select(l => l.FormKey).ToHashSet();
        _editorIdPrefixes = settings.BossDetection.EditorIdPrefixWhitelist
            .Where(p => !string.IsNullOrWhiteSpace(p))
            .Select(p => p.Trim())
            .ToArray();
        _trustedModKeys = ToModKeySet(settings.BossDetection.TrustedUniqueModKeys);
        _pluginSourceAllow = ToModKeySet(settings.BossDetection.PluginSourceAllowlist);
    }

    public IEnumerable<BossMatch> Select()
    {
        var bosses = new List<BossMatch>();
        foreach (var npc in _state.LoadOrder.PriorityOrder.Npc().WinningOverrides())
        {
            if (_explicitBlock.Contains(npc.FormKey))
            {
                Reject(npc, "explicit blocklist");
                continue;
            }
            if (!IsFromAllowedPlugin(npc))
            {
                Reject(npc, "plugin not in source allowlist");
                continue;
            }

            var match = Match(npc);
            if (match is not null)
                bosses.Add(match);
            else
                Reject(npc, "no whitelist matcher fired");
        }
        return bosses.OrderBy(m => m.Npc.FormKey.ModKey.FileName.String, StringComparer.OrdinalIgnoreCase)
                     .ThenBy(m => m.Npc.FormKey.ID);
    }

    public bool IsNamedUnique(INpcGetter npc) => _namedUniqueAllow.Contains(npc.FormKey);

    private BossMatch? Match(INpcGetter npc)
    {
        // Explicit user-curated lists fire for any plugin — the user named these NPCs themselves.
        if (_explicitAllow.Contains(npc.FormKey))
            return new BossMatch(npc, MatchReason.ExplicitAllow, "");
        if (_namedUniqueAllow.Contains(npc.FormKey))
            return new BossMatch(npc, MatchReason.NamedUniqueAllow, "");

        // Heuristic matchers. In whitelist-only mode they are gated to NPCs
        // originating from a trusted plugin so that random modded creatures
        // (modded dragons, modded "EncBanditChief..." variants, etc.) do
        // not get patched unless the user has explicitly opted them in.
        bool gateHeuristics = _settings.BossDetection.UseDefaultWhitelistOnly && !IsTrustedPlugin(npc);
        if (gateHeuristics) return null;

        if (_raceAllow.Count > 0 && !npc.Race.IsNull && _raceAllow.Contains(npc.Race.FormKey))
            return new BossMatch(npc, MatchReason.Race, npc.Race.FormKey.ToString());

        if (_keywordAllow.Count > 0 && npc.Keywords is { } kws)
        {
            foreach (var k in kws)
                if (_keywordAllow.Contains(k.FormKey))
                    return new BossMatch(npc, MatchReason.Keyword, k.FormKey.ToString());
        }

        var prefix = WhitelistMatcher.FirstMatchingPrefix(npc.EditorID, _editorIdPrefixes);
        if (prefix is not null)
            return new BossMatch(npc, MatchReason.EditorIdPrefix, prefix);

        if (WhitelistMatcher.ShouldFireUniqueFlagFallback(
                whitelistOnly: _settings.BossDetection.UseDefaultWhitelistOnly,
                useUniqueFlag: _settings.BossDetection.UseUniqueFlag,
                npcIsUnique: (npc.Configuration.Flags & NpcConfiguration.Flag.Unique) != 0,
                npcHasName: HasName(npc),
                npcFromTrustedPlugin: IsTrustedPlugin(npc)))
            return new BossMatch(npc, MatchReason.UniqueFlag, "");

        return null;
    }

    private void Reject(INpcGetter npc, string reason)
    {
        RejectedCount++;
        if (RejectedSamples.Count < RejectedSampleCap)
            RejectedSamples.Add((npc, reason));
    }

    private bool IsFromAllowedPlugin(INpcGetter npc) =>
        WhitelistMatcher.IsPluginAllowed(npc.FormKey.ModKey.FileName.String, _pluginSourceAllow);

    private bool IsTrustedPlugin(INpcGetter npc)
    {
        if (_trustedModKeys.Count == 0) return true;
        return _trustedModKeys.Contains(npc.FormKey.ModKey.FileName.String);
    }

    private static HashSet<string> ToModKeySet(IEnumerable<string> values) =>
        new(values.Where(s => !string.IsNullOrWhiteSpace(s)), StringComparer.OrdinalIgnoreCase);

    private static bool HasName(INpcGetter npc) =>
        !string.IsNullOrWhiteSpace(npc.Name?.String);
}
