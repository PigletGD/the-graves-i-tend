using System;
using System.Collections.Generic;
using System.Linq;

// https://hsr.keqingmains.com/misc/speed-guide/#How_Does_Speed_Really_Work
public static class CombatTurnManager
{
    public static Action OnTurnOrderUpdated;
    
    private static Dictionary<Combatant, float> actionValues = new();

    public static void InitializeTurnOrder(List<Combatant> combatants)
    {
        SortTurns(combatants);
    }

    private static void SortTurns(List<Combatant> combatants)
    {
        actionValues.Clear();
        foreach (Combatant combatant in combatants)
            actionValues[combatant] = combatant.Stats.ActionValue;

        OnTurnOrderUpdated?.Invoke();
    }

    public static Combatant GetNextCombatant()
    {
        KeyValuePair<Combatant, float> nextCombatantEntry = actionValues.OrderBy(kv => kv.Value).FirstOrDefault();
        if (nextCombatantEntry.Key != null)
        {
            foreach (var combatant in actionValues.Keys.ToList())
            {
                if (!combatant.Equals(nextCombatantEntry.Key))
                    actionValues[combatant] -= nextCombatantEntry.Value;
            }

            actionValues[nextCombatantEntry.Key] = nextCombatantEntry.Key.Stats.ActionValue;
            OnTurnOrderUpdated?.Invoke();
        }

        return nextCombatantEntry.Key;
    }
    
    public static List<Combatant> GetTurnOrder()
    {
        return actionValues.OrderBy(kv => kv.Value).Select(kv => kv.Key).ToList();
    }
}