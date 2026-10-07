using System;
using UnityEngine;

/// <summary>
/// ScriptableObject asset definition for a status effect, exposing its type and creating runtime instances.
/// </summary>
// TODO: Go back to StatusEffect and see if this is actually relevant because SerializeReference exists. The only issue I see is the max stacks for Stackable Effects.
[Serializable]
public abstract class StatusEffectSO : ScriptableObject
{
    public abstract StatusEffectType StatusEffectType { get; }
    public abstract StatusEffect CreateInstance();
}