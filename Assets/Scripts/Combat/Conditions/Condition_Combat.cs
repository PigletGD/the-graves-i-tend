using System;

[Serializable]
public abstract class Condition_Combat : ICondition<Combat>
{
    public abstract bool Check(Combat value);
}