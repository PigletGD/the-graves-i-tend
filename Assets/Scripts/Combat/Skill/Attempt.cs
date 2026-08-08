using System;
using UnityEngine;

[Serializable]
public class Attempt : IAttempt
{
    [SerializeField] private ProbabilityCondition<float> accuracy;
    [SerializeField] private DamageEffect damageEffect;
    // [SerializeField] private Effect[] statusEffects;

    public void Execute(Battle battle)
    {
        // TO DO: Don't use getters from battle. 
        // The Effect class should already know who/what the target is (if it should be self/enemy/multiple targets).
        // From there it should resolve what to target in the battle param.
        damageEffect.target = battle.Defender; 
        damageEffect.Apply(battle);
    }
}
