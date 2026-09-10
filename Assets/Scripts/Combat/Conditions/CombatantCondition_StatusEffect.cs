using System;
using UnityEngine;

[Serializable]
public class CombatantCondition_StatusEffect : CombatantCondition
{
    [SerializeField] private bool hasEffect = true;
    [SerializeField] private StatusEffectType effectType;

    public StatusEffectType EffectType => effectType;

    public override bool Check(Combatant combatant)
    {
        return hasEffect ? combatant.StatusEffects.HasStatusEffect(effectType) : !combatant.StatusEffects.HasStatusEffect(effectType);
    }
}