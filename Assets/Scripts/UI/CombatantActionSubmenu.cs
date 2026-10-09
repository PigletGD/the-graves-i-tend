using System;
using System.Collections.Generic;
using UnityEngine;

public class CombatantActionSubmenu : MonoBehaviour
{
    public event Action<int> OnActionButtonSelected;

    [SerializeField] private CombatantActionButton combatantActionButtonPrefab;

    private List<CombatantActionButton> combatantActionButtons = new();

    public void UpdateActions(Combatant combatant, CombatActionType actionType)
    {
        Skill[] actions = actionType switch
        {
            CombatActionType.Skill => combatant.CharacterData.Skills,
            CombatActionType.Item => combatant.CharacterData.Items,
            _ => throw new ArgumentOutOfRangeException(nameof(actionType), actionType, "Submenus only support skills and items.")
        };

        int actionCount = actions.Length;
        InstantiateActionButtons(actionCount);

        for (int i = 0; i < combatantActionButtons.Count; i++)
        {
            CombatantActionButton actionButton = combatantActionButtons[i];

            if (i < actionCount)
            {
                actionButton.UpdateCombatantActionButton(i, actions[i].name);
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