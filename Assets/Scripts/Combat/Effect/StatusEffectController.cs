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
        // Add more code...
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