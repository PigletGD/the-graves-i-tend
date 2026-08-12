using System;
using UnityEngine;

[Serializable]
public class FrozenEffect : Effect
{
    [SerializeField] private FrozenStatusEffectSO frozenStatusEffectSO;
    [SerializeField] private ProbabilityCondition<float> applyChance;
    [SerializeField] private CombatantCondition_StatusEffect[] effectConditions;

    public override void Apply(ITarget target)
    {
        if (target is not Combatant combatant)
        {
            Log("failed because Target is not a combatant");
            return;
        }

        if (!applyChance.Check(0))
        {
            Log("missed");
            return;
        }

        foreach (CombatantCondition_StatusEffect effectCondition in effectConditions)
        {
            if (!effectCondition.Check(combatant))
            {
                Log($"failed because of {effectCondition.EffectType}");
                return;
            }
        }

        combatant.EffectController.AddEffect(frozenStatusEffectSO.CreateInstance());
    }
}
