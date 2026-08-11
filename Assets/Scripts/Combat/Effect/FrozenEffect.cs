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

        FrozenStatusEffect runtimeEffect = ScriptableObject.CreateInstance<FrozenStatusEffect>();
        combatant.EffectController.AddEffect(runtimeEffect);
    }
}

[Serializable, CreateAssetMenu(fileName = "Status Effect", menuName = "Status Effect")]
public class FrozenStatusEffect : StatusEffect, IStackable
{
    public override StatusEffectType StatusEffectType => StatusEffectType.Frozen;

    [SerializeField] private int maxStacks = 3;
    [SerializeField, ReadOnly] private int currentStacks = 1;

    public override void Apply(ITarget target)
    {
        if (target is Combatant combatant)
        {
            Debug.Log($"{combatant.name} is affected by {StatusEffectType} ({currentStacks}/{maxStacks} stacks).");
        }
    }

    public void AddStacks(int count)
    {
        currentStacks += count;
        if (currentStacks >= maxStacks)
            currentStacks = maxStacks;
    }

    public void RemoveStacks(int count)
    {
        currentStacks -= count;
        if (currentStacks < 0)
            currentStacks = 0;
    }

    public int GetCurrentStacks()
    {
        return currentStacks;
    }

    public int GetMaxStacks()
    {
        return maxStacks;
    }
}