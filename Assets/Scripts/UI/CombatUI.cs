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
        Combat.OnTurnUpdated += OnTurnUpdated;

        playerCombatantActionPanel.OnActionSelected += OnActionButton;
    }

    private void OnDisable()
    {
        Combatant.OnHPChanged -= UpdateCharacterResourceBars;
        Combatant.OnMPChanged -= UpdateCharacterResourceBars;
        Combat.OnTurnUpdated += OnTurnUpdated;

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
        combat.TryPlayerSkill(-1);
    }

    private void OnActionButton(int index)
    {
        combat.TryPlayerSkill(index);
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
        combat.SkipTurn();
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

    public void OnTurnUpdated(Combatant combatant)
    {
        ShowPlayerActionsPanel(combatant.IsPlayerControlled);
    }
}