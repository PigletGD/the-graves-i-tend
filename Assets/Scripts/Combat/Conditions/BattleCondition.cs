using System;

[Serializable]
public abstract class BattleCondition : ICondition<Battle>
{
    public abstract bool Check(Battle value);
}