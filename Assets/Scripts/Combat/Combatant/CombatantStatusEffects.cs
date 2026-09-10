using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Status Effect container for characters.
/// </summary>
public class CombatantStatusEffects
{
    private List<StatusEffect> statusEffects = new();

    public void AddEffect(StatusEffect statusEffect, int stacks)
    {
        StatusEffect existingEffect = statusEffects.FirstOrDefault(x => x.StatusEffectType == statusEffect.StatusEffectType);

        if (existingEffect == null)
        {
            statusEffects.Add(statusEffect);
            if (statusEffect is StackableStatusEffect stackableEffect)
                stackableEffect.AddStacks(stacks - 1);

            Debug.Log($"{statusEffect.StatusEffectType} was added.");
        }
        else if (existingEffect is StackableStatusEffect stackableEffect)
        {
            stackableEffect.AddStacks(stacks);
            Debug.Log($"{statusEffect.StatusEffectType} stacks updated to {stackableEffect.StackCount}/{stackableEffect.MaxStacks}.");
        }
        else
        {
            Debug.Log($"{statusEffect.StatusEffectType} already exists.");
        }
    }

    public void RemoveEffect(StatusEffectType statusEffectType)
    {
        StatusEffect statusEffect = statusEffects.FirstOrDefault(x => x.StatusEffectType == statusEffectType);

        if (statusEffect != null)
        {
            statusEffects.Remove(statusEffect);
            Debug.Log($"{statusEffectType} was removed.");
        }
    }

    public int GetStackCount(StatusEffectType effectType)
    {
        StatusEffect statusEffect = statusEffects.FirstOrDefault(x => x.StatusEffectType == effectType);
        return statusEffect is StackableStatusEffect stackableEffect ? stackableEffect.StackCount : 0;
    }

    public bool HasStatusEffect(StatusEffectType effectType) => statusEffects.Any(x => x.StatusEffectType == effectType);
}