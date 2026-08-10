using UnityEngine;

public class TargetSelectionCondition_HasTags : ITargetSelectionCondition<TargetSelectionArgs>
{
    [SerializeField] private string[] tags;
    
    public bool Check(TargetSelectionArgs value)
    {
        if (value == null)
            return false;

        var target = value.Target;
        if (target == null)
            return false;

        return true; // Just a test condition for now
    }
}