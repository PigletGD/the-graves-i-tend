using System;
using UnityEngine;

public class CombatantActionMenu : MonoBehaviour
{
    public event Action<CombatActionType, int> OnMenuActionSelected;

    [SerializeField] private CombatantActionSubmenu combatActionSubmenu;

    private Combatant cachedCombatant;
    private CombatActionType combatActionType;

    private void Awake()
    {
        combatActionSubmenu.OnActionButtonSelected += OnSubmenuActionSelected;
    }

    private void OnDestroy()
    {
        combatActionSubmenu.OnActionButtonSelected -= OnSubmenuActionSelected;
    }

    private void OnEnable()
    {
        combatActionSubmenu.gameObject.SetActive(false);
    }

    public void SetCombatant(Combatant combatant)
    {
        cachedCombatant = combatant;
    }

    public void OnAttackButton()
    {
        combatActionSubmenu.gameObject.SetActive(false);
        OnMenuActionSelected?.Invoke(CombatActionType.Attack, -1);
    }

    public void OnSkipButton()
    {
        combatActionSubmenu.gameObject.SetActive(false);
        OnMenuActionSelected?.Invoke(CombatActionType.Skip, -1);
    }

    public void OnSkillButton()
    {
        OpenSubmenu(CombatActionType.Skill);
    }

    public void OnItemButton()
    {
        OpenSubmenu(CombatActionType.Item);
    }

    public void OnOpenMenu()
    {
        gameObject.SetActive(true);
        combatActionSubmenu.gameObject.SetActive(false);
    }

    private void OpenSubmenu(CombatActionType actionType)
    {
        combatActionType = actionType;
        combatActionSubmenu.UpdateActions(cachedCombatant);
        combatActionSubmenu.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OnSubmenuActionSelected(int index)
    {
        OnMenuActionSelected?.Invoke(combatActionType, index);
    }
}
