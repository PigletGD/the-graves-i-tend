using System;
using UnityEngine;

/// <summary>
/// Base class for applying effects to the target.
/// </summary>
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
