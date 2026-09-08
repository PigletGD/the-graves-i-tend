using System.Collections.Generic;
using UnityEngine;

public class ActionTurnPanel : MonoBehaviour
{
    [SerializeField] private List<ActionTurnCombatantPanel> actionTurnCombatantPanels;

    private void OnEnable()
    {
        CombatTurnManager.OnTurnOrderUpdated += UpdateTurnOrder;
    }

    private void OnDisable()
    {
        CombatTurnManager.OnTurnOrderUpdated -= UpdateTurnOrder;
    }

    public void UpdateTurnOrder()
    {
        List<(Combatant combatant, float actionValue)> combatantPredictions = CombatTurnManager.GetTurnOrderPrediction(actionTurnCombatantPanels.Count);

        for (int i = 0; i < actionTurnCombatantPanels.Count; i++)
        {
            if (i < combatantPredictions.Count)
            {
                actionTurnCombatantPanels[i].UpdateCombatantPanel(combatantPredictions[i].combatant, combatantPredictions[i].actionValue);
                actionTurnCombatantPanels[i].gameObject.SetActive(true);
            }
            else
            {
                actionTurnCombatantPanels[i].gameObject.SetActive(false);
            }
        }
    }
}
