using System;
using UnityEngine;

[Serializable]
public class CombatantEffectTypeCondition : CombatantCondition
{
    [SerializeField] private bool hasEffect = true;
    [SerializeField] private StatusEffectType effectType;

    public StatusEffectType EffectType => effectType;

    public override bool Check(Combatant combatant)
    {
        return hasEffect ? combatant.EffectController.HasStatusEffect(effectType) : !combatant.EffectController.HasStatusEffect(effectType);
    }
}