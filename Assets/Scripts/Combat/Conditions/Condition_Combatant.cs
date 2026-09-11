using System;

[Serializable]
public abstract class Condition_Combatant : ICondition<Combatant>
{
    public abstract bool Check(Combatant value);
}