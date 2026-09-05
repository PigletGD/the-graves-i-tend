using UnityEngine;
using UnityEngine.InputSystem;

public class TargetSelector : MonoBehaviour
{
    [SerializeField] private Combat combat;
    
    // TODO: Temporary creation of input actions. We'll eventually need a centralized player controls reference to pass around.
    private InputAction leftClickAction;
    private InputAction rightClickAction;
    
    private ITarget hoveredCombatant;

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

    private void Update()
    {
        var newHoveredCombatant = GetHoveredCombatant();
        if (hoveredCombatant != null && hoveredCombatant.Equals(newHoveredCombatant))
            return;

        if (hoveredCombatant?.GetSelectionVisualizer() != null && !hoveredCombatant.GetSelectionVisualizer().IsSelected)
        {
            hoveredCombatant.GetSelectionVisualizer().IsHovered = true;
            hoveredCombatant.GetSelectionVisualizer().SetToUnselectedColor();
        }
        
        if (newHoveredCombatant?.GetSelectionVisualizer() != null && !newHoveredCombatant.GetSelectionVisualizer().IsSelected)
        {
            newHoveredCombatant.GetSelectionVisualizer().IsHovered = true;
            newHoveredCombatant.GetSelectionVisualizer().SetToHoveredColor();
        }
        
        hoveredCombatant = newHoveredCombatant;
    }

    private void OnLeftClickPressed(InputAction.CallbackContext ctx)
    {
        if (hoveredCombatant == null)
            return;
        
        combat.HandleAttackerSetupForSelected(hoveredCombatant);
    }
    
    private void OnRightClickPressed(InputAction.CallbackContext ctx)
    {
        if (hoveredCombatant == null)
            return;
        
        combat.HandleDefenderSetupForSelected(hoveredCombatant);
    }

    private ITarget GetHoveredCombatant()
    {
        if (Camera.main == null)
            return null;
            
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
        
        return hit.collider?.GetComponent<ITarget>();
    }
}
