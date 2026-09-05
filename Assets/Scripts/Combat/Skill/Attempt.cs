using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Attempt
{
    [SerializeField] private ProbabilityCondition<float> accuracy = new(1);
    [SerializeReference, SerializeReferenceDropdown] private CombatCondition[] combatConditions;
    [SerializeReference, SerializeReferenceDropdown] private CombatantCondition[] invokerConditions;
    [SerializeReference, SerializeReferenceDropdown] private CombatantCondition[] targetConditions;
    [SerializeReference, SerializeReferenceDropdown] public List<Effect> effects;

    public void Execute(Combat combat, ITarget invoker, ITarget target)
    {
        foreach (CombatCondition combatCondition in combatConditions)
        {
            if (!combatCondition.Check(combat))
            {
                Debug.Log($"Attempt failed due to {combatCondition.GetType().Name}!");
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
