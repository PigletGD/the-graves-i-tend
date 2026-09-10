using System.Collections.Generic;
using UnityEngine;

public class CombatUI : MonoBehaviour
{
    [SerializeField] private Combat combat; // Temporary. This should be passed in when creating the combat UI manager.

    [SerializeField] private PlayerCombatantPanel playerCombatantPanel;
    [SerializeField] private EnemyCombatantPanel enemyCombatantPanel;
    [SerializeField] private TurnOrderPanel turnOrderPanel;
    [SerializeField] private GameObject playerActionsPanel;
    [SerializeField] private GameObject playerBasicActionsPanel;
    [SerializeField] private PlayerCombatantActionPanel playerCombatantActionPanel;
    [SerializeField] private CombatResultsPanel combatResultsPanel;

    private Dictionary<Combatant, ICombatantPanel> combatantPanels = new();

    private void Start()
    {
        Register(combat.PlayerCombatant, playerCombatantPanel);
        Register(combat.EnemyCombatant, enemyCombatantPanel);
    }

    private void OnEnable()
    {
        Combatant.OnHPChanged += UpdateCharacterResourceBars;
        Combatant.OnMPChanged += UpdateCharacterResourceBars;
        Combat.OnTurnStarted += OnTurnStarted;
        Combat.OnTurnEnded += OnTurnEnded;
        Combat.OnCombatEnded += OnCombatEnded;

        playerCombatantActionPanel.OnActionSelected += OnActionButton;
    }

    private void OnDisable()
    {
        Combatant.OnHPChanged -= UpdateCharacterResourceBars;
        Combatant.OnMPChanged -= UpdateCharacterResourceBars;
        Combat.OnTurnStarted -= OnTurnStarted;
        Combat.OnTurnEnded -= OnTurnEnded;
        Combat.OnCombatEnded -= OnCombatEnded;

        playerCombatantActionPanel.OnActionSelected -= OnActionButton;
    }

    public void Register(Combatant combatant, ICombatantPanel characterResourceBars)
    {
        combatantPanels[combatant] = characterResourceBars;

        characterResourceBars.Initialize(combatant);
    }

    // Maybe move this into the class EnemyCombatantPanel and PlayerCombatantPanel
    public void UpdateCharacterResourceBars(Combatant combatant, float _)
    {
        if (!combatantPanels.TryGetValue(combatant, out ICombatantPanel combatantPanel))
            return;

        combatantPanel.UpdateResourceBars(combatant);
    }

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
        playerCombatantActionPanel.UpdateActionsPanel(true, combat.ActiveCombatant);
        TogglePlayerBasicActionsPanel(false);
    }

    public void OnItemsButton()
    {
        playerCombatantActionPanel.UpdateActionsPanel(false, combat.ActiveCombatant);
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

    public void ShowPlayerActionsPanel(bool value)
    {
        playerActionsPanel.SetActive(value);
    }

    public void TogglePlayerBasicActionsPanel(bool value)
    {
        playerBasicActionsPanel.SetActive(value);
        playerCombatantActionPanel.gameObject.SetActive(!value);
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
}