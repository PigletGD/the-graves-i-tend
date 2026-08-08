using UnityEngine;
using UnityEngine.InputSystem;

public class CombatantSelector : MonoBehaviour
{
    [SerializeField] private Battle battle;
    
    // TODO: Temporary creation of input actions. We'll eventually need a centralized player controls reference to pass around.
    private InputAction leftClickAction;
    private InputAction rightClickAction;

    private void Awake()
    {
        leftClickAction = new InputAction(binding: "<Mouse>/leftButton");
        rightClickAction = new InputAction(binding: "<Mouse>/rightButton");
    }

    private void OnEnable()
    {
        leftClickAction.performed += OnLeftClickPressed;
        leftClickAction.Enable();
        
        rightClickAction.performed += OnRightClickPressed;
        rightClickAction.Enable();
    }

    private void OnDisable()
    {
        leftClickAction.performed -= OnLeftClickPressed;
        leftClickAction.Disable();
        
        rightClickAction.performed -= OnRightClickPressed;
        rightClickAction.Disable();
    }

    private void OnLeftClickPressed(InputAction.CallbackContext ctx)
    {
        var selectedCombatant = GetHoveredCombatant();
        if (selectedCombatant == null)
            return;
        
        battle.HandleAttackerSetupForSelected(selectedCombatant);
    }
    
    private void OnRightClickPressed(InputAction.CallbackContext ctx)
    {
        var selectedCombatant = GetHoveredCombatant();
        if (selectedCombatant == null)
            return;
        
        battle.HandleDefenderSetupForSelected(selectedCombatant);
    }

    private Combatant GetHoveredCombatant()
    {
        if (Camera.main == null)
            return null;
            
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
        
        return hit.collider?.GetComponent<Combatant>();
    }
}
