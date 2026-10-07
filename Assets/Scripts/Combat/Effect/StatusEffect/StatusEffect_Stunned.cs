using UnityEngine;

public class StatusEffect_Stunned : StatusEffect
{
    public override StatusEffectType StatusEffectType => statusEffectType;

    public StatusEffect_Stunned(StatusEffectSO_Stunned source)
    {
        statusEffectType = source.StatusEffectType;
    }

    public override void Apply(ITarget target)
    {
        if (target is Combatant combatant)
        {
            Debug.Log($"{combatant.name} was {StatusEffectType}.");
        }
    }
}
