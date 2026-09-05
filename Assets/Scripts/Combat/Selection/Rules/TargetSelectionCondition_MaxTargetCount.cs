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
        
        var targets = combat.Targets;
        if (targets == null)
            return false;

        return targets.Count <= max;
    }
}