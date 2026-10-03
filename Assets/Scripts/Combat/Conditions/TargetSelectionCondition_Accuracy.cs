using UnityEngine;

public class TargetSelectionCondition_Accuracy : TargetSelectionCondition
{
    [SerializeField] private ProbabilityCondition<float> accuracy = new(1);

    public override bool Check(TargetSelectionArgs args) => accuracy.Check(0);
}
