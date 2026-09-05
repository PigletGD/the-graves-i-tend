using System;

[Serializable]
public abstract class CombatCondition : ICondition<Combat>
{
    public abstract bool Check(Combat value);
}