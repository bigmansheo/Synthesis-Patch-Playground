using BossGearSynthesis.Settings;
using Mutagen.Bethesda.Skyrim;

namespace BossGearSynthesis.Analysis;

public class TierClassifier
{
    private readonly Settings.Settings _settings;

    public TierClassifier(Settings.Settings settings) { _settings = settings; }

    public BossTier Classify(INpcGetter npc, bool isNamedUnique, int effectiveLevel)
    {
        if (isNamedUnique) return BossTier.Unique;

        foreach (var rule in _settings.MagnitudeScaling.ClassificationRules)
        {
            if (rule.MinLevel > 0 && effectiveLevel < rule.MinLevel) continue;
            if (rule.MaxLevel > 0 && effectiveLevel > rule.MaxLevel) continue;

            switch (rule.Matcher)
            {
                case TierMatcherKind.Faction:
                    if (!rule.Faction.IsNull && npc.Factions.Any(f => f.Faction.FormKey == rule.Faction.FormKey))
                        return rule.Tier;
                    break;
                case TierMatcherKind.Race:
                    if (!rule.Race.IsNull && npc.Race.FormKey == rule.Race.FormKey)
                        return rule.Tier;
                    break;
                case TierMatcherKind.Keyword:
                    if (!rule.Keyword.IsNull && npc.Keywords is { } kws &&
                        kws.Any(k => k.FormKey == rule.Keyword.FormKey))
                        return rule.Tier;
                    break;
            }
        }
        return _settings.MagnitudeScaling.DefaultTier;
    }
}
