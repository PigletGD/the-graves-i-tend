using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Status Effect container for characters.
/// </summary>
public class CombatantStatusEffects
{
    private Combatant target; // Temporary
    private List<StatusEffect> statusEffects = new();

    public void AddEffect(StatusEffect statusEffect)
    {
        StatusEffect existingEffect = statusEffects.FirstOrDefault(x => x.StatusEffectType == statusEffect.StatusEffectType);

        if (existingEffect == null)
        {
            statusEffects.Add(statusEffect);
            Debug.Log($"{statusEffect.StatusEffectType} was added.");
        }
        else
        {
            if (existingEffect is StackableStatusEffect stackableEffect)
            {
                stackableEffect.AddStacks(stackableEffect.StacksOnAdd);
                Debug.Log($"{statusEffect.StatusEffectType} stacks updated to {stackableEffect.StackCount}/{stackableEffect.MaxStacks}.");
            }
            else
            {
                Debug.Log($"{statusEffect.StatusEffectType} already exists and cannot stack.");
            }
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

    public void ApplyAllStatusEffects()
    {
        foreach(StatusEffect statusEffect in statusEffects)
            statusEffect.Apply(target);
    }

    public int GetStackCount(StatusEffectType effectType)
    {
        StatusEffect statusEffect = statusEffects.FirstOrDefault(x => x.StatusEffectType == effectType);
        return statusEffect is StackableStatusEffect stackableEffect ? stackableEffect.StackCount : 0;
    }

    public bool HasStatusEffect(StatusEffectType effectType) => statusEffects.Any(x => x.StatusEffectType == effectType);
}