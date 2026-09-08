using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionTurnCombatantPanel : MonoBehaviour
{
    [SerializeField] private Image combatantIcon;
    [SerializeField] private TextMeshProUGUI actionValueText;

    public void UpdateCombatantPanel(Combatant combatant, float actionValue)
    {
        combatantIcon.sprite = combatant.CharacterData.Portrait;
        actionValueText.text = actionValue.ToString("F0");
    }
}
