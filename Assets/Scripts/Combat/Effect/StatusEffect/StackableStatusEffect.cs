using System;

/// <summary>
/// Base class for status effects that can stack, tracking the current count and enforcing minimum/maximum limits.
/// </summary>
[Serializable]
public abstract class StackableStatusEffect : StatusEffect, IStackable
{
    protected StatusEffectType statusEffectType;
    protected int currentStacks;
    protected int maxStacks;
    protected int stacksOnAdd; 

    public int StackCount => currentStacks;

    public int MaxStacks => maxStacks;

    public int StacksOnAdd => stacksOnAdd;

    public void AddStacks(int amount)
    {
        currentStacks += amount;
        if (currentStacks >= maxStacks)
            currentStacks = maxStacks;
    }

    public void RemoveStacks(int amount)
    {
        currentStacks -= amount;
        if (currentStacks < 0)
            currentStacks = 0;
    }
}