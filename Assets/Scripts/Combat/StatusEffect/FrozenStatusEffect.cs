using UnityEngine;

public class FrozenStatusEffect : StatusEffect, IStackable
{
    private int currentStacks = 1;

    protected FrozenStatusEffectSO source;

    public override StatusEffectType StatusEffectType => source.StatusEffectType;
    public int StackCount => currentStacks;
    public int MaxStacks => source.MaxStacks;
    public int StacksOnAdd => source.StacksPerApplication;

    public FrozenStatusEffect(FrozenStatusEffectSO source)
    {
        this.source = source;
    }

    public override void Apply(ITarget target)
    {
        if (target is Combatant combatant)
        {
            Debug.Log($"{combatant.name} is affected by {StatusEffectType} ({currentStacks}/{source.MaxStacks} stacks).");
        }
    }

    public void AddStacks(int amount)
    {
        currentStacks += amount;
        if (currentStacks >= source.MaxStacks)
            currentStacks = source.MaxStacks;
    }

    public void RemoveStacks(int amount)
    {
        currentStacks -= amount;
        if (currentStacks < 0)
            currentStacks = 0;
    }
}