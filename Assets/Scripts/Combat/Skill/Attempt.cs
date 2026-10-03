using System;
using UnityEngine;

[Serializable]
public abstract class Attempt
{
    public static Action<TargetSelectionArgs, TargetSelectionCondition> OnAttemptFailed;

    [SerializeReference, SerializeReferenceDropdown] public Effect[] effects;

    public abstract void Execute(TargetSelectionArgs args);
}
