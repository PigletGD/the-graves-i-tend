using System;
using UnityEngine;

/// <summary>
/// Effect cares about applying an effect to the intended target.
/// </summary>
[Serializable]
public abstract class Effect
{
    public abstract void Apply(ITarget target);


    // Just to standardize logging.
    public virtual void Log(object message)
    {
        Debug.Log($"{this} {message}!");
    }
}
