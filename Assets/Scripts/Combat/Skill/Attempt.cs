using System;
using UnityEngine;

[Serializable]
public class Attempt : IAttempt
{
    // Animation animation;
    [SerializeField] private ProbabilityCondition<float> accuracy;
    // On Hit
    // On Miss
    // After

    public void Execute()
    {
        Debug.Log($"Tried hitting with a {accuracy.Probability * 100:F0}% chance: {accuracy.Check(0)}");
    }
}
