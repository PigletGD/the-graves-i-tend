using System;
using UnityEngine;

/// <summary>
/// StatusEffect are effects that stays with the combatant.
/// </summary>
[Serializable]
public abstract class StatusEffect : ScriptableObject, IEffect
{
    public abstract StatusEffectType StatusEffectType { get; }
    public abstract void Apply(ITarget target);
}

public enum StatusEffectType
{
    None,
    Frozen
}
