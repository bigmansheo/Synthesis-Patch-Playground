using BossGearSynthesis.Defaults;
using BossGearSynthesis.Settings;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;

namespace BossGearSynthesis.Enchantments;

/// <summary>Resolved skill/attribute → MGEF lookups, merging user settings over built-in defaults.</summary>
public class EffectMap
{
    private readonly Dictionary<Skill, (FormKey mgef, float mult)> _skill;
    private readonly Dictionary<BasicAttribute, (FormKey mgef, float mult)> _attribute;

    public EffectMap(EffectMapSettings settings)
    {
        _skill = EffectMapDefaults.SkillMgef.ToDictionary(kv => kv.Key, kv => (kv.Value, 1.0f));
        foreach (var entry in settings.SkillEffects)
        {
            if (entry.MagicEffect.IsNull) continue;
            _skill[entry.Skill] = (entry.MagicEffect.FormKey, entry.Multiplier);
        }

        _attribute = EffectMapDefaults.AttributeMgef.ToDictionary(kv => kv.Key, kv => (kv.Value, 1.0f));
        foreach (var entry in settings.AttributeEffects)
        {
            if (entry.MagicEffect.IsNull) continue;
            _attribute[entry.Attribute] = (entry.MagicEffect.FormKey, entry.Multiplier);
        }
    }

    public (FormKey Mgef, float Multiplier)? ForSkill(Skill skill) =>
        _skill.TryGetValue(skill, out var v) ? v : null;

    public (FormKey Mgef, float Multiplier)? ForAttribute(BasicAttribute attr) =>
        _attribute.TryGetValue(attr, out var v) ? v : null;
}
