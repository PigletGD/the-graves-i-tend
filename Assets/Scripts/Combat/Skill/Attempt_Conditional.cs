using System;
using UnityEngine;

[Serializable]
public class Attempt_Conditional : Attempt
{
    [SerializeReference, SerializeReferenceDropdown] public TargetSelectionCondition[] conditions;

    public override void Execute(TargetSelectionArgs args)
    {
        foreach (TargetSelectionCondition condition in conditions)
        {
            if (!condition.Check(args))
            {
                OnAttemptFailed?.Invoke(args, condition);
                return;
            }
        }

        foreach (ITarget target in args.Targets)
        {
            foreach (Effect effect in effects)
            {
                effect.Apply(target);
            }
        }
    }
}
