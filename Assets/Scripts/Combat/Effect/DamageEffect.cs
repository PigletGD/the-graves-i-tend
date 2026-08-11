using System;
using UnityEngine;

[Serializable]
public class DamageEffect : Effect
{
    [SerializeField] private float damage;
    [SerializeField] private CombatantEffectTypeCondition amplifyCondition;

    public override void Apply(ITarget target)
    {
        if (target is not Combatant combatant)
        {
            Log("failed because Target is not a combatant");
            return;
        }

        if (amplifyCondition.Check(combatant))
        {
            Log($"was amplified by {amplifyCondition.EffectType} and dealt {damage * 2f} damage");
            combatant.UpdateHP(-damage * 2f);
            combatant.EffectController.RemoveEffect(amplifyCondition.EffectType);
        }
        else
        {
            Log($"dealt {damage} damage");
            combatant.UpdateHP(-damage);
        }
    }
}