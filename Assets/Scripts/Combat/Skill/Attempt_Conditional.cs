using System;
using UnityEngine;

[Serializable]
public class Attempt_Conditional : Attempt
{
    [SerializeReference, SerializeReferenceDropdown] public TargetSelectionCondition[] conditions;

    protected override void ExecuteAttempt(TargetSelectionArgs args, ITarget[] targets)
    {
        foreach (TargetSelectionCondition condition in conditions)
        {
            if (!condition.Check(args))
            {
                OnAttemptFailed?.Invoke(args, condition);
                return;
            }
        }

        foreach (ITarget target in targets)
        {
            foreach (Effect effect in effects)
            {
                effect.Apply(target);
            }
        }
    }
}
