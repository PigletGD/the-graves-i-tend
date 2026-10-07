using System;
using TMPro;
using UnityEngine;

public class CombatantActionMenu : MonoBehaviour
{
    public event Action<CombatActionType, int> OnMenuActionSelected;
    public event Action OnTargetingCancelRequested;

    [SerializeField] private CombatantActionSubmenu combatActionSubmenu;

    // TODO: Have these in its own class.
    [SerializeField] private Transform combatantActionDescriptionMenu;
    [SerializeField] private TMP_Text combatantActionDescription;

    private Combatant cachedCombatant;
    private CombatActionType combatActionType;
    private RectTransform rectTransform;
    private RectTransform parentRectTransform;
    private Canvas canvas;
    private bool positionPending;

    private void Awake()
    {
        rectTransform = (RectTransform)transform;
        parentRectTransform = (RectTransform)transform.parent;
        canvas = GetComponentInParent<Canvas>();
        combatActionSubmenu.OnActionButtonSelected += OnSubmenuActionSelected;
    }

    private void OnDestroy()
    {
        combatActionSubmenu.OnActionButtonSelected -= OnSubmenuActionSelected;
    }

    private void OnEnable()
    {
        positionPending = true;
        HideSubmenus();
    }

    // UI does not update properly; similar to EnemyCombatantPanel.
    private void LateUpdate()
    {
        if (!positionPending)
            return;

        positionPending = false;
        PositionAtCombatant();
    }

    public void SetCombatant(Combatant combatant)
    {
        cachedCombatant = combatant;

        if (combatant != null && combatant.IsPlayerControlled)
            PositionAtCombatant();
    }

    private void PositionAtCombatant()
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(cachedCombatant.BoxCollider.bounds.center);
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, screenPosition, canvas.worldCamera, out Vector2 localPosition))
        {
            rectTransform.anchoredPosition = localPosition;
            ((RectTransform)combatActionSubmenu.transform).anchoredPosition = localPosition;
            ((RectTransform)combatantActionDescriptionMenu).anchoredPosition = localPosition;
        }
    }

    public void OnAttackButton()
    {
        OnMenuActionSelected?.Invoke(CombatActionType.Attack, -1);
    }

    public void OnSkipButton()
    {
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

    public void OnCloseSubmenuButton()
    {
        gameObject.SetActive(true);
        OnTargetingCancelRequested?.Invoke();
    }

    private void OpenSubmenu(CombatActionType actionType)
    {
        combatActionType = actionType;
        combatActionSubmenu.UpdateActions(cachedCombatant);

        gameObject.SetActive(false);
        combatActionSubmenu.gameObject.SetActive(true);
        combatantActionDescriptionMenu.gameObject.SetActive(false);
        OnTargetingCancelRequested?.Invoke();
    }

    public void HideSubmenus()
    {
        combatActionSubmenu.gameObject.SetActive(false);
        combatantActionDescriptionMenu.gameObject.SetActive(false);
    }

    private void OnSubmenuActionSelected(int index)
    {
        combatantActionDescription.SetText($"{cachedCombatant.CharacterData.Skills[index].Description}");
        combatantActionDescriptionMenu.gameObject.SetActive(true);

        OnMenuActionSelected?.Invoke(combatActionType, index);
    }
}
