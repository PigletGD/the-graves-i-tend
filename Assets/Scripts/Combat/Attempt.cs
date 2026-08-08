using System;
using UnityEngine;

[Serializable]
public class Attempt
{
    // Animation animation;
    [SerializeField] private ProbabilityCondition<float> accuracy;
    // On Hit
    // On Miss
    // After

    public void Execute()
    {
        throw new NotImplementedException();
    }
}
