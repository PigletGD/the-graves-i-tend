using System;
using Unity.Collections;
using UnityEngine;

[Serializable]
public class FrozenEffect : Effect
{
    [SerializeField] private FrozenStatusEffect frozenStatusEffect;
    [SerializeField] private ProbabilityCondition<float> applyChance;
    [SerializeField] private CombatantEffectTypeCondition[] effectConditions;

    public override void Apply(ITarget target)
    {
        if (target is not Combatant combatant)
        {
            Log("failed because Target is not a combatant");
            return;
        }

        if (!applyChance.Check(0))
        {
            Log("missed");
            return;
        }

        foreach (CombatantEffectTypeCondition effectCondition in effectConditions)
        {
            if (!effectCondition.Check(combatant))
            {
                Log($"failed because of {effectCondition.EffectType}");
                return;
            }
        }

        combatant.EffectController.AddEffect(frozenStatusEffect);
    }
}

[Serializable, CreateAssetMenu(fileName = "Status Effect", menuName = "Status Effect")]
public class FrozenStatusEffect : StatusEffect, IStackable
{
    [SerializeField] private int maxStacks = 3;
    [SerializeField, ReadOnly] private int stacks;

    public override StatusEffectType StatusEffectType => StatusEffectType.Frozen;

    public FrozenStatusEffect()
    {
        stacks = 1;

        Debug.Log($"{this} was added.");
    }

    public void AddStacks(int count)
    {
        stacks += count;
        if (stacks >= maxStacks)
        {
            stacks = maxStacks;
            Debug.Log($"{this} is at max stacks: {maxStacks}.");
        }
        else
            Debug.Log($"{this} added a stack.");
    }

    public void RemoveStacks(int count)
    {
        stacks -= count;
        Debug.Log($"{this} removed a stack.");
    }

    public int GetCurrentStacks()
    {
        throw new NotImplementedException();
    }

    public int GetMaxStacks()
    {
        throw new NotImplementedException();
    }
}