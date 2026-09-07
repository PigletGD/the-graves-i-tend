using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TODO: This should be a panel that shows the turn order of the combatants.
public class ActionTurnPanel : MonoBehaviour
{
    [SerializeField] private List<Image> combatantPortraits;

    public void Initialize(List<Combatant> combatants)
    {
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
