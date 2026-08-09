using System;
using UnityEngine;

[Serializable]
public class Attempt : IAttempt
{
    [SerializeField] private ProbabilityCondition<float> accuracy;
    [SerializeField] private Effect[] effects;

    public void Execute(Battle battle)
    {
        foreach (Effect effect in effects)
            effect.Apply(battle);
    }
}
