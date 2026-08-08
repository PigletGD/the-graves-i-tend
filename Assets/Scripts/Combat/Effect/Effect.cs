using System;

[Serializable]
public abstract class Effect
{
    public abstract void Apply(Battle battle);
}