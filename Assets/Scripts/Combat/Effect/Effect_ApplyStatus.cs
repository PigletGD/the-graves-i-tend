using System;
using UnityEngine;

[Serializable]
public class Effect_ApplyStatus : Effect
{
    [SerializeField] private StatusEffectSO statusEffectSO;
    [SerializeField] private int stacksOnApply = 1; // Only relevant to stacking status effects.

    public override void Apply(ITarget target)
    {
        if (target is not Combatant combatant)
        {
            Log("failed because Target is not a combatant");
            return;
        }

        combatant.StatusEffects.AddEffect(statusEffectSO.CreateInstance(), stacksOnApply);
    }
}
