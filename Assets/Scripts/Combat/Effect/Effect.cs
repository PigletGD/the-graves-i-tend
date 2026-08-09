using System;
using UnityEngine;

/// <summary>
/// Effect only cares about applying it's effect to the intended target.
/// </summary>
[Serializable]
public abstract class Effect : ScriptableObject
{
    public abstract void Apply(ITarget target);
}