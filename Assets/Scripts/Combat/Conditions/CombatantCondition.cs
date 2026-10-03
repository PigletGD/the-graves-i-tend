public abstract class CombatantCondition : ICondition<Combatant>
{
    public abstract bool Check(Combatant combatant);
}
