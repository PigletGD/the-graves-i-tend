using System;
using UnityEngine;

public class CombatantSelectionVisualizer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    [SerializeField] private Color unselectedColor;
    [SerializeField] private Color hoveredColor;
    [SerializeField] private Color attackerColor;
    [SerializeField] private Color defenderColor;
    
    public bool IsHovered { get; set; }
    public bool IsSelected { get; set; }

    private void Awake()
    {
        SetToUnselectedColor();
    }

    public void SetToUnselectedColor()
    {
        spriteRenderer.color = unselectedColor;
        IsSelected = false;
    }

    public void SetToHoveredColor(bool setAsUnselected = false)
    {
        if (setAsUnselected)
            IsSelected = false;
        else if (IsSelected)
            return;
        
        spriteRenderer.color = hoveredColor;
    }

    public void SetToAttackerColor()
    {
        spriteRenderer.color = attackerColor;
        IsSelected = true;
    }

    public void SetToDefenderColor()
    {
        spriteRenderer.color = defenderColor;
        IsSelected = true;
    }
}
