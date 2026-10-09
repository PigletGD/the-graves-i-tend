using UnityEngine;

public class TargetSelectionCondition_MaxTargetCount : ITargetSelectionCondition<TargetSelectionArgs>
{
    [SerializeField] private int max;
    
    public bool Check(TargetSelectionArgs value)
    {
        if (value == null)
            return false;
        
        var combat = value.Combat;
        if (combat == null)
            return false;
        
        if (value.Targets == null)
            return false;

        return value.Targets.Length <= max;
    }
}