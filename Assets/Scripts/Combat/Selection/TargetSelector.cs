using UnityEngine;
using UnityEngine.InputSystem;

public class TargetSelector : MonoBehaviour
{
    // TODO: Temporary creation of input actions. We'll eventually need a centralized player controls reference to pass around.
    private InputAction leftClickAction;
    private InputAction pointAction;

    private ITarget currentHoveredTarget;

    private bool isTargetingSelectorEnabled = true; // Temporarily True

    private void Awake()
    {
        leftClickAction = new InputAction(binding: "<Mouse>/leftButton");
        pointAction = new InputAction(binding: "<Mouse>/position");
    }
    
    private void OnEnable()
    {
        leftClickAction.performed += OnLeftClick;
        leftClickAction.Enable();
        
        pointAction.performed += OnPoint;
        pointAction.Enable();
    }

    private void OnDisable()
    {
        leftClickAction.performed -= OnLeftClick;
        leftClickAction.Disable();
        
        pointAction.performed -= OnPoint;
        pointAction.Disable();
    }

    public void SetTargetingSelectorEnabled(bool isEnabled)
    {
        isTargetingSelectorEnabled = isEnabled;
    }

    public void OnLeftClick(InputAction.CallbackContext _)
    {
        
    }

    public void OnPoint(InputAction.CallbackContext _)
    {
        if (!isTargetingSelectorEnabled)
            return;

        ITarget newHoveredTarget = GetHoveredTarget();
        if (currentHoveredTarget == newHoveredTarget)
            return;

        if (currentHoveredTarget != null)
            HideTargetVisualizer(currentHoveredTarget);

        currentHoveredTarget = newHoveredTarget;
        if (currentHoveredTarget != null)
            ShowTargetVisualizer(currentHoveredTarget);
    }

    private void ShowTargetVisualizer(ITarget target)
    {
        target.GetSelectionVisualizer().SetToHoveredColor();
    }
    
    private void HideTargetVisualizer(ITarget target)
    {
        target.GetSelectionVisualizer().SetToUnselectedColor();
    }

    private ITarget GetHoveredTarget()
    {
        if (Camera.main == null)
            return null;
            
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
        
        return hit.collider?.GetComponent<ITarget>();
    }
}
