using System.Collections.Generic;
using UnityEngine;

public class TurnOrderPanel : MonoBehaviour
{
    [SerializeField] private List<TurnOrderCombatantPanel> turnOrderCombatantPanels;

    private void OnEnable()
    {
        CombatTurnOrder.OnTurnOrderUpdated += UpdateTurnOrder;
    }

    private void OnDisable()
    {
        CombatTurnOrder.OnTurnOrderUpdated -= UpdateTurnOrder;
    }

    public void UpdateTurnOrder()
    {
        List<(Combatant combatant, float actionValue)> combatantPredictions = CombatTurnOrder.GetTurnOrderPrediction(turnOrderCombatantPanels.Count);

        for (int i = 0; i < turnOrderCombatantPanels.Count; i++)
        {
            if (i < combatantPredictions.Count)
            {
                turnOrderCombatantPanels[i].UpdateCombatantPanel(combatantPredictions[i].combatant, combatantPredictions[i].actionValue);
                turnOrderCombatantPanels[i].gameObject.SetActive(true);
            }
            else
            {
                turnOrderCombatantPanels[i].gameObject.SetActive(false);
            }
        }
    }
}
