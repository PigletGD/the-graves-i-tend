using System;
using UnityEngine;

[Serializable]
public class Effect_RemoveStatus : Effect
{
    [SerializeField] private StatusEffectType statusEffectType;

    public override void Apply(ITarget target)
    {
        if (target is not Combatant combatant)
        {
            Log("failed because Target is not a combatant");
            return;
        }

        combatant.StatusEffects.RemoveEffect(statusEffectType);
    }
}
