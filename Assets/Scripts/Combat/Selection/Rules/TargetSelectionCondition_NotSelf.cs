public class TargetSelectionCondition_NotSelf : ITargetSelectionCondition<TargetSelectionArgs>
{
    public bool Check(TargetSelectionArgs value)
    {
        if (value == null)
            return false;
        
        if (value.Invoker == null)
            return false;
        
        return value.Invoker.Equals(value.Target);
    }
}