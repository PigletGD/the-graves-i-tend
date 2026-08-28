using System;

/// <summary>
/// Base class for applying status effect to a target.
/// </summary>
[Serializable]
public abstract class StatusEffect : Effect
{
    public abstract StatusEffectType StatusEffectType { get; }
}