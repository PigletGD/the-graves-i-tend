using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionTurnPanel : MonoBehaviour
{
    [SerializeField] private List<Image> combatantPortraits;

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
        List<Combatant> combatants = CombatTurnManager.GetTurnOrder();
        combatants.Reverse();

        for (int i = 0; i < combatantPortraits.Count; i++)
        {
            if (i < combatants.Count)
            {
                combatantPortraits[i].sprite = combatants[i].CharacterData.Portrait;
                combatantPortraits[i].gameObject.SetActive(true);
            }
            else
            {
                combatantPortraits[i].gameObject.SetActive(false);
            }
        }
    }
}
