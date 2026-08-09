using System;
using UnityEngine;

/// <summary>
/// Effect only cares about applying it's effect to the intended target.
/// </summary>
[Serializable]
public abstract class Effect : ScriptableObject
{
    public abstract void Apply(ITarget target);

    // Just to standardize logging.
    public virtual void Log(object message)
    {
        Debug.Log($"{name} {message}!");
    }
}