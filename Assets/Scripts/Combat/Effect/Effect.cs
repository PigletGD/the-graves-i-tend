using System;
using UnityEngine;

/// <summary>
/// Base class for applying effects to the target.
/// </summary>
// TODO: Calculations should be more robust. 
// Effect should have a struct/class that has a value, operation, and condition to allow robustness in formulating values in effects.
[Serializable]
public abstract class Effect
{
    public abstract void Apply(ITarget target);

    // Just to standardize logging.
    public virtual void Log(object message)
    {
        Debug.Log($"{GetType().Name} {message}!");
    }
}
