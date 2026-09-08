using System.Collections.Generic;
using UnityEngine;

public class CombatUIManager : MonoBehaviour
{
    public static CombatUIManager Instance { get; private set; }

    [SerializeField] private Combat combat; // Temporary. This should be passed in when creating the combat UI manager.

    [SerializeField] private PlayerCombatantPanel playerCombatantPanel;
    [SerializeField] private EnemyCombatantPanel enemyCombatantPanel;
    [SerializeField] private ActionTurnPanel actionTurnPanel;

    private Dictionary<Combatant, ICombatantPanel> combatantPanels = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        Register(combat.PlayerCombatant, playerCombatantPanel);
        Register(combat.EnemyCombatant, enemyCombatantPanel);
    }

    private void OnEnable()
    {
        Combatant.OnDamageTaken += UpdateCharacterResourceBars;
    }

    private void OnDisable()
    {
        Combatant.OnDamageTaken -= UpdateCharacterResourceBars;
    }

    public void Register(Combatant combatant, ICombatantPanel characterResourceBars)
    {
        combatantPanels[combatant] = characterResourceBars;

        characterResourceBars.Initialize(combatant);
    }

    public void UpdateCharacterResourceBars(Combatant combatant, float _)
    {
        if (!combatantPanels.TryGetValue(combatant, out ICombatantPanel combatantPanel))
            return;

        combatantPanel.UpdateResourceBars(combatant);
    }

    public void OnAttackButton()
    {
        combat.Attack();
    }

    public void OnSkillsButton()
    {

    }

    public void OnItemsButton()
    {

    }

    public void OnSkipTurnButton()
    {

    }
}