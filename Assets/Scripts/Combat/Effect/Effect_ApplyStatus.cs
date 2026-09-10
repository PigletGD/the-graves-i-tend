using System;
using UnityEngine;

[Serializable]
public class Effect_ApplyStatus : Effect
{
    [SerializeField] private StatusEffectSO statusEffectSO;
    [SerializeField] private int stacksOnApply = 1; // Only relevant to stacking status effects. Placed here because having a separate class is redundant.
    [SerializeField] private ProbabilityCondition<float> applyChance = new(1);
    [SerializeField] private CombatantCondition_StatusEffect[] statusEffectCondition;

    public override void Apply(ITarget target)
    {
        if (target is not Combatant combatant)
        {
            Log("failed because Target is not a combatant");
            return;
        }

        if (!applyChance.Check(0))
        {
            Log($"{statusEffectSO.StatusEffectType} failed apply chance");
            return;
        }

        foreach (CombatantCondition_StatusEffect effectCondition in statusEffectCondition)
        {
            if (!effectCondition.Check(combatant))
            {
                Log($"failed because of {effectCondition.EffectType}");
                return;
            }
        }

        combatant.StatusEffects.AddEffect(statusEffectSO.CreateInstance(), stacksOnApply);
    }
}
