using System;
using UnityEngine;

// This can have a parent class called Character for basic information. Apart from that this should only contain combat related code.
public class Combatant : MonoBehaviour, ITarget
{
    [SerializeField] private float maxHP;
    [SerializeField] private Combatant target;
    
    // TODO: Temporary visualizer just to make selection more visible in terms of what is the attacker and what is the targets
    public TargetSelectionVisualizer Visualizer;

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

    public void SetTarget(Combatant target)
    {
        this.target = target;
    }

    public Combatant GetTarget(Battle _)
    {
        return target;
    }

    public TargetSelectionVisualizer GetSelectionVisualizer()
    {
        return Visualizer;
    }

    public GameObject GetRootObject()
    {
        return gameObject;
    }
}
