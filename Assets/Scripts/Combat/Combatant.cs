using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// This can have a parent class called Character for basic information. Apart from that this should only contain combat related code.
public class Combatant : MonoBehaviour, ITarget
{
    [SerializeField] private float maxHP;
    [SerializeField] private Combatant[] targets;
    
    // TODO: Temporary visualizer just to make selection more visible in terms of what is the attacker and what is the targets
    public TargetSelectionVisualizer Visualizer;

    private float currentHP;
    [SerializeField] private List<Effect> effects = new();

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
        Debug.Log($"{name} is at {currentHP}HP!");
    }

    public ITarget[] GetTargets(Battle _)
    {
        return targets;
    }

    public TargetSelectionVisualizer GetSelectionVisualizer()
    {
        return Visualizer;
    }

    public GameObject GetRootObject()
    {
        return gameObject;
    }

    // TODO: Effect stacking.
    public void AddEffect(Effect effect)
    {
        if (!HasEffect(effect))
        {
            Debug.Log($"{effect.name} was added.");
            effects.Add(effect);
        }
    }

    public void RemoveEffect(Effect effect)
    {
        if (HasEffect(effect))
        {
            Debug.Log($"{effect.name} was removed.");
            effects.Remove(effect);
        }
    }

    public bool HasEffect(Effect effect) => effects.Contains(effect);
    
}
