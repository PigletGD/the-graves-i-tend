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
        damageEffect.Apply(battle);
    }
}
