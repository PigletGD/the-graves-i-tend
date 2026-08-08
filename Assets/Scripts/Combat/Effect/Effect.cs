using System;

[Serializable]
public abstract class Effect
{
    public ITarget target;
    public abstract void Apply(Battle battle);
}