using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis;

namespace BossGearSynthesis.Analysis;

public static class AttackStyleResolver
{
    public static AttackStyle Resolve(INpcGetter npc, IPatcherState<ISkyrimMod, ISkyrimModGetter> state, Skill topCombatSkill)
    {
        // First derive directly from skill — most accurate signal we have.
        switch (topCombatSkill)
        {
            case Skill.OneHanded: return AttackStyle.Melee1H;
            case Skill.TwoHanded: return AttackStyle.Melee2H;
            case Skill.Archery: return AttackStyle.Ranged;
            case Skill.Alteration: return AttackStyle.MagicAlteration;
            case Skill.Conjuration: return AttackStyle.MagicConjuration;
            case Skill.Destruction: return AttackStyle.MagicDestruction;
            case Skill.Illusion: return AttackStyle.MagicIllusion;
            case Skill.Restoration: return AttackStyle.MagicRestoration;
        }

        // Fallback: scan inventory weapons.
        if (npc.Items is { } items)
        {
            foreach (var entry in items)
            {
                if (state.LinkCache.TryResolve<IWeaponGetter>(entry.Item.Item.FormKey, out var weap) &&
                    weap.Data is { } data)
                {
                    return data.AnimationType switch
                    {
                        WeaponAnimationType.Bow => AttackStyle.Ranged,
                        WeaponAnimationType.Crossbow => AttackStyle.Ranged,
                        WeaponAnimationType.TwoHandSword or WeaponAnimationType.TwoHandAxe
                            => AttackStyle.Melee2H,
                        _ => AttackStyle.Melee1H,
                    };
                }
            }
        }
        return AttackStyle.Unknown;
    }
}
