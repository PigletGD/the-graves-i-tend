using System;
using System.Collections.Generic;
using System.Linq;

// https://hsr.keqingmains.com/misc/speed-guide/#How_Does_Speed_Really_Work
public static class CombatTurnManager
{
    public static Action OnTurnOrderUpdated;
    
    private static Dictionary<Combatant, float> actionValues = new();

    private static IOrderedEnumerable<KeyValuePair<Combatant, float>> OrderedActionValues
        => actionValues
            .OrderBy(kv => kv.Value)
            .ThenByDescending(kv => kv.Key.IsPlayerControlled);

    public static void InitializeTurnOrder(List<Combatant> combatants)
    {
        actionValues.Clear();
        foreach (Combatant combatant in combatants)
            actionValues[combatant] = combatant.Stats.ActionValue;

        OnTurnOrderUpdated?.Invoke();
    }

    public static Combatant GetNextCombatant()
    {
        KeyValuePair<Combatant, float> nextCombatantEntry = OrderedActionValues.FirstOrDefault();

        foreach (var combatant in actionValues.Keys.ToList())
            actionValues[combatant] -= nextCombatantEntry.Value;

        OnTurnOrderUpdated?.Invoke();
        return nextCombatantEntry.Key;
    }
    
    /// <summary>
    /// Call after finishing a Combatant's turn to re-queue them back in actionValues.
    /// </summary>
    public static void ResetCombatantActionValue(Combatant combatant)
    {
        if (actionValues.ContainsKey(combatant))
        {
            actionValues[combatant] = combatant.Stats.ActionValue;
            OnTurnOrderUpdated?.Invoke();
        }
    }

    public static List<(Combatant combatant, float actionValue)> GetTurnOrderPrediction(int turnCount)
    {
        List<(Combatant combatant, float actionValue)> prediction = new();
        Dictionary<Combatant, float> predictedActionValues = new(actionValues);

        for (int i = 0; i < turnCount; i++)
        {
            KeyValuePair<Combatant, float> nextCombatant = predictedActionValues
                .OrderBy(kv => kv.Value)
                .ThenByDescending(kv => kv.Key.IsPlayerControlled)
                .First();

            prediction.Add((nextCombatant.Key, nextCombatant.Value));
            predictedActionValues[nextCombatant.Key] += nextCombatant.Key.Stats.ActionValue;
        }

        return prediction;
    }

}