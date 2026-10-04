using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TargetSelector : MonoBehaviour
{
    public event Action<ITarget[]> OnTargetsSelected;

    private bool isTargetingEnabled;
    private ITarget currentHoveredTarget;
    private List<ITarget> targetableTargets = new();
    private bool isTargetingAll;
    private HashSet<ITarget> visualizedTargets = new();
 
    // TODO: Temporary creation of input actions. We'll eventually need a centralized player controls reference to pass around.
    private InputAction leftClickAction;
    private InputAction pointAction;

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

    public void SetTargetingSelectorEnabled(bool isEnabled, TargetSelectionMode visualTargetingMode = TargetSelectionMode.None, ITarget[] targets = null)
    {
        isTargetingEnabled = isEnabled;
        targetableTargets.Clear();
        currentHoveredTarget = null;

        isTargetingAll = visualTargetingMode == TargetSelectionMode.AllAllies || visualTargetingMode == TargetSelectionMode.AllEnemies;
        if (isEnabled)
            targetableTargets.AddRange(targets);

        ClearTargetVisualizers();
        if (isEnabled && isTargetingAll)
            foreach (ITarget target in targetableTargets)
                ShowTargetVisualizer(target);
    }

    public void OnLeftClick(InputAction.CallbackContext _)
    {
        if (!isTargetingEnabled)
            return;

        if (TryGetHoveredTarget(out ITarget target))
        {
            if (isTargetingAll)
                OnTargetsSelected?.Invoke(targetableTargets.ToArray());
            else
                OnTargetsSelected?.Invoke(new[] { target });
        }
    }

    public void OnPoint(InputAction.CallbackContext _)
    {
        if (!isTargetingEnabled || isTargetingAll)
            return;

        ITarget newHoveredTarget = TryGetHoveredTarget(out ITarget hoveredTarget) ? hoveredTarget : null;
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
        visualizedTargets.Add(target);
        target.GetSelectionVisualizer().SetToHoveredColor();
    }
    
    private void HideTargetVisualizer(ITarget target)
    {
        target.GetSelectionVisualizer().SetToUnselectedColor();
        visualizedTargets.Remove(target);
    }

    private bool TryGetHoveredTarget(out ITarget target)
    {
        target = null;
        if (Camera.main == null)
            return false;

        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

        if (!hit || hit.collider == null || !hit.collider.TryGetComponent(out target))
            return false;

        return targetableTargets.Contains(target);
    }

    private void ClearTargetVisualizers()
    {
        foreach (ITarget target in visualizedTargets)
            target.GetSelectionVisualizer().SetToUnselectedColor();

        visualizedTargets.Clear();
        currentHoveredTarget = null;
    }
}
