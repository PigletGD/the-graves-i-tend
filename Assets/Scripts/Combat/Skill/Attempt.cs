using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Attempt
{
    [SerializeField] private ProbabilityCondition<float> accuracy = new(1);
    [SerializeReference, SerializeReferenceDropdown] private BattleCondition[] battleConditions;
    [SerializeReference, SerializeReferenceDropdown] private CombatantCondition[] invokerConditions;
    [SerializeReference, SerializeReferenceDropdown] private CombatantCondition[] targetConditions;
    [SerializeReference, SerializeReferenceDropdown] public List<Effect> effects;

    public void Execute(Battle battle, ITarget invoker, ITarget target)
    {
        foreach (BattleCondition battleCondition in battleConditions)
        {
            if (!battleCondition.Check(battle))
            {
                Debug.Log($"Attempt failed due to {battleCondition.GetType().Name}!");
                return;
            }
        }

        if (invoker is Combatant invokerCombatant)
        {
            foreach (CombatantCondition invokerCondition in invokerConditions)
            {
                if (!invokerCondition.Check(invokerCombatant))
                {
                    Debug.Log($"Attempt failed due to {invokerCondition.GetType().Name}!");
                    return;
                }
            }
        }

        if (!accuracy.Check(0))
        {
            List<string> effectNames = new();
            foreach (Effect effect in effects)
                effectNames.Add(effect.GetType().Name);
            Debug.Log($"Attempt missed! Effects: {string.Join(", ", effectNames)}.");

            return;
        }

        if (target is Combatant targetCombatant)
        {
            foreach (CombatantCondition targetCondition in targetConditions)
            {
                if (!targetCondition.Check(targetCombatant))
                {
                    Debug.Log($"Attempt failed due to {targetCondition.GetType().Name}!");
                    return;
                }
            }
        }

        foreach (Effect effect in effects)
            effect.Apply(target);
    }
}
