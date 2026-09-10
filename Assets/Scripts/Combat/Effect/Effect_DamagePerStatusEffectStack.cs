using System;
using UnityEngine;

[Serializable]
public class Effect_DamagePerStatusEffectStack : Effect
{
    [SerializeField] private StatusEffectType statusEffectType;
    [SerializeField] private float damagePerStack = 1f;

    public override void Apply(ITarget target)
    {
        if (target is not Combatant combatant)
        {
            Log("failed because Target is not a combatant");
            return;
        }

        int damage = (int)(damagePerStack * combatant.StatusEffects.GetStackCount(statusEffectType));
        combatant.TakeDamage(damage);
    }
}
