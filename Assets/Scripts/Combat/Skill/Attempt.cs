using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Attempt : IAttempt
{
    [SerializeField] private ProbabilityCondition<float> accuracy;
    [SerializeReference, SerializeReferenceDropdown] private CombatantCondition[] userConditions;
    [SerializeReference, SerializeReferenceDropdown] public List<Effect> effects;

    // TODO: We'll need to figure out at what level of the skill the target gets chosen.
    // I believe that it should probably maybe at the skill level (or maybe the attempt level),
    // depending on how we sequence the skills (like from an animation). Could also be in attempt.
    public void Execute(Battle battle, ITarget source, ITarget[] targets)
    {
        if (source is Combatant sourceCombatant)
        {
            foreach (CombatantCondition damageCondition in userConditions)
            {
                if (!damageCondition.Check(sourceCombatant))
                {
                    Debug.Log($"Attempt failed!");
                    return;
                }
            }
        }

        // We can probably leave it like this for now. Still needs to resolve good targets and bad targets in any case the Skill targets both ally/enemy.
        foreach (ITarget target in targets)
            foreach (Effect effect in effects)
                effect.Apply(target);
    }
}
