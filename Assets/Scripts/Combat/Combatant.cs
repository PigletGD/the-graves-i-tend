using System;
using UnityEngine;

// This can have a parent class called Character for basic information. Apart from that this should only contain combat related code.
public class Combatant : MonoBehaviour, ITarget
{
    [SerializeField] private float maxHP;
    [SerializeField] private Combatant[] targets;
    
    // TODO: Temporary visualizer just to make selection more visible in terms of what is the attacker and what is the targets
    public CombatantSelectionVisualizer Visualizer;

    private float currentHP;

    private void Awake()
    {
        Visualizer?.SetToUnselectedColor();
    }

    private void Start()
    {
        currentHP = maxHP;
    }

    public void UpdateHP(float hpValue)
    {
        currentHP = Mathf.Clamp(currentHP += hpValue, 0, maxHP);
        Debug.Log($"{name} is at {currentHP}HP! ");
    }

    public ITarget[] GetTargets(Battle _)
    {
        return targets;
    }
}
