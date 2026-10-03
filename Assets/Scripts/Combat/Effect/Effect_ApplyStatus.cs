using System;
using UnityEngine;

[Serializable]
public class Effect_ApplyStatus : Effect
{
    [SerializeField] private StatusEffectSO statusEffectSO;
    [SerializeField] private int stacksOnApply = 1; // Only relevant to stacking status effects. Placed here because having a separate class is redundant.
    [SerializeField] private CombatantCondition[] conditions;

    public override void Apply(ITarget target)
    {
        if (target is not Combatant combatant)
        {
            Log("failed because Target is not a combatant");
            return;
        }

        foreach (CombatantCondition conditions in conditions)
        {
            if (!conditions.Check(combatant))
                return;
        }

        combatant.StatusEffects.AddEffect(statusEffectSO.CreateInstance(), stacksOnApply);
    }
}
