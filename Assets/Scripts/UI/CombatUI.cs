using System;
using System.Linq;
using UnityEngine;

public class CombatUI : MonoBehaviour
{
    public event Action<CombatActionType, int> OnActionButton;

    [SerializeField] private PlayerCombatantPanel playerCombatantPanel; // TODO: In its own class
    [SerializeField] private EnemyCombatantPanel enemyCombatantPanel; // TODO: In its own class

    [SerializeField] private TurnOrderPanel turnOrderPanel;
    [SerializeField] private CombatantActionMenu combatantActionsMenu;
    [SerializeField] private CombatResultsPanel combatResultsPanel;

    // TODO: Clean method.
    public void Initialize(Combat combat)
    {
        playerCombatantPanel.Initialize(combat.PlayerCombatants.FirstOrDefault());
        enemyCombatantPanel.Initialize(combat.EnemyCombatants.FirstOrDefault());
    }

    private void OnEnable()
    {
        Combat.OnTurnStarted += OnTurnStarted;
        Combat.OnTurnEnded += OnTurnEnded;
        Combat.OnCombatEnded += OnCombatEnded;

        combatantActionsMenu.OnMenuActionSelected += OnActionButtonSelected;
    }

    private void OnDisable()
    {
        Combat.OnTurnStarted -= OnTurnStarted;
        Combat.OnTurnEnded -= OnTurnEnded;
        Combat.OnCombatEnded -= OnCombatEnded;

        combatantActionsMenu.OnMenuActionSelected -= OnActionButtonSelected;
    }

    public void OnTurnStarted(Combatant combatant)
    {
        ShowActionsMenu(combatant);
    }

    public void OnTurnEnded(Combatant _)
    {
        ShowActionsMenu(null);
    }

    public void OnCombatEnded(bool isVictory)
    {
        combatResultsPanel.gameObject.SetActive(true);
        combatResultsPanel.Initialize(isVictory);
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }

    // TODO: Clean method.
    public void ShowActionsMenu(Combatant combatant)
    {
        bool isPlayerControlled = combatant != null && combatant.IsPlayerControlled;

        combatantActionsMenu.gameObject.SetActive(isPlayerControlled);

        if (isPlayerControlled)
            combatantActionsMenu.SetCombatant(combatant);
    }

    private void OnActionButtonSelected(CombatActionType actionType, int index)
    {
        OnActionButton?.Invoke(actionType, index);
    }
}