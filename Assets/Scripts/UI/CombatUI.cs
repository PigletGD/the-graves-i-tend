using UnityEngine;

public class CombatUI : MonoBehaviour
{
    [SerializeField] private Combat combat; // Temporary. This should be passed in when creating the combat UI manager.

    [SerializeField] private PlayerCombatantPanel playerCombatantPanel;
    [SerializeField] private EnemyCombatantPanel enemyCombatantPanel;
    [SerializeField] private TurnOrderPanel turnOrderPanel;
    [SerializeField] private GameObject playerActionsParentPanel;
    [SerializeField] private GameObject playerBasicActionSelectionPanel; // Attack/Skills/Items/Skip Turn
    [SerializeField] private PlayerCombatantActionsPanel playerCombatantActionsPanel; // All Skills/All Items
    [SerializeField] private CombatResultsPanel combatResultsPanel;

    private void Start()
    {
        playerCombatantPanel.Initialize(combat.PlayerCombatant);
        enemyCombatantPanel.Initialize(combat.EnemyCombatant);
    }

    private void OnEnable()
    {
        Combat.OnTurnStarted += OnTurnStarted;
        Combat.OnTurnEnded += OnTurnEnded;
        Combat.OnCombatEnded += OnCombatEnded;

        playerCombatantActionsPanel.OnActionSelected += OnActionButton;
    }

    private void OnDisable()
    {
        Combat.OnTurnStarted -= OnTurnStarted;
        Combat.OnTurnEnded -= OnTurnEnded;
        Combat.OnCombatEnded -= OnCombatEnded;

        playerCombatantActionsPanel.OnActionSelected -= OnActionButton;
    }

    public void OnTurnStarted(Combatant combatant)
    {
        ShowPlayerActionsPanel(combatant.IsPlayerControlled);

        if (combatant.IsPlayerControlled)
            TogglePlayerBasicActionsPanel(true);
    }

    public void OnTurnEnded(Combatant _)
    {
        ShowPlayerActionsPanel(false);
    }

    public void OnCombatEnded(bool isVictory)
    {
        combatResultsPanel.gameObject.SetActive(true);
        combatResultsPanel.Initialize(isVictory);
    }


#region Button Events
    public void OnAttackButton()
    {
        combat.TryPlayerAct(Combatant.BasicAttackIndex);
    }

    private void OnActionButton(int index)
    {
        combat.TryPlayerAct(index);
    }

    public void OnSkillsButton()
    {
        playerCombatantActionsPanel.UpdateActionsPanel(true, combat.ActiveCombatant);
        TogglePlayerBasicActionsPanel(false);
    }

    public void OnItemsButton()
    {
        playerCombatantActionsPanel.UpdateActionsPanel(false, combat.ActiveCombatant);
        TogglePlayerBasicActionsPanel(false);
    }

    public void OnSkipTurnButton()
    {
        combat.TryPlayerAct(Combatant.SkipTurnIndex);
    }

    public void OnActionListBackButton()
    {
        TogglePlayerBasicActionsPanel(true);
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
#endregion

    public void ShowPlayerActionsPanel(bool value)
    {
        playerActionsParentPanel.SetActive(value);
    }

    public void TogglePlayerBasicActionsPanel(bool value)
    {
        playerBasicActionSelectionPanel.SetActive(value);
        playerCombatantActionsPanel.gameObject.SetActive(!value);
    }
}