using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

/// <summary>
/// Effect container for characters.
/// </summary>
public class StatusEffectController : MonoBehaviour
{
    // Serialize just so that we have visual of what effects are added.
    [SerializeField, ReadOnly] private List<StatusEffect> statusEffects = new();

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
            if (existingEffect is IStackable stackableEffect)
            {
                stackableEffect.AddStacks(1);
                Debug.Log($"{statusEffect.StatusEffectType} stacks updated to {stackableEffect.GetCurrentStacks()}/{stackableEffect.GetMaxStacks()}.");
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

    public bool HasStatusEffect(StatusEffectType effectType) => statusEffects.Any(x => x.StatusEffectType == effectType);
}