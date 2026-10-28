using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TargetSelector : MonoBehaviour
{
    public event Action<ITarget[]> OnTargetsSelected;
    public event Action<ITarget> OnHoveredTargetChanged;

    [SerializeField] private CombatArena combatArena;

    private bool isTargetingEnabled;
    private ITarget currentHoveredTarget;
    private List<ITarget> targetableTargets = new();
    private bool isTargetingAll;
    private bool isTargetingRow;
    private bool isTargetingColumn;
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
        OnHoveredTargetChanged?.Invoke(currentHoveredTarget);

        isTargetingAll = visualTargetingMode == TargetSelectionMode.AllAllies || visualTargetingMode == TargetSelectionMode.AllEnemies;
        isTargetingRow = visualTargetingMode == TargetSelectionMode.RowAllies || visualTargetingMode == TargetSelectionMode.RowEnemies;
        isTargetingColumn = visualTargetingMode == TargetSelectionMode.ColumnAllies || visualTargetingMode == TargetSelectionMode.ColumnEnemies;
        if (isEnabled)
        {
            foreach (ITarget target in targets)
                targetableTargets.Add(target);
        }

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
            {
                if (targetableTargets.Count > 0)
                    OnTargetsSelected?.Invoke(targetableTargets.ToArray());
            }
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
        
        /*if (currentHoveredTarget != null)
            HideTargetVisualizer(currentHoveredTarget);*/
        
        if (targetableTargets != null && targetableTargets.Count > 0)
            foreach (var target in targetableTargets)
                HideTargetVisualizer(target);
        
        targetableTargets.Clear();

        currentHoveredTarget = newHoveredTarget;
        if (currentHoveredTarget != null)
        {
            if (isTargetingRow)
                targetableTargets = combatArena.GetTargetsInRow(currentHoveredTarget as Combatant, true);
            else if (isTargetingColumn)
                targetableTargets = combatArena.GetTargetsInColumn(currentHoveredTarget as Combatant, true);
            else
                targetableTargets = new List<ITarget>{currentHoveredTarget};
            
            if (targetableTargets != null && targetableTargets.Count > 0)
            {
                var combatants = new List<Combatant>();
                foreach (var target in targetableTargets)
                {
                    ShowTargetVisualizer(target);
                    if (target is Combatant combatant)
                        combatants.Add(combatant);
                }
                
                if (combatants.Count > 0)
                    combatArena.SetHoveredCombatants(combatants);
            }
        }

        OnHoveredTargetChanged?.Invoke(currentHoveredTarget);
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
