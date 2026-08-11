using System;
using UnityEngine;

/// <summary>
/// StatusEffect are effects that stays with the combatant.
/// </summary>
[Serializable]
public abstract class StatusEffect : ScriptableObject
{
    public abstract StatusEffectType StatusEffectType { get; }
}

public enum StatusEffectType
{
    None,
    Frozen
}