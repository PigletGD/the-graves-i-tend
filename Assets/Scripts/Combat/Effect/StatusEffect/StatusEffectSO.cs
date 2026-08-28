using System;
using UnityEngine;

/// <summary>
/// ScriptableObject asset definition for a status effect, exposing its type and creating runtime instances.
/// </summary>
[Serializable]
public abstract class StatusEffectSO : ScriptableObject
{
    public abstract StatusEffectType StatusEffectType { get; }
    public abstract StatusEffect CreateInstance();
}