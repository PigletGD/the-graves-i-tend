using System;
using UnityEngine;

[Serializable]
public class SunderStatusEffect : StackableStatusEffect
{
    public override StatusEffectType StatusEffectType => statusEffectType;

    public SunderStatusEffect(SunderStatusEffectSO source)
    {
        currentStacks = 1;
        statusEffectType = source.StatusEffectType;
        maxStacks = source.MaxStacks;
        stacksOnAdd = source.StacksPerApplication;
    }

    public override void Apply(ITarget target)
    {
        if (target is Combatant combatant)
        {
            Debug.Log($"{combatant.name} is affected by {StatusEffectType} ({currentStacks}/{maxStacks} stacks).");
        }
    }
}
