using System;
using System.Collections.Generic;
using UnityEngine;

public class CombatUI : MonoBehaviour
{
    public event Action<CombatActionType, int> OnActionButton;

    // TODO: Maybe combatant panel should be in its own class for handling?
    [SerializeField] private PlayerCombatantPanel playerCombatantPanelPrefab;
    [SerializeField] private Transform playerCombatantPanelParent;
    [SerializeField] private EnemyCombatantPanel enemyCombatantPanelPrefab;
    [SerializeField] private Transform enemyCombatantPanelParent;


    [SerializeField] private TurnOrderPanel turnOrderPanel;
    [SerializeField] private CombatantActionMenu combatantActionsMenu;
    [SerializeField] private CombatResultsPanel combatResultsPanel;

    private List<PlayerCombatantPanel> playerCombatantPanels = new();
    private List<EnemyCombatantPanel> enemyCombatantPanels = new();

    // TODO: Clean method.
    public void Initialize(Combat combat)
    {
        InitializeCombatantPanels(combat.PlayerCombatants, playerCombatantPanelPrefab, playerCombatantPanelParent, playerCombatantPanels);
        InitializeCombatantPanels(combat.EnemyCombatants, enemyCombatantPanelPrefab, enemyCombatantPanelParent, enemyCombatantPanels);
    }

    private void InitializeCombatantPanels<TPanel>(IReadOnlyList<Combatant> combatants, TPanel panelTemplate, Transform panelParent, List<TPanel> panels) where TPanel : CombatantPanel
    {
        while (panels.Count < combatants.Count)
            panels.Add(Instantiate(panelTemplate, panelParent));

        for (int i = 0; i < panels.Count; i++)
        {
            bool hasCombatant = i < combatants.Count;
            panels[i].gameObject.SetActive(hasCombatant);

            if (hasCombatant)
                panels[i].Initialize(combatants[i]);
        }
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
        combatantActionsMenu.HideSubmenus();

        if (isPlayerControlled)
            combatantActionsMenu.SetCombatant(combatant);
    }

    private void OnActionButtonSelected(CombatActionType actionType, int index)
    {
        OnActionButton?.Invoke(actionType, index);
    }
}