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
        _trustedModKeys = new HashSet<string>(
            settings.BossDetection.TrustedUniqueModKeys.Where(s => !string.IsNullOrWhiteSpace(s)),
            StringComparer.OrdinalIgnoreCase);
    }

    public IEnumerable<INpcGetter> Select()
    {
        var bosses = new List<INpcGetter>();
        foreach (var npc in _state.LoadOrder.PriorityOrder.Npc().WinningOverrides())
        {
            if (_explicitBlock.Contains(npc.FormKey)) continue;
            if (IsBoss(npc)) bosses.Add(npc);
        }
        // Deterministic order
        return bosses.OrderBy(n => n.FormKey.ModKey.FileName.String, StringComparer.OrdinalIgnoreCase)
                     .ThenBy(n => n.FormKey.ID);
    }

    public bool IsNamedUnique(INpcGetter npc) => _namedUniqueAllow.Contains(npc.FormKey);

    private bool IsBoss(INpcGetter npc)
    {
        if (_explicitAllow.Contains(npc.FormKey)) return true;
        if (_namedUniqueAllow.Contains(npc.FormKey)) return true;

        if (_raceAllow.Count > 0 && !npc.Race.IsNull && _raceAllow.Contains(npc.Race.FormKey))
            return true;

        if (_keywordAllow.Count > 0 && npc.Keywords is { } kws && kws.Any(k => _keywordAllow.Contains(k.FormKey)))
            return true;

        if (_editorIdPrefixes.Length > 0 && npc.EditorID is { } eid)
        {
            foreach (var prefix in _editorIdPrefixes)
                if (eid.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    return true;
        }

        if (_settings.BossDetection.UseUniqueFlag &&
            (npc.Configuration.Flags & NpcConfiguration.Flag.Unique) != 0 &&
            HasName(npc) &&
            UniqueFlagAllowed(npc))
            return true;

        return false;
    }

    private bool UniqueFlagAllowed(INpcGetter npc)
    {
        if (!_settings.BossDetection.UseDefaultWhitelistOnly) return true;
        if (_trustedModKeys.Count == 0) return true;
        return _trustedModKeys.Contains(npc.FormKey.ModKey.FileName.String);
    }

    private static bool HasName(INpcGetter npc) =>
        !string.IsNullOrWhiteSpace(npc.Name?.String);
}
