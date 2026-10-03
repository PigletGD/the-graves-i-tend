using System;

[Serializable]
public abstract class TargetSelectionCondition : ICondition<TargetSelectionArgs>
{
    public abstract bool Check(TargetSelectionArgs args);
}
