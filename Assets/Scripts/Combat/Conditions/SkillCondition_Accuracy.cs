using System;
using UnityEngine;

[Serializable]
public class SkillCondition_Accuracy : ISkillCondition
{
    [SerializeField] private ProbabilityCondition<float> accuracy = new(1);

    public bool Check(TargetSelectionArgs args) => accuracy.Check(0);
}
