using System;
using UnityEngine;

[Serializable]
public sealed class CombatantEffectCondition : ICondition<Combatant>
{
    [SerializeField] private bool hasEffect = true;
    [SerializeField] private Effect effect;

    public Effect Effect => effect;

    public bool Check(Combatant combatant)
    {
        return hasEffect ? combatant.HasEffect(effect) : !combatant.HasEffect(effect);
    }
}