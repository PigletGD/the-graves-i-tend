using System;
using System.Collections.Generic;
using UnityEngine;

public class CombatantActionSubmenu : MonoBehaviour
{
    public event Action<int> OnActionButtonSelected;

    [SerializeField] private CombatantActionButton combatantActionButtonPrefab;
    
    private List<CombatantActionButton> combatantActionButtons = new();

    public void UpdateActions(Combatant combatant)
    {
        int actionCount = combatant.CharacterData.Skills.Length;
        InstantiateActionButtons(actionCount);

        for (int i = 0; i < combatantActionButtons.Count; i++)
        {
            CombatantActionButton actionButton = combatantActionButtons[i];

            if (i < actionCount)
            {
                actionButton.UpdateCombatantActionButton(i, combatant.CharacterData.Skills[i].name);
                actionButton.gameObject.SetActive(true);
            }
            else
            {
                actionButton.gameObject.SetActive(false);
            }
        }
    }

    private void InstantiateActionButtons(int actionCount)
    {
        while (combatantActionButtons.Count < actionCount)
        {
            CombatantActionButton combatantActionButton = Instantiate(combatantActionButtonPrefab, transform);

            combatantActionButton.OnClick += index => OnActionButtonSelected?.Invoke(index);
            combatantActionButtons.Add(combatantActionButton);
        }
    }
}