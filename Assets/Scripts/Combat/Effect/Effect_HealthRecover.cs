using System;
using UnityEngine;

// Refer to TODO note in Effect.
[Serializable]
public class Effect_HealthRecover : Effect
{
    [SerializeField] private float percentage = 0.5f;

    public override void Apply(ITarget target)
    {
        if (target is not Combatant combatant)
        {
            Log("failed because Target is not a combatant");
            return;
        }

        // Log($"dealt {damage} damage");
        combatant.RecoverHealth(combatant.Stats.MaxHP * percentage);
    }
}