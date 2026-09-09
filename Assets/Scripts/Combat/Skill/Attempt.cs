using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Attempt
{
    public static event Action<ITarget, ITarget> OnAttemptMissed;

    [SerializeField] private ProbabilityCondition<float> accuracy = new(1);
    [SerializeReference, SerializeReferenceDropdown] private CombatCondition[] combatConditions;
    [SerializeReference, SerializeReferenceDropdown] private CombatantCondition[] invokerConditions;
    [SerializeReference, SerializeReferenceDropdown] private CombatantCondition[] targetConditions;
    [SerializeReference, SerializeReferenceDropdown] public List<Effect> invokerEffects;
    [SerializeReference, SerializeReferenceDropdown] public List<Effect> targetEffects;

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

        // Check if we actually hit the Attempt first before checking if the Effect works on the Target.
        if (!accuracy.Check(0))
        {
            List<string> effectNames = new();

            foreach (Effect effect in targetEffects)
                effectNames.Add(effect.GetType().Name);

            foreach (Effect effect in invokerEffects)
                effectNames.Add(effect.GetType().Name);

            Debug.Log($"Attempt missed! Effects: {string.Join(", ", effectNames)}.");
            OnAttemptMissed?.Invoke(invoker, target);
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

        foreach (Effect effect in invokerEffects)
            effect.Apply(invoker);

        foreach (Effect effect in targetEffects)
            effect.Apply(target);
    }
}
