using System;
using UnityEngine;

[Serializable]
public sealed class CombatantEffectTypeCondition : ICondition<Combatant>
{
    [SerializeField] private bool hasEffect = true;
    [SerializeField] private EffectType effectType;

    public EffectType EffectType => effectType;

    public bool Check(Combatant combatant)
    {
        return hasEffect ? combatant.HasEffect(effectType) : !combatant.HasEffect(effectType);
    }
}