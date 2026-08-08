using System;
using UnityEngine;

public class CombatantSelectionVisualizer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    [SerializeField] private Color unselectedColor;
    [SerializeField] private Color attackerColor;
    [SerializeField] private Color defenderColor;

    private void Awake()
    {
        SetToUnselectedColor();
    }

    public void SetToUnselectedColor()
    {
        spriteRenderer.color = unselectedColor;
    }

    public void SetToAttackerColor()
    {
        spriteRenderer.color = attackerColor;
    }

    public void SetToDefenderColor()
    {
        spriteRenderer.color = defenderColor;
    }
}
