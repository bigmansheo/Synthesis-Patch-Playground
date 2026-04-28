using BossGearSynthesis.Settings;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis;

namespace BossGearSynthesis.Analysis;

public class StrengthAnalyzer
{
    private readonly Settings.Settings _settings;
    private readonly TierClassifier _tier;
    private readonly HashSet<Skill> _combatSkills;

    public StrengthAnalyzer(Settings.Settings settings, TierClassifier tier)
    {
        _settings = settings;
        _tier = tier;
        _combatSkills = settings.StrengthAnalysis.CombatSkills.ToHashSet();
    }

    public BossStrengthProfile? Analyze(
        INpcGetter npc,
        bool isNamedUnique,
        IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
    {
        var skills = ResolveSkills(npc);
        if (skills.Count == 0) return null;

        var combatRanked = skills
            .Where(kv => _combatSkills.Contains(kv.Key))
            .OrderByDescending(kv => kv.Value)
            .ThenBy(kv => kv.Key)
            .ToList();
        if (combatRanked.Count == 0) return null;

        var topCombat = combatRanked[0].Key;

        var secondSkill = skills
            .Where(kv => kv.Key != topCombat)
            .OrderByDescending(kv => kv.Value)
            .ThenBy(kv => kv.Key)
            .Select(kv => (Skill?)kv.Key)
            .FirstOrDefault();

        var (topAttribute, attributeValue) = ResolveTopAttribute(npc);
        var level = ResolveLevel(npc);
        var tier = _tier.Classify(npc, isNamedUnique, level);
        var attackStyle = AttackStyleResolver.Resolve(npc, state, topCombat);
        var isNamed = (npc.Configuration.Flags & NpcConfiguration.Flag.Unique) != 0
                      && !string.IsNullOrWhiteSpace(npc.Name?.String);

        var useAttribute = ChooseEffect2Source(
            secondSkill is { } s ? skills[s] : 0,
            attributeValue,
            level);

        return new BossStrengthProfile(
            TopCombatSkill: topCombat,
            SecondSkill: secondSkill,
            TopAttribute: topAttribute,
            DerivedStyle: attackStyle,
            EffectiveLevel: level,
            Tier: tier,
            IsNamedUnique: isNamedUnique,
            IsNamed: isNamed)
        {
            UseAttributeForEffect2 = useAttribute,
        };
    }

    private static Dictionary<Skill, int> ResolveSkills(INpcGetter npc)
    {
        var ps = npc.PlayerSkills;
        if (ps == null) return new();
        var values = ps.SkillValues ?? new Dictionary<Skill, byte>();
        var offsets = ps.SkillOffsets ?? new Dictionary<Skill, byte>();
        var result = new Dictionary<Skill, int>();
        foreach (var kv in values)
        {
            offsets.TryGetValue(kv.Key, out var off);
            result[kv.Key] = kv.Value + off;
        }
        return result;
    }

    private static (BasicAttribute? top, int value) ResolveTopAttribute(INpcGetter npc)
    {
        var ps = npc.PlayerSkills;
        var cfg = npc.Configuration;
        if (ps == null) return (null, 0);
        var hp = ps.Health + cfg.HealthOffset;
        var mp = ps.Magicka + cfg.MagickaOffset;
        var sp = ps.Stamina + cfg.StaminaOffset;
        var max = Math.Max(hp, Math.Max(mp, sp));
        if (max <= 0) return (null, 0);
        if (hp >= mp && hp >= sp) return (BasicAttribute.Health, hp);
        if (mp >= sp) return (BasicAttribute.Magicka, mp);
        return (BasicAttribute.Stamina, sp);
    }

    private static int ResolveLevel(INpcGetter npc)
    {
        // npc.Configuration.Level is INpcLevelGetter; concrete NpcLevel has Level (ushort).
        // PCLevelMult uses CalcMinLevel and CalcMaxLevel; just use CalcMinLevel as a stable approximation.
        if (npc.Configuration.Level is NpcLevel fixedLevel)
            return fixedLevel.Level;
        var min = (int)npc.Configuration.CalcMinLevel;
        return min > 0 ? min : 1;
    }

    private bool ChooseEffect2Source(int secondSkillValue, int attributeValue, int level)
    {
        return _settings.StrengthAnalysis.SecondEffectMode switch
        {
            SecondEffectMode.AlwaysSkill => false,
            SecondEffectMode.AlwaysAttribute => true,
            SecondEffectMode.PreferSkill => secondSkillValue > 0 ? false : true,
            SecondEffectMode.PreferAttribute => attributeValue > 0 ? true : false,
            _ => AutoChoose(secondSkillValue, attributeValue, level),
        };
    }

    private bool AutoChoose(int secondSkillValue, int attributeValue, int level)
    {
        var sa = _settings.StrengthAnalysis;
        var skillBaseline = sa.SkillBaseline + level * sa.SkillBaselinePerLevel;
        var attrBaseline = sa.AttributeBaseline + level * sa.AttributeBaselinePerLevel;
        var skillScore = secondSkillValue - skillBaseline;
        var attrScore = attributeValue - attrBaseline;
        return attrScore > skillScore;
    }
}
