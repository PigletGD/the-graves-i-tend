using System;
using UnityEngine;

[Serializable]
public abstract class Effect : ScriptableObject
{
    public abstract void Apply(Battle battle);
}