using UnityEngine;

public class TargetSelectionCondition_MaxTargetCount : ITargetSelectionCondition<TargetSelectionArgs>
{
    [SerializeField] private int max;
    
    public bool Check(TargetSelectionArgs value)
    {
        if (value == null)
            return false;
        
        var battle = value.Battle;
        if (battle == null)
            return false;
        
        var targets = battle.Targets;
        if (targets == null)
            return false;

        return targets.Count <= max;
    }
}