using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Attempt : IAttempt
{
    [SerializeField] private ProbabilityCondition<float> accuracy;
    [SerializeReference, SerializeReferenceDropdown] private BattleCondition[] battleConditions;
    [SerializeReference, SerializeReferenceDropdown] private CombatantCondition[] invokerConditions;
    [SerializeReference, SerializeReferenceDropdown] private CombatantCondition[] targetConditions;
    [SerializeReference, SerializeReferenceDropdown] public List<Effect> effects;

    public void Execute(Battle battle, ITarget invoker, ITarget target)
    {
        if (invoker is Combatant sourceCombatant)
        {   
            foreach (BattleCondition battleCondition in battleConditions)
            {
                if (!battleCondition.Check(battle))
                {
                    Debug.Log($"Attempt failed due to a battle condition!");
                    return;
                }
            }

            foreach (CombatantCondition invokerCondition in invokerConditions)
            {
                if (!invokerCondition.Check(sourceCombatant))
                {
                    Debug.Log($"Attempt failed due to a invoke condition!");
                    return;
                }
            }
        }

        foreach (Effect effect in effects)
            effect.Apply(target);
    }
}
