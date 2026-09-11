using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatantActionsPanel : MonoBehaviour
{
    public event Action<int> OnActionSelected;

    [SerializeField] private PlayerCombatantActionButton combatantActionButtonPrefab;
    
    private List<PlayerCombatantActionButton> playerCombatantActionButtons = new();

    public void UpdateActionsPanel(bool isSkills, Combatant combatant)
    {
        int actionCount = isSkills ? combatant.CharacterData.Skills.Length : 0;
        InstantiateActionButtons(actionCount);

        for (int i = 0; i < playerCombatantActionButtons.Count; i++)
        {
            PlayerCombatantActionButton actionButton = playerCombatantActionButtons[i];

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
        while (playerCombatantActionButtons.Count < actionCount)
        {
            PlayerCombatantActionButton combatantActionButton = Instantiate(combatantActionButtonPrefab, transform);

            combatantActionButton.OnClick += index => OnActionSelected?.Invoke(index);
            playerCombatantActionButtons.Add(combatantActionButton);
        }
    }
}