using System;
using UnityEngine;

[Serializable]
public class Condition_Combatant_StatusEffect : Condition_Combatant
{
    [SerializeField] private bool hasEffect = true;
    [SerializeField] private StatusEffectType effectType;

    public StatusEffectType EffectType => effectType;

    public override bool Check(Combatant combatant)
    {
        return hasEffect ? combatant.StatusEffects.HasStatusEffect(effectType) : !combatant.StatusEffects.HasStatusEffect(effectType);
    }
}