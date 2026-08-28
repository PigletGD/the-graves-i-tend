using UnityEngine;

/// <summary>
/// Base class for applying status effect to a target.
/// </summary>
public abstract class StatusEffect
{
    public abstract StatusEffectType StatusEffectType { get; }

    public abstract void Apply(ITarget target);

    // Just to standardize logging.
    public virtual void Log(object message)
    {
        Debug.Log($"{GetType().Name} {message}!");
    }
}