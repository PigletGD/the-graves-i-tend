using System;
using UnityEngine;

[Serializable]
public class Effect_Damage : Effect
{
    [SerializeField] private float damage = 10f;

    public override void Apply(ITarget target)
    {
        if (target is not Combatant combatant)
        {
            Log("failed because Target is not a combatant");
            return;
        }

        combatant.TakeDamage(damage);
    }
}